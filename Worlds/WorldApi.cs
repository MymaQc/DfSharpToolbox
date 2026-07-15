using Dragonfly;

namespace Toolbox.Worlds;

public static class WorldApi
{
    public static readonly World.Difficulty PEACEFUL = World.DifficultyPeaceful;
    public static readonly World.Difficulty EASY = World.DifficultyEasy;
    public static readonly World.Difficulty NORMAL = World.DifficultyNormal;
    public static readonly World.Difficulty HARD = World.DifficultyHard;

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

    private static Cube.Pos GetPlayerSpawn(World world, Guid playerId)
    {
        return world.PlayerSpawn(playerId);
    }

    public static void SetPlayerSpawn(World world, Player player, Cube.Pos spawn)
    {
        ArgumentNullException.ThrowIfNull(player);
        SetPlayerSpawn(world, player.UUID(), spawn);
    }

    private static void SetPlayerSpawn(World world, Guid playerId, Cube.Pos spawn)
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

    public static (World.Difficulty Difficulty, bool Ok) GetDifficultyById(int id)
    {
        return World.DifficultyByID(id);
    }

    public static (int Id, bool Ok) GetDifficultyId(World.Difficulty difficulty)
    {
        return World.DifficultyID(difficulty);
    }

    public static void SaveWorld(World world)
    {
        world.Save();
    }

    public static void CloseWorld(World world)
    {
        world.Close();
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

    public static IEnumerable<Player> GetPlayers(World.Tx tx)
    {
        return tx.Players().OfType<Player>();
    }

    public static void BroadcastMessage(World.Tx tx, params object?[] message)
    {
        foreach (var player in GetPlayers(tx))
        {
            player.Message(message);
        }
    }
}
