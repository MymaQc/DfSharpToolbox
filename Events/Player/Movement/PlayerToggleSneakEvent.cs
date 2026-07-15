using Toolbox.Events.Player.Base;

namespace Toolbox.Events.Player.Movement;

public sealed class PlayerToggleSneakEvent(Dragonfly.Player player, bool sneaking) : CancellablePlayerEvent(player)
{
    public bool Sneaking { get; } = sneaking;
}
