using DPacketContext = Dragonfly.Packet.Context;
using DPacket = Dragonfly.Packet.Packet;

namespace Toolbox.Events.Packets;

public sealed class ServerPacketEvent(DPacketContext context, DPacket packet) : PacketEvent(context, packet);
