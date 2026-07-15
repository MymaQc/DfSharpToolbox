using Dragonfly;

namespace Toolbox.Blocks;

public static class BlockFactory
{
    private static (World.Block? Block, bool Ok) Get(string name, Dictionary<string, object?>? states = null)
    {
        return World.BlockByName(name, states);
    }

    public static (World.Block? Block, bool Ok) GetBlockByName(string name, Dictionary<string, object?>? states = null)
    {
        return Get(name, states);
    }

    private static bool TryGet(string name, out World.Block block, Dictionary<string, object?>? states = null)
    {
        var (value, ok) = Get(name, states);
        if (!ok || value is null)
        {
            block = null!;
            return false;
        }

        block = value;
        return true;
    }

    public static bool TryGetBlockByName(string name, out World.Block block, Dictionary<string, object?>? states = null)
    {
        return TryGet(name, out block, states);
    }

    public static World.Block RequireBlockByName(string name, Dictionary<string, object?>? states = null)
    {
        var (block, ok) = Get(name, states);
        if (!ok || block is null)
        {
            throw new ArgumentException($"Unknown block: {name}", nameof(name));
        }

        return block;
    }

    public static Dictionary<string, object?> CreateBlockStates(params (string Name, object? Value)[] states)
    {
        var values = new Dictionary<string, object?>(StringComparer.Ordinal);
        foreach (var (name, value) in states)
        {
            values[name] = value;
        }

        return values;
    }
}
