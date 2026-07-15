using Dragonfly;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using DPacketContext = Dragonfly.Packet.Context;
using DPacket = Dragonfly.Packet.Packet;
using DPacketUnknown = Dragonfly.Packet.Unknown;
using DPacketValue = Dragonfly.Packet.Value;

namespace Toolbox.Packets;

public static class PacketApi
{
    private const BindingFlags PublicInstance = BindingFlags.Instance | BindingFlags.Public;

    private static uint GetDragonflyPacketId(DPacket packet)
    {
        ArgumentNullException.ThrowIfNull(packet);
        return packet.ID();
    }

    public static string GetPacketName(DPacket packet)
    {
        ArgumentNullException.ThrowIfNull(packet);
        return packet.GetType().Name;
    }

    public static Type GetPacketType(DPacket packet)
    {
        ArgumentNullException.ThrowIfNull(packet);
        return packet.GetType();
    }

    public static bool IsKnownPacket(DPacket packet)
    {
        ArgumentNullException.ThrowIfNull(packet);
        return packet is not DPacketUnknown;
    }

    public static bool HasPacketId(DPacket packet, uint id)
    {
        return GetDragonflyPacketId(packet) == id;
    }

    public static string GetContextXuid(DPacketContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return context.XUID();
    }

    public static bool IsCancelled(DPacketContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return context.Cancelled();
    }

    public static void CancelContext(DPacketContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        context.Cancel();
    }

    public static void SendPacket(Player player, DPacket packet)
    {
        ArgumentNullException.ThrowIfNull(player);
        ArgumentNullException.ThrowIfNull(packet);
        player.WritePacket(packet);
    }

    public static void SendPacketToPlayer(Player player, DPacket packet)
    {
        SendPacket(player, packet);
    }

    public static void SendPacketToPlayers(IEnumerable<Player> players, DPacket packet)
    {
        ArgumentNullException.ThrowIfNull(players);
        ArgumentNullException.ThrowIfNull(packet);

        foreach (var player in players)
        {
            SendPacket(player, packet);
        }
    }

    public static void SendPacketToWorldPlayers(World.Tx tx, DPacket packet)
    {
        ArgumentNullException.ThrowIfNull(tx);
        SendPacketToPlayers(tx.Players().OfType<Player>(), packet);
    }

    public static void BroadcastPacket(IEnumerable<Player> players, DPacket packet)
    {
        SendPacketToPlayers(players, packet);
    }

    public static void BroadcastPacket(World.Tx tx, DPacket packet)
    {
        SendPacketToWorldPlayers(tx, packet);
    }

    public static bool TrySendPacket(Player player, DPacket packet, out Exception? exception)
    {
        try
        {
            SendPacket(player, packet);
            exception = null;
            return true;
        }
        catch (Exception error)
        {
            exception = error;
            return false;
        }
    }

    public static bool IsPacket<TPacket>(DPacket packet) where TPacket : class, DPacket
    {
        return packet is TPacket;
    }

    public static TPacket? AsPacket<TPacket>(DPacket packet) where TPacket : class, DPacket
    {
        return packet as TPacket;
    }

    public static TPacket RequirePacket<TPacket>(DPacket packet) where TPacket : class, DPacket
    {
        if (packet is TPacket typed)
        {
            return typed;
        }

        throw new InvalidOperationException($"Packet {GetPacketName(packet)} is not {typeof(TPacket).Name}.");
    }

    public static bool TryGetPacket<TPacket>(DPacket packet, out TPacket typed) where TPacket : class, DPacket
    {
        if (packet is TPacket value)
        {
            typed = value;
            return true;
        }

        typed = null!;
        return false;
    }

    public static void EditPacket<TPacket>(DPacket packet, Action<TPacket> editor) where TPacket : class, DPacket
    {
        ArgumentNullException.ThrowIfNull(editor);
        editor(RequirePacket<TPacket>(packet));
    }

    public static bool TryEditPacket<TPacket>(DPacket packet, Action<TPacket> editor, out Exception? exception) where TPacket : class, DPacket
    {
        try
        {
            EditPacket(packet, editor);
            exception = null;
            return true;
        }
        catch (Exception error)
        {
            exception = error;
            return false;
        }
    }

    [UnconditionalSuppressMessage("Trimming", "IL2075", Justification = "Packet inspection is a best-effort Toolbox helper for generated Dragonfly packet proxies.")]
    public static IReadOnlyList<PacketFieldSnapshot> InspectPacketFields(DPacket packet)
    {
        ArgumentNullException.ThrowIfNull(packet);
        var fields = new List<PacketFieldSnapshot>();
        foreach (var property in packet.GetType().GetProperties(PublicInstance))
        {
            if (property.GetIndexParameters().Length != 0)
            {
                continue;
            }

            fields.Add(ReadField(packet, property));
        }

        return fields;
    }

    public static IReadOnlyList<PacketFieldSnapshot> GetPacketFields(DPacket packet)
    {
        return InspectPacketFields(packet);
    }

    public static object? GetPacketField(DPacket packet, string name)
    {
        if (!TryGetPacketField(packet, name, out var value, out var exception))
        {
            throw exception ?? new InvalidOperationException($"Unable to read {name}.");
        }

        return value;
    }

    public static bool TryGetPacketField(DPacket packet, string name, out object? value, out Exception? exception)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(packet);
            value = GetProperty(packet, name).GetValue(packet);
            exception = null;
            return true;
        }
        catch (Exception error)
        {
            value = null;
            exception = error;
            return false;
        }
    }

    public static void SetPacketField(DPacket packet, string name, object? value)
    {
        if (!TrySetPacketField(packet, name, value, out var exception))
        {
            throw exception ?? new InvalidOperationException($"Unable to set {name}.");
        }
    }

    public static bool TrySetPacketField(DPacket packet, string name, object? value, out Exception? exception)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(packet);
            var property = GetProperty(packet, name);
            if (!property.CanWrite)
            {
                throw new InvalidOperationException($"Field {property.Name} is read-only.");
            }

            property.SetValue(packet, ConvertTo(value, property.PropertyType));
            exception = null;
            return true;
        }
        catch (Exception error)
        {
            exception = error;
            return false;
        }
    }

    private static string GetJson(DPacketValue value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return value.Json();
    }

    private static bool TryGetJson(DPacketValue value, out string json)
    {
        try
        {
            json = GetJson(value);
            return true;
        }
        catch (Exception)
        {
            json = string.Empty;
            return false;
        }
    }

    [UnconditionalSuppressMessage("Trimming", "IL2075", Justification = "Packet field access is a best-effort Toolbox helper for generated Dragonfly packet proxies.")]
    private static PropertyInfo GetProperty(DPacket packet, string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        var property = packet.GetType().GetProperty(name, PublicInstance | BindingFlags.IgnoreCase);
        if (property is null || property.GetIndexParameters().Length != 0)
        {
            throw new MissingMemberException(packet.GetType().Name, name);
        }

        return property;
    }

    private static PacketFieldSnapshot ReadField(DPacket packet, PropertyInfo property)
    {
        try
        {
            var value = property.GetValue(packet);
            var json = value is DPacketValue packetValue && TryGetJson(packetValue, out var valueJson) ? valueJson : null;
            return new PacketFieldSnapshot(
                property.Name,
                GetFriendlyTypeName(property.PropertyType),
                value,
                FormatValue(value),
                property.CanWrite,
                json,
                null);
        }
        catch (Exception exception)
        {
            return new PacketFieldSnapshot(
                property.Name,
                GetFriendlyTypeName(property.PropertyType),
                null,
                string.Empty,
                property.CanWrite,
                null,
                exception.Message);
        }
    }

    private static object? ConvertTo(object? value, Type targetType)
    {
        var nullableType = Nullable.GetUnderlyingType(targetType);
        var type = nullableType ?? targetType;
        if (value is null)
        {
            if (nullableType is not null || !type.IsValueType)
            {
                return null;
            }

            throw new InvalidCastException($"Cannot assign null to {type.Name}.");
        }

        if (type.IsInstanceOfType(value))
        {
            return value;
        }

        if (type == typeof(Guid))
        {
            return value is Guid guid ? guid : Guid.Parse(value.ToString() ?? string.Empty);
        }

        if (type == typeof(byte[]))
        {
            return value is byte[] bytes ? bytes : Convert.FromBase64String(value.ToString() ?? string.Empty);
        }

        if (type.IsEnum)
        {
            return value is string text ? Enum.Parse(type, text, true) : Enum.ToObject(type, value);
        }

        if (type == typeof(string))
        {
            return value.ToString() ?? string.Empty;
        }

        return Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
    }

    private static string FormatValue(object? value)
    {
        return value switch
        {
            null => "null",
            string text => text,
            byte[] bytes => $"byte[{bytes.Length}]",
            DPacketValue packetValue => TryGetJson(packetValue, out var json) ? json : "<complex>",
            Array array => $"{value.GetType().GetElementType()?.Name ?? "object"}[{array.Length}]",
            _ => value.ToString() ?? string.Empty,
        };
    }

    private static string GetFriendlyTypeName(Type type)
    {
        if (!type.IsGenericType)
        {
            return type.Name;
        }

        var name = type.Name[..type.Name.IndexOf('`')];
        return $"{name}<{string.Join(", ", type.GetGenericArguments().Select(GetFriendlyTypeName))}>";
    }
}
