using Toolbox.Events.Player.Base;

namespace Toolbox.Events.Player.Lifecycle;

public sealed class PlayerJoinEvent(Dragonfly.Player player) : CancellablePlayerEvent(player);
