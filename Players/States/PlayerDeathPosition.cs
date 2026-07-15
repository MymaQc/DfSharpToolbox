using Dragonfly;

namespace Toolbox.Players.States;

public readonly record struct PlayerDeathPosition(
    Vector3 Position,
    World.Dimension? Dimension,
    bool Found);
