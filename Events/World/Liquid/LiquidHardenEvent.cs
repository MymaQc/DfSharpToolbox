using Dragonfly;
using Toolbox.Events.World.Base;

namespace Toolbox.Events.World.Liquid;

public sealed class LiquidHardenEvent(
    Dragonfly.World world,
    Cube.Pos position,
    Dragonfly.World.Block hardenedLiquid,
    Dragonfly.World.Block otherLiquid,
    Dragonfly.World.Block newBlock) : CancellableWorldEvent(world)
{
    public Cube.Pos Position { get; } = position;

    public Dragonfly.World.Block HardenedLiquid { get; } = hardenedLiquid;

    public Dragonfly.World.Block OtherLiquid { get; } = otherLiquid;

    public Dragonfly.World.Block NewBlock { get; } = newBlock;
}
