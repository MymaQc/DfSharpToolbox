using System.Globalization;

namespace Toolbox.Entities;

public sealed class EntityProperties
{
    private readonly Dictionary<string, object?> _values;

    public EntityProperties()
    {
        _values = new Dictionary<string, object?>(StringComparer.Ordinal);
    }

    internal EntityProperties(IEnumerable<KeyValuePair<string, object?>> values)
    {
        _values = new Dictionary<string, object?>(values, StringComparer.Ordinal);
    }

    public int Count => _values.Count;
    public IEnumerable<string> Names => _values.Keys;

    public bool Contains(string name) => _values.ContainsKey(ValidateName(name));

    public EntityProperties Set<T>(string name, T value)
    {
        ArgumentNullException.ThrowIfNull(value);
        _values[ValidateName(name)] = value;
        return this;
    }

    public T Get<T>(string name, T fallback = default!)
    {
        return TryGet<T>(name, out var value) ? value : fallback;
    }

    public T GetRequired<T>(string name)
    {
        return TryGet<T>(name, out var value) ? value : throw new KeyNotFoundException($"Entity property '{name}' was not found or is not a {typeof(T).Name}.");
    }

    public bool TryGet<T>(string name, out T value)
    {
        if (_values.TryGetValue(ValidateName(name), out var raw) && TryConvert(raw, out value))
        {
            return true;
        }
        value = default!;
        return false;
    }

    public bool Remove(string name) => _values.Remove(ValidateName(name));

    public void Clear() => _values.Clear();

    internal EntityProperties Clone() => new(_values);

    internal Dictionary<string, object?> Snapshot() => new(_values, StringComparer.Ordinal);

    internal void Merge(IEnumerable<KeyValuePair<string, object?>> values)
    {
        foreach (var (name, value) in values)
        {
            _values[name] = value;
        }
    }

    private static string ValidateName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        return name;
    }

    private static bool TryConvert<T>(object? raw, out T value)
    {
        if (raw is T typed)
        {
            value = typed;
            return true;
        }

        try
        {
            if (raw is IConvertible && typeof(IConvertible).IsAssignableFrom(typeof(T)))
            {
                value = (T)Convert.ChangeType(raw, typeof(T), CultureInfo.InvariantCulture);
                return true;
            }
        }
        catch (Exception exception) when (exception is InvalidCastException or FormatException or OverflowException)
        {
        }

        value = default!;
        return false;
    }
}
