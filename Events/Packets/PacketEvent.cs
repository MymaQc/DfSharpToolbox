using DPacketContext = Dragonfly.Packet.Context;
using DPacket = Dragonfly.Packet.Packet;
using DPlayer = Dragonfly.Player;
using DWorld = Dragonfly.World;
using Toolbox.Packets;

namespace Toolbox.Events.Packets;

public abstract class PacketEvent(DPacketContext context, DPacket packet) : CancellableEvent
{

    private DPacketContext Context { get; } = context;

    private DPacket Packet { get; } = packet;

    private string Xuid => Context.XUID();

    private uint PacketId => Packet.ID();

    private string PacketName => PacketApi.GetPacketName(Packet);

    public DPacketContext GetContext()
    {
        return Context;
    }

    public DPacket GetPacket()
    {
        return Packet;
    }

    public string GetContextXuid()
    {
        return Xuid;
    }

    public uint GetPacketId()
    {
        return PacketId;
    }

    public string GetPacketName()
    {
        return PacketName;
    }

    public bool IsPacket<TPacket>() where TPacket : class, DPacket
    {
        return PacketApi.IsPacket<TPacket>(Packet);
    }

    public TPacket? AsPacket<TPacket>() where TPacket : class, DPacket
    {
        return PacketApi.AsPacket<TPacket>(Packet);
    }

    public TPacket RequirePacket<TPacket>() where TPacket : class, DPacket
    {
        return PacketApi.RequirePacket<TPacket>(Packet);
    }

    public bool TryGetPacket<TPacket>(out TPacket packet) where TPacket : class, DPacket
    {
        return PacketApi.TryGetPacket(Packet, out packet);
    }

    public void EditPacket<TPacket>(Action<TPacket> editor) where TPacket : class, DPacket
    {
        PacketApi.EditPacket(Packet, editor);
    }

    public bool TryEditPacket<TPacket>(Action<TPacket> editor, out Exception? exception) where TPacket : class, DPacket
    {
        return PacketApi.TryEditPacket(Packet, editor, out exception);
    }

    private IReadOnlyList<PacketFieldSnapshot> InspectPacketFields()
    {
        return PacketApi.InspectPacketFields(Packet);
    }

    public IReadOnlyList<PacketFieldSnapshot> GetPacketFields()
    {
        return InspectPacketFields();
    }

    public object? GetPacketField(string name)
    {
        return PacketApi.GetPacketField(Packet, name);
    }

    public bool TryGetPacketField(string name, out object? value, out Exception? exception)
    {
        return PacketApi.TryGetPacketField(Packet, name, out value, out exception);
    }

    public void SetPacketField(string name, object? value)
    {
        PacketApi.SetPacketField(Packet, name, value);
    }

    public bool TrySetPacketField(string name, object? value, out Exception? exception)
    {
        return PacketApi.TrySetPacketField(Packet, name, value, out exception);
    }

    public void SendPacketTo(DPlayer player)
    {
        PacketApi.SendPacket(player, Packet);
    }

    public void SendPacketTo(IEnumerable<DPlayer> players)
    {
        PacketApi.SendPacketToPlayers(players, Packet);
    }

    public void SendPacketTo(DWorld.Tx tx)
    {
        PacketApi.SendPacketToWorldPlayers(tx, Packet);
    }

    public void CancelAndSendPacketTo(DPlayer player)
    {
        Cancel();
        SendPacketTo(player);
    }

    public void CancelAndSendPacketTo(IEnumerable<DPlayer> players)
    {
        Cancel();
        SendPacketTo(players);
    }

    public void CancelAndSendPacketTo(DWorld.Tx tx)
    {
        Cancel();
        SendPacketTo(tx);
    }
}
