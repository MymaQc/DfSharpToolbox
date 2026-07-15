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

    private string PacketName => PacketApi.GetName(Packet);

    public DPacketContext GetContext()
    {
        return Context;
    }

    public DPacket GetPacket()
    {
        return Packet;
    }

    public string GetXuid()
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

    public bool Is<TPacket>() where TPacket : class, DPacket
    {
        return PacketApi.Is<TPacket>(Packet);
    }

    public TPacket? As<TPacket>() where TPacket : class, DPacket
    {
        return PacketApi.As<TPacket>(Packet);
    }

    public TPacket Get<TPacket>() where TPacket : class, DPacket
    {
        return PacketApi.Get<TPacket>(Packet);
    }

    private bool TryGet<TPacket>(out TPacket packet) where TPacket : class, DPacket
    {
        return PacketApi.TryGet(Packet, out packet);
    }

    public bool TryAs<TPacket>(out TPacket packet) where TPacket : class, DPacket
    {
        return TryGet(out packet);
    }

    public void Edit<TPacket>(Action<TPacket> editor) where TPacket : class, DPacket
    {
        PacketApi.Edit(Packet, editor);
    }

    public bool TryEdit<TPacket>(Action<TPacket> editor, out Exception? exception) where TPacket : class, DPacket
    {
        return PacketApi.TryEdit(Packet, editor, out exception);
    }

    private IReadOnlyList<PacketFieldSnapshot> Inspect()
    {
        return PacketApi.Inspect(Packet);
    }

    public IReadOnlyList<PacketFieldSnapshot> GetFields()
    {
        return Inspect();
    }

    public object? GetField(string name)
    {
        return PacketApi.GetField(Packet, name);
    }

    public bool TryGetField(string name, out object? value, out Exception? exception)
    {
        return PacketApi.TryGetField(Packet, name, out value, out exception);
    }

    public void SetField(string name, object? value)
    {
        PacketApi.SetField(Packet, name, value);
    }

    public bool TrySetField(string name, object? value, out Exception? exception)
    {
        return PacketApi.TrySetField(Packet, name, value, out exception);
    }

    private void SendTo(DPlayer player)
    {
        PacketApi.Send(player, Packet);
    }

    private void SendTo(IEnumerable<DPlayer> players)
    {
        PacketApi.SendTo(players, Packet);
    }

    private void SendTo(DWorld.Tx tx)
    {
        PacketApi.SendTo(tx, Packet);
    }

    public void CancelAndSendTo(DPlayer player)
    {
        Cancel();
        SendTo(player);
    }

    public void CancelAndSendTo(IEnumerable<DPlayer> players)
    {
        Cancel();
        SendTo(players);
    }

    public void CancelAndSendTo(DWorld.Tx tx)
    {
        Cancel();
        SendTo(tx);
    }
}
