using Dragonfly;
using Toolbox.Events.World.Base;

namespace Toolbox.Events.World.Blocks;

public sealed class FireSpreadEvent(Dragonfly.World world, Cube.Pos from, Cube.Pos to) : CancellableWorldEvent(world)
{
    public Cube.Pos From { get; } = from;

    public Cube.Pos To { get; } = to;
}
