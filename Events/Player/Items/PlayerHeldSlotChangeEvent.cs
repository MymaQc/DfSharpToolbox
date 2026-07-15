using Toolbox.Events.Player.Base;

namespace Toolbox.Events.Player.Items;

public sealed class PlayerHeldSlotChangeEvent(Dragonfly.Player player, int from, int to) : CancellablePlayerEvent(player)
{
    public int From { get; } = from;

    public int To { get; } = to;
}
