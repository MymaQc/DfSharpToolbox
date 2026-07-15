namespace Toolbox.Players.States;

public readonly record struct PlayerStatusState(
    bool UsingItem,
    PlayerSleepState Sleep,
    PlayerDeathPosition DeathPosition);
