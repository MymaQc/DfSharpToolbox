namespace Toolbox.Players.States;

public readonly record struct PlayerMovementState(
    bool Sprinting,
    bool Sneaking,
    bool Swimming,
    bool Crawling,
    bool Gliding,
    bool Flying);
