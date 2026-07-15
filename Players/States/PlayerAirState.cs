namespace Toolbox.Players.States;

public readonly record struct PlayerAirState(
    TimeSpan AirSupply,
    TimeSpan MaxAirSupply);
