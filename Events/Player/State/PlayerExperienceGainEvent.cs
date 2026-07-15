using Toolbox.Events.Player.Base;

namespace Toolbox.Events.Player.State;

public sealed class PlayerExperienceGainEvent(Dragonfly.Player player, int amount) : CancellablePlayerEvent(player)
{
    public int Amount { get; set; } = amount;
}
