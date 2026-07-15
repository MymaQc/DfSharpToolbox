using Dragonfly;
using Toolbox.Events.Player.Base;

namespace Toolbox.Events.Player.Interaction;

public sealed class PlayerLecternPageTurnEvent(Dragonfly.Player player, Cube.Pos position, int oldPage, int newPage) : CancellablePlayerEvent(player)
{
    public Cube.Pos Position { get; } = position;

    public int OldPage { get; } = oldPage;

    public int NewPage { get; set; } = newPage;
}
