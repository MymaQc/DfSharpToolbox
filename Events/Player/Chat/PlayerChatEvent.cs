using Toolbox.Events.Player.Base;

namespace Toolbox.Events.Player.Chat;

public sealed class PlayerChatEvent(Dragonfly.Player player, string message) : CancellablePlayerEvent(player)
{
    public string Message { get; set; } = message;
}
