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

    private static uint GetId(DPacket packet)
    {
        ArgumentNullException.ThrowIfNull(packet);
        return packet.ID();
    }

    public static string GetName(DPacket packet)
    {
        ArgumentNullException.ThrowIfNull(packet);
        return packet.GetType().Name;
    }

    public static Type GetType(DPacket packet)
    {
        ArgumentNullException.ThrowIfNull(packet);
        return packet.GetType();
    }

    public static bool IsKnown(DPacket packet)
    {
        ArgumentNullException.ThrowIfNull(packet);
        return packet is not DPacketUnknown;
    }

    public static bool HasId(DPacket packet, uint id)
    {
        return GetId(packet) == id;
    }

    public static string GetXuid(DPacketContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return context.XUID();
    }

    public static bool IsCancelled(DPacketContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return context.Cancelled();
    }

    public static void Cancel(DPacketContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        context.Cancel();
    }

    public static void Send(Player player, DPacket packet)
    {
        ArgumentNullException.ThrowIfNull(player);
        ArgumentNullException.ThrowIfNull(packet);
        player.WritePacket(packet);
    }

    public static void SendTo(Player player, DPacket packet)
    {
        Send(player, packet);
    }

    public static void SendTo(IEnumerable<Player> players, DPacket packet)
    {
        ArgumentNullException.ThrowIfNull(players);
        ArgumentNullException.ThrowIfNull(packet);

        foreach (var player in players)
        {
            Send(player, packet);
        }
    }

    public static void SendTo(World.Tx tx, DPacket packet)
    {
        ArgumentNullException.ThrowIfNull(tx);
        SendTo(tx.Players().OfType<Player>(), packet);
    }

    public static void Broadcast(IEnumerable<Player> players, DPacket packet)
    {
        SendTo(players, packet);
    }

    public static void Broadcast(World.Tx tx, DPacket packet)
    {
        SendTo(tx, packet);
    }

    public static bool TrySend(Player player, DPacket packet, out Exception? exception)
    {
        try
        {
            Send(player, packet);
            exception = null;
            return true;
        }
        catch (Exception error)
        {
            exception = error;
            return false;
        }
    }

    public static bool Is<TPacket>(DPacket packet) where TPacket : class, DPacket
    {
        return packet is TPacket;
    }

    public static TPacket? As<TPacket>(DPacket packet) where TPacket : class, DPacket
    {
        return packet as TPacket;
    }

    public static TPacket Get<TPacket>(DPacket packet) where TPacket : class, DPacket
    {
        if (packet is TPacket typed)
        {
            return typed;
        }

        throw new InvalidOperationException($"Packet {GetName(packet)} is not {typeof(TPacket).Name}.");
    }

    public static TPacket Require<TPacket>(DPacket packet) where TPacket : class, DPacket
    {
        return Get<TPacket>(packet);
    }

    public static bool TryGet<TPacket>(DPacket packet, out TPacket typed) where TPacket : class, DPacket
    {
        if (packet is TPacket value)
        {
            typed = value;
            return true;
        }

        typed = null!;
        return false;
    }

    public static bool TryAs<TPacket>(DPacket packet, out TPacket typed) where TPacket : class, DPacket
    {
        return TryGet(packet, out typed);
    }

    public static void Edit<TPacket>(DPacket packet, Action<TPacket> editor) where TPacket : class, DPacket
    {
        ArgumentNullException.ThrowIfNull(editor);
        editor(Get<TPacket>(packet));
    }

    public static bool TryEdit<TPacket>(DPacket packet, Action<TPacket> editor, out Exception? exception) where TPacket : class, DPacket
    {
        try
        {
            Edit(packet, editor);
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
    public static IReadOnlyList<PacketFieldSnapshot> Inspect(DPacket packet)
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

    public static IReadOnlyList<PacketFieldSnapshot> GetFields(DPacket packet)
    {
        return Inspect(packet);
    }

    public static object? GetField(DPacket packet, string name)
    {
        if (!TryGetField(packet, name, out var value, out var exception))
        {
            throw exception ?? new InvalidOperationException($"Unable to read {name}.");
        }

        return value;
    }

    public static bool TryGetField(DPacket packet, string name, out object? value, out Exception? exception)
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

    public static void SetField(DPacket packet, string name, object? value)
    {
        if (!TrySetField(packet, name, value, out var exception))
        {
            throw exception ?? new InvalidOperationException($"Unable to set {name}.");
        }
    }

    public static bool TrySetField(DPacket packet, string name, object? value, out Exception? exception)
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

    private static bool TryGetJson(DPacketValue value, out string json, out Exception? exception)
    {
        try
        {
            json = GetJson(value);
            exception = null;
            return true;
        }
        catch (Exception error)
        {
            json = string.Empty;
            exception = error;
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
            var json = value is DPacketValue packetValue && TryGetJson(packetValue, out var valueJson, out _) ? valueJson : null;
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
            DPacketValue packetValue => TryGetJson(packetValue, out var json, out _) ? json : "<complex>",
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
