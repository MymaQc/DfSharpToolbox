using Dragonfly;
using Toolbox.Events.Player.Base;

namespace Toolbox.Events.Player.Blocks;

public sealed class PlayerItemUseOnBlockEvent(Dragonfly.Player player, Cube.Pos position, Cube.Face face, Vector3 clickPosition) : CancellablePlayerEvent(player)
{
    public Cube.Pos Position { get; } = position;

    public Cube.Face Face { get; } = face;

    public Vector3 ClickPosition { get; } = clickPosition;
}
