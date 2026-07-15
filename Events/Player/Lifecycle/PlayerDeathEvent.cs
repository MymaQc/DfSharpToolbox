using Toolbox.Events.Player.Base;
using DDamageSource = Dragonfly.World.DamageSource;

namespace Toolbox.Events.Player.Lifecycle;

public sealed class PlayerDeathEvent(Dragonfly.Player player, DDamageSource source, bool keepInventory) : PlayerEvent(player)
{
    public DDamageSource Source { get; } = source;

    public bool KeepInventory { get; set; } = keepInventory;
}
