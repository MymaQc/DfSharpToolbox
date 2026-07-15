using Dragonfly;
using Toolbox.Events.Player.Base;

namespace Toolbox.Events.Player.Interaction;

public sealed class PlayerSignEditEvent(Dragonfly.Player player, Cube.Pos position, bool frontSide, string oldText, string newText) : CancellablePlayerEvent(player)
{
    public Cube.Pos Position { get; } = position;

    public bool FrontSide { get; } = frontSide;

    public string OldText { get; } = oldText;

    public string NewText { get; } = newText;
}
