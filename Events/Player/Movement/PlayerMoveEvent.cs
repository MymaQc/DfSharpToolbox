using Dragonfly;
using Toolbox.Events.Player.Base;

namespace Toolbox.Events.Player.Movement;

public sealed class PlayerMoveEvent(Dragonfly.Player player, Vector3 newPosition, Rotation newRotation) : CancellablePlayerEvent(player)
{
    public Vector3 NewPosition { get; } = newPosition;

    public Rotation NewRotation { get; } = newRotation;
}
