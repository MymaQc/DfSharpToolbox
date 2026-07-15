using Dragonfly;
using Toolbox.Events.World.Base;

namespace Toolbox.Events.World.Explosions;

public sealed class WorldExplosionEvent(Dragonfly.World world, Vector3 position, Dragonfly.World.Entity[] entities, Cube.Pos[] blocks, double itemDropChance, bool spawnFire)
    : CancellableWorldEvent(world)
{
    public Vector3 Position { get; } = position;

    public Dragonfly.World.Entity[] Entities { get; set; } = entities;

    public Cube.Pos[] Blocks { get; set; } = blocks;

    public double ItemDropChance { get; set; } = itemDropChance;

    public bool SpawnFire { get; set; } = spawnFire;
}
