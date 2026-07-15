using Dragonfly;
using Toolbox.Events.World.Base;

namespace Toolbox.Events.World.Blocks;

public sealed class BlockBurnEvent(Dragonfly.World world, Cube.Pos position) : CancellableWorldEvent(world)
{
    public Cube.Pos Position { get; } = position;
}
