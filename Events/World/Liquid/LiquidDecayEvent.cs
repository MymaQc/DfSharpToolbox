using Dragonfly;
using Toolbox.Events.World.Base;

namespace Toolbox.Events.World.Liquid;

public sealed class LiquidDecayEvent(Dragonfly.World world, Cube.Pos position, Dragonfly.World.Liquid before, Dragonfly.World.Liquid? after) : CancellableWorldEvent(world)
{
    public Cube.Pos Position { get; } = position;

    public Dragonfly.World.Liquid Before { get; } = before;

    public Dragonfly.World.Liquid? After { get; } = after;
}
