using Dragonfly;
using Toolbox.Events.Player.Base;

namespace Toolbox.Events.Player.Items;

public sealed class PlayerItemDamageEvent(Dragonfly.Player player, Item.Stack item, int damage) : CancellablePlayerEvent(player)
{
    public Item.Stack Item { get; } = item;

    public int Damage { get; set; } = damage;
}
