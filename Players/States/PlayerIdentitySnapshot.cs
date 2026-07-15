using Dragonfly;

namespace Toolbox.Players.States;

public readonly record struct PlayerIdentitySnapshot(
    string Name,
    Guid UniqueId,
    string Xuid,
    string DeviceId,
    string DeviceModel,
    string SelfSignedId,
    Language.Tag Locale,
    Net.Addr? Address);
