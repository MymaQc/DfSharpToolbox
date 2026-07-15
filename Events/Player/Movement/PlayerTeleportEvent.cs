using Dragonfly;
using Toolbox.Events.Player.Base;

namespace Toolbox.Events.Player.Movement;

public sealed class PlayerTeleportEvent(Dragonfly.Player player, Vector3 position) : CancellablePlayerEvent(player)
{
    public Vector3 Position { get; } = position;
}
