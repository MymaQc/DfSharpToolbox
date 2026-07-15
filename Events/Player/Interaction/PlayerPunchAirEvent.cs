using Toolbox.Events.Player.Base;

namespace Toolbox.Events.Player.Interaction;

public sealed class PlayerPunchAirEvent(Dragonfly.Player player) : CancellablePlayerEvent(player);
