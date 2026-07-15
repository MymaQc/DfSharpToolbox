using Toolbox.Events.Player.Base;
using DEntity = Dragonfly.World.Entity;

namespace Toolbox.Events.Player.Combat;

public sealed class PlayerAttackEntityEvent(Dragonfly.Player player, DEntity entity, double knockbackForce, double knockbackHeight, bool critical)
    : CancellablePlayerEvent(player)
{
    public DEntity Entity { get; } = entity;

    public double KnockbackForce { get; set; } = knockbackForce;

    public double KnockbackHeight { get; set; } = knockbackHeight;

    public bool Critical { get; set; } = critical;
}
