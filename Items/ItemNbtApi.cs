using Dragonfly;

namespace Toolbox.Items;

public static class ItemNbtApi
{
    public static IReadOnlyDictionary<string, object> GetNamedTag(Item.Stack stack)
    {
        return stack.Values();
    }

    public static bool HasTag(Item.Stack stack, string key)
    {
        return stack.Value(key).Ok;
    }

    public static (object? Value, bool Found) GetTag(Item.Stack stack, string key)
    {
        var (value, ok) = stack.Value(key);
        return (value, ok);
    }

    public static Item.Stack SetTag(Item.Stack stack, string key, object value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return stack.WithValue(key, value);
    }

    public static Item.Stack RemoveTag(Item.Stack stack, string key)
    {
        return stack.WithValue(key, null);
    }

    public static Item.Stack SetString(Item.Stack stack, string key, string value)
    {
        return SetTag(stack, key, value);
    }
    public static Item.Stack SetInt(Item.Stack stack, string key, int value)
    {
        return SetTag(stack, key, value);
    }
    public static Item.Stack SetLong(Item.Stack stack, string key, long value)
    {
        return SetTag(stack, key, value);
    }
    public static Item.Stack SetFloat(Item.Stack stack, string key, float value)
    {
        return SetTag(stack, key, value);
    }
    public static Item.Stack SetDouble(Item.Stack stack, string key, double value)
    {
        return SetTag(stack, key, value);
    }
    public static Item.Stack SetBool(Item.Stack stack, string key, bool value)
    {
        return SetTag(stack, key, value);
    }
    public static Item.Stack SetByteArray(Item.Stack stack, string key, byte[] value)
    {
        return SetTag(stack, key, value);
    }
    public static Item.Stack SetCompound(Item.Stack stack, string key, IReadOnlyDictionary<string, object> value)
    {
        return SetTag(stack, key, value.ToDictionary(entry => entry.Key, entry => entry.Value, StringComparer.Ordinal));
    }

    public static string GetString(Item.Stack stack, string key, string defaultValue = "")
    {
        return stack.Value(key) is { Ok: true, Value: string value } ? value : defaultValue;
    }

    public static int GetInt(Item.Stack stack, string key, int defaultValue = 0)
    {
        return stack.Value(key) is { Ok: true, Value: int value } ? value : defaultValue;
    }

    public static long GetLong(Item.Stack stack, string key, long defaultValue = 0)
    {
        return stack.Value(key) is { Ok: true, Value: long value } ? value : defaultValue;
    }

    public static float GetFloat(Item.Stack stack, string key, float defaultValue = 0)
    {
        return stack.Value(key) is { Ok: true, Value: float value } ? value : defaultValue;
    }

    public static double GetDouble(Item.Stack stack, string key, double defaultValue = 0)
    {
        return stack.Value(key) is { Ok: true, Value: double value } ? value : defaultValue;
    }

    public static bool GetBool(Item.Stack stack, string key, bool defaultValue = false)
    {
        return stack.Value(key) is { Ok: true, Value: bool value } ? value : defaultValue;
    }

    public static byte[] GetByteArray(Item.Stack stack, string key)
    {
        return stack.Value(key) is { Ok: true, Value: byte[] value } ? value : [];
    }

    public static Dictionary<string, object> CreateCompoundTag(params (string Key, object Value)[] values)
    {
        var result = new Dictionary<string, object>(StringComparer.Ordinal);
        foreach (var (key, value) in values)
        {
            ArgumentNullException.ThrowIfNull(value);
            result[key] = value;
        }

        return result;
    }
}
