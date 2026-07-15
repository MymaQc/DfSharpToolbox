using Toolbox.Events.Player.Base;
using DWorld = Dragonfly.World;

namespace Toolbox.Events.Player.Lifecycle;

public sealed class PlayerChangeWorldEvent(Dragonfly.Player player, DWorld? before, DWorld after) : PlayerEvent(player)
{
    public DWorld? Before { get; } = before;

    public DWorld After { get; } = after;
}
