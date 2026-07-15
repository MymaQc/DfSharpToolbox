using Dragonfly;

namespace Toolbox.Blocks;

public static class BlockApi
{
    public static World.SetOpts CreateSetOptions(bool disableBlockUpdates = false, bool disableLiquidDisplacement = false, bool disableRedstoneUpdates = false)
    {
        return new World.SetOpts
        {
            DisableBlockUpdates = disableBlockUpdates,
            DisableLiquidDisplacement = disableLiquidDisplacement,
            DisableRedstoneUpdates = disableRedstoneUpdates,
        };
    }

    public static (World.Block? Block, bool Ok) GetBlockByName(string name, Dictionary<string, object?>? states = null)
    {
        return BlockFactory.GetBlockByName(name, states);
    }

    public static World.Block RequireBlockByName(string name, Dictionary<string, object?>? states = null)
    {
        return BlockFactory.RequireBlockByName(name, states);
    }

    public static Cube.Range GetRange(World.Tx tx)
    {
        return tx.Range();
    }

    public static bool IsOutOfBounds(World.Tx tx, Cube.Pos position)
    {
        return position.OutOfBounds(tx.Range());
    }

    public static World.Block GetBlock(World.Tx tx, Cube.Pos position)
    {
        return tx.Block(position);
    }

    public static (World.Block? Block, bool Ok) GetLoadedBlock(World.Tx tx, Cube.Pos position)
    {
        return tx.BlockLoaded(position);
    }

    public static void SetBlock(World.Tx tx, Cube.Pos position, World.Block? block, World.SetOpts? options = null)
    {
        tx.SetBlock(position, block, options);
    }

    public static void RemoveBlock(World.Tx tx, Cube.Pos position, World.SetOpts? options = null)
    {
        tx.SetBlock(position, null, options);
    }

    public static IEnumerable<Cube.Pos> FindNearbyBlocks(World.Tx tx, Cube.Pos center, int radius, params World.Block[] blocks)
    {
        return tx.BlocksWithin(center, radius, blocks);
    }

    public static (World.Liquid? Liquid, bool Ok) GetLiquid(World.Tx tx, Cube.Pos position)
    {
        return tx.Liquid(position);
    }

    public static void SetLiquid(World.Tx tx, Cube.Pos position, World.Liquid? liquid)
    {
        tx.SetLiquid(position, liquid);
    }

    public static void RemoveLiquid(World.Tx tx, Cube.Pos position)
    {
        tx.SetLiquid(position, null);
    }

    public static void ScheduleBlockUpdate(World.Tx tx, Cube.Pos position, World.Block block, TimeSpan delay)
    {
        tx.ScheduleBlockUpdate(position, block, delay);
    }

    public static int GetHighestLightBlocker(World.Tx tx, int x, int z)
    {
        return tx.HighestLightBlocker(x, z);
    }

    public static int GetHighestBlock(World.Tx tx, int x, int z)
    {
        return tx.HighestBlock(x, z);
    }

    public static byte GetLight(World.Tx tx, Cube.Pos position)
    {
        return tx.Light(position);
    }

    public static byte GetSkyLight(World.Tx tx, Cube.Pos position)
    {
        return tx.SkyLight(position);
    }

    public static World.Biome GetBiome(World.Tx tx, Cube.Pos position)
    {
        return tx.Biome(position);
    }

    public static void SetBiome(World.Tx tx, Cube.Pos position, World.Biome biome)
    {
        tx.SetBiome(position, biome);
    }

    public static double GetTemperature(World.Tx tx, Cube.Pos position)
    {
        return tx.Temperature(position);
    }

    public static bool IsRainingAt(World.Tx tx, Cube.Pos position)
    {
        return tx.RainingAt(position);
    }

    public static bool IsSnowingAt(World.Tx tx, Cube.Pos position)
    {
        return tx.SnowingAt(position);
    }

    public static bool IsThunderingAt(World.Tx tx, Cube.Pos position)
    {
        return tx.ThunderingAt(position);
    }

    public static bool IsRaining(World.Tx tx)
    {
        return tx.Raining();
    }

    public static bool IsThundering(World.Tx tx)
    {
        return tx.Thundering();
    }

}
