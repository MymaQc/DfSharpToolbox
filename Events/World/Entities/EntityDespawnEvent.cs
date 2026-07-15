using Toolbox.Events.World.Base;

namespace Toolbox.Events.World.Entities;

public sealed class EntityDespawnEvent(Dragonfly.World world, Dragonfly.World.Entity entity) : WorldEvent(world)
{
    public Dragonfly.World.Entity Entity { get; } = entity;
}
