namespace Toolbox.Players;

public readonly record struct PlayerPhysicalState(
    double FallDistance,
    double Absorption,
    bool Dead,
    bool OnGround,
    double EyeHeight,
    double TorsoHeight,
    bool Breathing);
