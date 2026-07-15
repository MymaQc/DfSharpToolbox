using Dragonfly;
using Toolbox.Events.Player.Base;
using DWorld = Dragonfly.World;

namespace Toolbox.Events.Player.Lifecycle;

public sealed class PlayerRespawnEvent(Dragonfly.Player player, Vector3 position, DWorld world) : PlayerEvent(player)
{
    public Vector3 Position { get; set; } = position;

    public DWorld World { get; set; } = world;
}
