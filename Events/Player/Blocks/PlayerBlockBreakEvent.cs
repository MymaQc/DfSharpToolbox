using Dragonfly;
using Toolbox.Events.Player.Base;

namespace Toolbox.Events.Player.Blocks;

public sealed class PlayerBlockBreakEvent(Dragonfly.Player player, Cube.Pos position, Item.Stack[] drops, int experience) : CancellablePlayerEvent(player)
{
    public Cube.Pos Position { get; } = position;

    public Item.Stack[] Drops { get; set; } = drops;

    public int Experience { get; set; } = experience;
}
