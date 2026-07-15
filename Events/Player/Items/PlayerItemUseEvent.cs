using Toolbox.Events.Player.Base;

namespace Toolbox.Events.Player.Items;

public sealed class PlayerItemUseEvent(Dragonfly.Player player) : CancellablePlayerEvent(player);
