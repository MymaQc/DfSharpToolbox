using Dragonfly;
using Toolbox.Events.Player.Base;
using DBlock = Dragonfly.World.Block;

namespace Toolbox.Events.Player.Blocks;

public sealed class PlayerBlockPlaceEvent(Dragonfly.Player player, Cube.Pos position, DBlock block) : CancellablePlayerEvent(player)
{
    public Cube.Pos Position { get; } = position;

    public DBlock Block { get; } = block;
}
