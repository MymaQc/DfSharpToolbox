using Dragonfly;

namespace Toolbox.Blocks;

public static class BlockApi
{
    public static World.SetOpts Options(bool disableBlockUpdates = false, bool disableLiquidDisplacement = false, bool disableRedstoneUpdates = false)
    {
        return new World.SetOpts
        {
            DisableBlockUpdates = disableBlockUpdates,
            DisableLiquidDisplacement = disableLiquidDisplacement,
            DisableRedstoneUpdates = disableRedstoneUpdates,
        };
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

    public static IEnumerable<Cube.Pos> FindNearby(World.Tx tx, Cube.Pos center, int radius, params World.Block[] blocks)
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

    public static void ScheduleUpdate(World.Tx tx, Cube.Pos position, World.Block block, TimeSpan delay)
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

    public static void AddParticle(World.Tx tx, Vector3 position, World.Particle particle)
    {
        tx.AddParticle(position, particle);
    }

    public static Cube.Pos FromVector(Vector3 position)
    {
        return Cube.PosFromVec3(position);
    }

    public static Cube.Pos Add(Cube.Pos position, int x = 0, int y = 0, int z = 0)
    {
        return position.Add(new Cube.Pos(x, y, z));
    }

    public static Cube.Pos GetSide(Cube.Pos position, Cube.Face face)
    {
        return position.Side(face);
    }
}
