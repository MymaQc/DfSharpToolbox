using Dragonfly;

namespace Toolbox.Presentation;

public static class ToastApi
{
    public static void SendToast(Player player, string title, string message)
    {
        player.SendToast(title, message);
    }
}
