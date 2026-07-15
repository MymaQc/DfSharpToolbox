namespace Toolbox.Events.Player.Base;

public abstract class PlayerEvent(Dragonfly.Player player) : Event
{
    public Dragonfly.Player Player { get; } = player;
}
