using Toolbox.Events.World.Base;

namespace Toolbox.Events.World.Redstone;

public sealed class RedstoneUpdateEvent(Dragonfly.World world, Dragonfly.World.RedstoneUpdate update) : CancellableWorldEvent(world)
{
    public Dragonfly.World.RedstoneUpdate Update { get; } = update;
}
