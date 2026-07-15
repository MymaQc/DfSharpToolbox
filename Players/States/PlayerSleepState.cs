using Dragonfly;

namespace Toolbox.Players.States;

public readonly record struct PlayerSleepState(
    Cube.Pos Position,
    bool Sleeping);
