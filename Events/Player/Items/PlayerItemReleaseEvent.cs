using Dragonfly;
using Toolbox.Events.Player.Base;

namespace Toolbox.Events.Player.Items;

public sealed class PlayerItemReleaseEvent(Dragonfly.Player player, Item.Stack item, TimeSpan duration) : CancellablePlayerEvent(player)
{
    public Item.Stack Item { get; } = item;

    public TimeSpan Duration { get; } = duration;
}
