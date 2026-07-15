using Toolbox.Events.Player.Base;

namespace Toolbox.Events.Player.State;

public sealed class PlayerFoodLossEvent(Dragonfly.Player player, int from, int to) : CancellablePlayerEvent(player)
{
    public int From { get; } = from;

    public int To { get; set; } = to;
}
