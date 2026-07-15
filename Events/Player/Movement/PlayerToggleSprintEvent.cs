using Toolbox.Events.Player.Base;

namespace Toolbox.Events.Player.Movement;

public sealed class PlayerToggleSprintEvent(Dragonfly.Player player, bool sprinting) : CancellablePlayerEvent(player)
{
    public bool Sprinting { get; } = sprinting;
}
