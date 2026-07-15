using Dragonfly;
using Toolbox.Events.Player.Base;

namespace Toolbox.Events.Player.Network;

public sealed class PlayerTransferEvent(Dragonfly.Player player, Net.UDPAddr address) : CancellablePlayerEvent(player)
{
    public Net.UDPAddr Address { get; set; } = address;
}
