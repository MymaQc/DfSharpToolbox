using Dragonfly;
using Toolbox.Items;
using Toolbox.Diagnostics;

namespace Toolbox.Blocks;

public static class CustomBlockApi
{
    private sealed record Callbacks(Action<CustomBlockInteractionContext>? Interact, Action<CustomBlockPlacementContext>? Placing);
    private static readonly Dictionary<string, Callbacks> RegisteredCallbacks = new(StringComparer.Ordinal);
    private static readonly Lock CallbackLock = new();
    public static CustomBlockBuilder Create(string identifier, string displayName, byte[] texturePng) =>
        new(identifier, displayName, texturePng);

    public static CustomBlockBuilder CreateEmbedded<TMarker>(string identifier, string displayName, string resourceName)
    {
        var assembly = typeof(TMarker).Assembly;
        using var stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new ArgumentException($"Embedded texture {resourceName} was not found in {assembly.GetName().Name}.", nameof(resourceName));
        using var buffer = new MemoryStream();
        stream.CopyTo(buffer);
        return Create(identifier, displayName, buffer.ToArray());
    }

    public static Item.Stack CreateStack(Block.Custom block, int count = 1) => Item.NewStack(block, count);
    public static ItemStackBuilder CreateBuilder(Block.Custom block, int count = 1) => new(CreateStack(block, count));

    internal static void RegisterCallbacks(Block.Custom block, Action<CustomBlockInteractionContext>? interact, Action<CustomBlockPlacementContext>? placing)
    {
        if (interact is null && placing is null)
        {
            return;
        }
        lock (CallbackLock)
        {
            RegisteredCallbacks[block.Identifier] = new Callbacks(interact, placing);
        }
    }

    internal static void DispatchInteract(Player.Context context, Cube.Pos position, Cube.Face face, Vector3 clickPosition)
    {
        if (context.Block(position) is not Block.Custom block)
        {
            return;
        }
        Callbacks? callbacks;
        lock (CallbackLock)
        {
            RegisteredCallbacks.TryGetValue(block.Identifier, out callbacks);
        }
        if (callbacks?.Interact is { } callback)
        {
            ToolboxLogger.Try(() => callback(new CustomBlockInteractionContext(context, position, face, clickPosition)), $"Custom block {block.Identifier} interact");
        }
    }

    internal static void DispatchPlacing(Player.Context context, Cube.Pos position, World.Block placed)
    {
        if (placed is not Block.Custom block)
        {
            return;
        }
        Callbacks? callbacks;
        lock (CallbackLock)
        {
            RegisteredCallbacks.TryGetValue(block.Identifier, out callbacks);
        }
        if (callbacks?.Placing is { } callback)
        {
            ToolboxLogger.Try(() => callback(new CustomBlockPlacementContext(context, position, block)), $"Custom block {block.Identifier} placing");
        }
    }

    internal static void ClearCallbacks() { lock (CallbackLock)
        {
            RegisteredCallbacks.Clear();
        }
    }
}
