using Dragonfly;
using Toolbox.Events.Player.Base;

namespace Toolbox.Events.Player.Items;

public sealed class PlayerItemPickupEvent(Dragonfly.Player player, Item.Stack item) : CancellablePlayerEvent(player)
{
    public Item.Stack Item { get; set; } = item;
}
