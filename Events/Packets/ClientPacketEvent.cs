using DPacketContext = Dragonfly.Packet.Context;
using DPacket = Dragonfly.Packet.Packet;

namespace Toolbox.Events.Packets;

public sealed class ClientPacketEvent(DPacketContext context, DPacket packet) : PacketEvent(context, packet);
