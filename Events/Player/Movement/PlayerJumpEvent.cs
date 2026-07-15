using Toolbox.Events.Player.Base;

namespace Toolbox.Events.Player.Movement;

public sealed class PlayerJumpEvent(Dragonfly.Player player) : PlayerEvent(player);
