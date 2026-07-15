using Dragonfly;
using Toolbox.Events.World.Base;

namespace Toolbox.Events.World.Liquid;

public sealed class LiquidFlowEvent(Dragonfly.World world, Cube.Pos from, Cube.Pos into, Dragonfly.World.Liquid liquid, Dragonfly.World.Block replaced) : CancellableWorldEvent(world)
{
    public Cube.Pos From { get; } = from;

    public Cube.Pos Into { get; } = into;

    public Dragonfly.World.Liquid Liquid { get; } = liquid;

    public Dragonfly.World.Block Replaced { get; } = replaced;
}
