namespace Toolbox.Events.Player.Base;

public abstract class CancellablePlayerEvent(Dragonfly.Player player) : CancellableEvent
{
    public Dragonfly.Player Player { get; } = player;
}
