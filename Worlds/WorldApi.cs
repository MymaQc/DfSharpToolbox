using Dragonfly;

namespace Toolbox.Worlds;

public static class WorldApi
{

    public static World CreateWorld(World.Config config)
    {
        ArgumentNullException.ThrowIfNull(config);
        return config.New();
    }

    public static World CreateMemoryWorld(World.Dimension? dimension = null,
        bool readOnly = false,
        TimeSpan saveInterval = default,
        TimeSpan chunkUnloadInterval = default,
        int randomTickSpeed = 0)
    {
        return new World.Config
        {
            Dim = dimension,
            ReadOnly = readOnly,
            SaveInterval = saveInterval,
            ChunkUnloadInterval = chunkUnloadInterval,
            RandomTickSpeed = randomTickSpeed,
        }.New();
    }

    public static World OpenMcdbWorld(string directory,
        World.Dimension? dimension = null,
        bool readOnly = false,
        TimeSpan saveInterval = default,
        TimeSpan chunkUnloadInterval = default,
        int randomTickSpeed = 0)
    {
        return new World.Config
        {
            Dim = dimension,
            Provider = new MCDB.Config().Open(directory),
            ReadOnly = readOnly,
            SaveInterval = saveInterval,
            ChunkUnloadInterval = chunkUnloadInterval,
            RandomTickSpeed = randomTickSpeed,
        }.New();
    }

    public static World.Dimension GetOverworldDimension()
    {
        return World.Overworld;
    }

    public static World.Dimension GetNetherDimension()
    {
        return World.Nether;
    }

    public static World.Dimension GetEndDimension()
    {
        return World.End;
    }

    public static string GetName(World world)
    {
        return world.Name();
    }

    public static Cube.Range GetRange(World world)
    {
        return world.Range();
    }

    public static World.Dimension GetDimension(World world)
    {
        return world.Dimension();
    }

    public static int GetHighestLightBlocker(World world, int x, int z)
    {
        return world.HighestLightBlocker(x, z);
    }

    public static Cube.Pos GetSpawn(World world)
    {
        return world.Spawn();
    }

    public static void SetSpawn(World world, Cube.Pos spawn)
    {
        world.SetSpawn(spawn);
    }

    public static Cube.Pos GetPlayerSpawn(World world, Player player)
    {
        ArgumentNullException.ThrowIfNull(player);
        return GetPlayerSpawn(world, player.UUID());
    }

    public static Cube.Pos GetPlayerSpawn(World world, Guid playerId)
    {
        return world.PlayerSpawn(playerId);
    }

    public static void SetPlayerSpawn(World world, Player player, Cube.Pos spawn)
    {
        ArgumentNullException.ThrowIfNull(player);
        SetPlayerSpawn(world, player.UUID(), spawn);
    }

    public static void SetPlayerSpawn(World world, Guid playerId, Cube.Pos spawn)
    {
        world.SetPlayerSpawn(playerId, spawn);
    }

    public static int GetTime(World world)
    {
        return world.Time();
    }

    public static void SetTime(World world, int time)
    {
        world.SetTime(time);
    }

    public static bool IsTimeCycleEnabled(World world)
    {
        return world.TimeCycle();
    }

    public static void StartTimeCycle(World world)
    {
        world.StartTime();
    }

    public static void StopTimeCycle(World world)
    {
        world.StopTime();
    }

    public static void SetRequiredSleepDuration(World world, TimeSpan duration)
    {
        world.SetRequiredSleepDuration(duration);
    }

    public static World.GameMode GetDefaultGameMode(World world)
    {
        return world.DefaultGameMode();
    }

    public static void SetDefaultGameMode(World world, World.GameMode gameMode)
    {
        world.SetDefaultGameMode(gameMode);
    }

    public static void SetTickRange(World world, int tickRange)
    {
        world.SetTickRange(tickRange);
    }

    public static World.Difficulty GetDifficulty(World world)
    {
        return world.Difficulty();
    }

    public static void SetDifficulty(World world, World.Difficulty difficulty)
    {
        world.SetDifficulty(difficulty);
    }

    public static World.Difficulty GetPeacefulDifficulty()
    {
        return World.DifficultyPeaceful;
    }

    public static World.Difficulty GetEasyDifficulty()
    {
        return World.DifficultyEasy;
    }

    public static World.Difficulty GetNormalDifficulty()
    {
        return World.DifficultyNormal;
    }

    public static World.Difficulty GetHardDifficulty()
    {
        return World.DifficultyHard;
    }

    public static (World.Difficulty Difficulty, bool Ok) GetDifficultyById(int id)
    {
        return World.DifficultyByID(id);
    }

    public static (int Id, bool Ok) GetDifficultyId(World.Difficulty difficulty)
    {
        return World.DifficultyID(difficulty);
    }

    public static void Save(World world)
    {
        world.Save();
    }

    public static void Close(World world)
    {
        world.Close();
    }

    public static World.Task Run(World world, Action<World.Tx> callback)
    {
        return world.Do(callback);
    }

    public static World.Task RunLater(World world, TimeSpan delay, Action<World.Tx> callback)
    {
        return world.DoAfter(delay, callback);
    }

    public static World GetWorld(World.Tx tx)
    {
        return tx.World();
    }

    public static Cube.Range GetRange(World.Tx tx)
    {
        return tx.Range();
    }

    public static long GetCurrentTick(World.Tx tx)
    {
        return tx.CurrentTick();
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

    public static void PlaySound(World.Tx tx, Vector3 position, World.Sound sound)
    {
        tx.PlaySound(position, sound);
    }

    public static IEnumerable<Player> GetPlayers(World.Tx tx)
    {
        return tx.Players().OfType<Player>();
    }

    public static IEnumerable<World.Entity> GetEntities(World.Tx tx)
    {
        return tx.Entities();
    }

    public static IEnumerable<World.Entity> GetEntitiesAround(World.Tx tx, Vector3 center, double radius)
    {
        return tx.EntitiesWithin(Cube.Box(center.X - radius, center.Y - radius, center.Z - radius, center.X + radius, center.Y + radius, center.Z + radius));
    }

    public static IEnumerable<World.Entity> GetEntitiesWithin(World.Tx tx, Cube.BBox box)
    {
        return tx.EntitiesWithin(box);
    }

    public static void Broadcast(World.Tx tx, params object?[] message)
    {
        foreach (var player in GetPlayers(tx))
        {
            player.Message(message);
        }
    }
}
