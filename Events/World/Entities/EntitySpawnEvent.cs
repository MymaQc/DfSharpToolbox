using Toolbox.Events.World.Base;

namespace Toolbox.Events.World.Entities;

public sealed class EntitySpawnEvent(Dragonfly.World world, Dragonfly.World.Entity entity) : WorldEvent(world)
{
    public Dragonfly.World.Entity Entity { get; } = entity;
}
