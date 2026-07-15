using Toolbox.Events.Player.Base;
using DHealingSource = Dragonfly.World.HealingSource;

namespace Toolbox.Events.Player.State;

public sealed class PlayerHealEvent(Dragonfly.Player player, double amount, DHealingSource source) : CancellablePlayerEvent(player)
{
    public double Amount { get; set; } = amount;

    public DHealingSource Source { get; } = source;
}
