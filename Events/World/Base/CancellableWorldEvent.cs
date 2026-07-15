namespace Toolbox.Events.World.Base;

public abstract class CancellableWorldEvent(Dragonfly.World world) : CancellableEvent
{
    public Dragonfly.World World { get; } = world;
}
