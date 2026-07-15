using Toolbox.Events.Player.Base;

namespace Toolbox.Events.Player.Lifecycle;

public sealed class PlayerQuitEvent(Dragonfly.Player player) : PlayerEvent(player);
