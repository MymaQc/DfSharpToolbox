namespace Toolbox.Players.States;

public readonly record struct PlayerFireState(
    bool FireProof,
    TimeSpan OnFireDuration);
