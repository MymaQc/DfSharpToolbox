namespace Toolbox.Forms;

public sealed class CustomFormResponse
{
    private readonly IReadOnlyDictionary<string, string[]> _options;
    private readonly IReadOnlyDictionary<string, object?> _values;

    internal CustomFormResponse(IReadOnlyDictionary<string, object?> values, IReadOnlyDictionary<string, string[]> options)
    {
        _values = values;
        _options = options;
    }

    public bool HasValue(string name)
    {
        return _values.ContainsKey(name);
    }

    public string GetString(string name, string fallback = "")
    {
        return _values.TryGetValue(name, out var value) && value is string typed ? typed : fallback;
    }

    public bool GetBool(string name, bool fallback = false)
    {
        return _values.TryGetValue(name, out var value) && value is bool typed ? typed : fallback;
    }

    public double GetFloat(string name, double fallback = 0)
    {
        return _values.TryGetValue(name, out var value) && value is double typed ? typed : fallback;
    }

    private int GetInt(string name, int fallback = 0)
    {
        return _values.TryGetValue(name, out var value) && value is int typed ? typed : fallback;
    }

    public string GetSelectedOption(string name, string fallback = "")
    {
        var index = GetInt(name, -1);
        return _options.TryGetValue(name, out var options) && (uint)index < (uint)options.Length ? options[index] : fallback;
    }
}
