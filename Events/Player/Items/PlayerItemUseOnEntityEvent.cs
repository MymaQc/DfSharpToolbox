using Toolbox.Events.Player.Base;
using DEntity = Dragonfly.World.Entity;

namespace Toolbox.Events.Player.Items;

public sealed class PlayerItemUseOnEntityEvent(Dragonfly.Player player, DEntity entity) : CancellablePlayerEvent(player)
{
    public DEntity Entity { get; } = entity;
}
