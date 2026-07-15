using Dragonfly;
using Toolbox.Events.Player.Base;

namespace Toolbox.Events.Player.Lifecycle;

public sealed class PlayerSkinChangeEvent(Dragonfly.Player player, Skin skin) : CancellablePlayerEvent(player)
{
    public Skin Skin { get; set; } = skin;
}
