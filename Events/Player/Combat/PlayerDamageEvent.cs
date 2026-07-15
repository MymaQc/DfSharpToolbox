using Toolbox.Events.Player.Base;
using DDamageSource = Dragonfly.World.DamageSource;

namespace Toolbox.Events.Player.Combat;

public sealed class PlayerDamageEvent(Dragonfly.Player player, double damage, bool immune, TimeSpan attackImmunity, DDamageSource source) : CancellablePlayerEvent(player)
{
    public double Damage { get; set; } = damage;

    public bool Immune { get; } = immune;

    public TimeSpan AttackImmunity { get; set; } = attackImmunity;

    public DDamageSource Source { get; } = source;
}
