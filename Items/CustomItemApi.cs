using Dragonfly;

namespace Toolbox.Items;

public static class CustomItemApi
{
    public static Item.Custom Register(
        string identifier,
        string displayName,
        byte[] texturePng,
        Item.CustomItemCategory category = Item.CustomItemCategory.Items,
        int maxStackSize = 64,
        string creativeGroup = "",
        Item.CustomItemData? data = null)
    {
        return Item.RegisterCustom(identifier, displayName, texturePng, category, maxStackSize, creativeGroup, data);
    }

    public static CustomItemBuilder Create(string identifier, string displayName, byte[] texturePng)
    {
        return new CustomItemBuilder(identifier, displayName, texturePng);
    }

    public static CustomItemBuilder CreateEmbedded<TMarker>(string identifier, string displayName, string resourceName)
    {
        return Create(identifier, displayName, ReadEmbedded<TMarker>(resourceName));
    }

    public static Item.Custom RegisterEmbedded<TMarker>(
        string identifier,
        string displayName,
        string resourceName,
        Item.CustomItemCategory category = Item.CustomItemCategory.Items,
        int maxStackSize = 64,
        string creativeGroup = "",
        Item.CustomItemData? data = null)
    {
        return Register(identifier, displayName, ReadEmbedded<TMarker>(resourceName), category, maxStackSize, creativeGroup, data);
    }

    public static Item.Stack CreateStack(Item.Custom item, int count = 1)
    {
        ArgumentNullException.ThrowIfNull(item);
        return Item.NewStack(item, count);
    }

    public static ItemStackBuilder CreateBuilder(Item.Custom item, int count = 1)
    {
        return new ItemStackBuilder(CreateStack(item, count));
    }

    private static byte[] ReadEmbedded<TMarker>(string resourceName)
    {
        var assembly = typeof(TMarker).Assembly;
        using var stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new ArgumentException($"Embedded texture {resourceName} was not found in {assembly.GetName().Name}.", nameof(resourceName));
        using var buffer = new MemoryStream();
        stream.CopyTo(buffer);
        return buffer.ToArray();
    }
}
