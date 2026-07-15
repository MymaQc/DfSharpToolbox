using Dragonfly;
using Toolbox.Events.Player.Base;

namespace Toolbox.Events.Player.Blocks;

public sealed class PlayerFireExtinguishEvent(Dragonfly.Player player, Cube.Pos position) : CancellablePlayerEvent(player)
{
    public Cube.Pos Position { get; } = position;
}
