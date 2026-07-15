namespace Toolbox.Events.World.Base;

public abstract class WorldEvent(Dragonfly.World world) : Event
{
    public Dragonfly.World World { get; } = world;
}
