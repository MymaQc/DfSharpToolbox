namespace Toolbox.Players.States;

public readonly record struct PlayerExperienceState(
    int Level,
    double Progress,
    int Experience,
    long EnchantmentSeed,
    bool CanCollectExperience);
