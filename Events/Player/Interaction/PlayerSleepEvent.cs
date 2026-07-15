using Toolbox.Events.Player.Base;

namespace Toolbox.Events.Player.Interaction;

public sealed class PlayerSleepEvent(Dragonfly.Player player, bool sendReminder) : CancellablePlayerEvent(player)
{
    public bool SendReminder { get; set; } = sendReminder;
}
