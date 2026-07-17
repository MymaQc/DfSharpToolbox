using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization.Metadata;

namespace Toolbox.Configs;

public sealed class JsonConfig
{
    private static readonly Encoding Utf8NoBom = new UTF8Encoding(false);
    private readonly Lock _sync = new();
    private JsonObject? _defaults;
    private JsonObject _root = new();

    public JsonConfig(string filePath,
        IReadOnlyDictionary<string, object?>? defaults = null,
        bool saveDefaults = true,
        JsonSerializerOptions? jsonOptions = null)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException("Config file path cannot be empty.", nameof(filePath));
        }

        FilePath = Path.GetFullPath(filePath);
        JsonOptions = jsonOptions is null ? CreateDefaultOptions() : new JsonSerializerOptions(jsonOptions);
        _defaults = defaults is null ? null : ToJsonObject(defaults);

        Reload(saveDefaults);
    }

    private string FilePath { get; }

    private JsonSerializerOptions JsonOptions { get; }

    public void Reload(bool saveDefaults = true)
    {
        lock (_sync)
        {
            var fileExists = File.Exists(FilePath);
            _root = fileExists ? LoadFromDisk() : new JsonObject();

            var changed = !fileExists;
            if (_defaults is not null)
            {
                changed |= MergeMissing(_root, _defaults);
            }

            if (changed && saveDefaults)
            {
                SaveUnsafe();
            }
        }
    }

    public void Save()
    {
        lock (_sync)
        {
            SaveUnsafe();
        }
    }

    public bool AddDefaults(IReadOnlyDictionary<string, object?> defaults, bool save = true)
    {
        ArgumentNullException.ThrowIfNull(defaults);
        return AddDefaults(ToJsonObject(defaults), save);
    }

    private bool AddDefaults(JsonObject defaults, bool save = true)
    {
        ArgumentNullException.ThrowIfNull(defaults);

        lock (_sync)
        {
            _defaults ??= new JsonObject();
            MergeMissing(_defaults, defaults);

            var changed = MergeMissing(_root, defaults);
            if (changed && save)
            {
                SaveUnsafe();
            }

            return changed;
        }
    }

    public Dictionary<string, object?> GetAll()
    {
        lock (_sync)
        {
            return ToObjectDictionary(_root);
        }
    }

    public JsonObject GetJsonObject()
    {
        lock (_sync)
        {
            return CloneObject(_root);
        }
    }

    public void SetAll(IReadOnlyDictionary<string, object?> values, bool save = false)
    {
        ArgumentNullException.ThrowIfNull(values);

        lock (_sync)
        {
            _root = ToJsonObject(values);
            if (save)
            {
                SaveUnsafe();
            }
        }
    }

    public void SetAll(JsonObject values, bool save = false)
    {
        ArgumentNullException.ThrowIfNull(values);

        lock (_sync)
        {
            _root = CloneObject(values);
            if (save)
            {
                SaveUnsafe();
            }
        }
    }

    public void Clear(bool save = false)
    {
        lock (_sync)
        {
            _root = new JsonObject();
            if (save)
            {
                SaveUnsafe();
            }
        }
    }

    public bool Exists(string key)
    {
        ValidateKey(key, nameof(key));

        lock (_sync)
        {
            return _root.ContainsKey(key);
        }
    }

    public bool ExistsNested(string path, char separator = '.')
    {
        var segments = SplitPath(path, separator);

        lock (_sync)
        {
            return TryGetNestedNodeUnsafe(segments, out _);
        }
    }

    public object? Get(string key, object? defaultValue = null)
    {
        ValidateKey(key, nameof(key));

        lock (_sync)
        {
            return _root.TryGetPropertyValue(key, out var value) ? ToPlainObject(value) : defaultValue;
        }
    }

    public JsonNode? GetNode(string key)
    {
        ValidateKey(key, nameof(key));

        lock (_sync)
        {
            return _root.TryGetPropertyValue(key, out var value) ? value?.DeepClone() : null;
        }
    }

    [RequiresDynamicCode("Use the overload that accepts JsonTypeInfo<T> for NativeAOT-safe config models.")]
    [RequiresUnreferencedCode("Use the overload that accepts JsonTypeInfo<T> for NativeAOT-safe config models.")]
    public T? Get<T>(string key, T? defaultValue = default)
    {
        ValidateKey(key, nameof(key));

        lock (_sync)
        {
            return _root.TryGetPropertyValue(key, out var value) ? Deserialize(value, defaultValue) : defaultValue;
        }
    }

    public T? Get<T>(string key, JsonTypeInfo<T> typeInfo, T? defaultValue = default)
    {
        ArgumentNullException.ThrowIfNull(typeInfo);
        ValidateKey(key, nameof(key));

        lock (_sync)
        {
            return _root.TryGetPropertyValue(key, out var value) ? Deserialize(value, typeInfo, defaultValue) : defaultValue;
        }
    }

    public string GetString(string key, string defaultValue = "")
    {
        ValidateKey(key, nameof(key));

        lock (_sync)
        {
            return _root.TryGetPropertyValue(key, out var value) ? ReadString(value, defaultValue) : defaultValue;
        }
    }

    public int GetInt(string key, int defaultValue = 0)
    {
        ValidateKey(key, nameof(key));

        lock (_sync)
        {
            return _root.TryGetPropertyValue(key, out var value) && TryReadInt(value, out var result) ? result : defaultValue;
        }
    }

    public double GetDouble(string key, double defaultValue = 0)
    {
        ValidateKey(key, nameof(key));

        lock (_sync)
        {
            return _root.TryGetPropertyValue(key, out var value) && TryReadDouble(value, out var result) ? result : defaultValue;
        }
    }

    public bool GetBool(string key, bool defaultValue = false)
    {
        ValidateKey(key, nameof(key));

        lock (_sync)
        {
            return _root.TryGetPropertyValue(key, out var value) && TryReadBool(value, out var result) ? result : defaultValue;
        }
    }

    public string GetNestedString(string path, string defaultValue = "", char separator = '.')
    {
        var segments = SplitPath(path, separator);

        lock (_sync)
        {
            return TryGetNestedNodeUnsafe(segments, out var value) ? ReadString(value, defaultValue) : defaultValue;
        }
    }

    public int GetNestedInt(string path, int defaultValue = 0, char separator = '.')
    {
        var segments = SplitPath(path, separator);

        lock (_sync)
        {
            return TryGetNestedNodeUnsafe(segments, out var value) && TryReadInt(value, out var result) ? result : defaultValue;
        }
    }

    public double GetNestedDouble(string path, double defaultValue = 0, char separator = '.')
    {
        var segments = SplitPath(path, separator);

        lock (_sync)
        {
            return TryGetNestedNodeUnsafe(segments, out var value) && TryReadDouble(value, out var result) ? result : defaultValue;
        }
    }

    public bool GetNestedBool(string path, bool defaultValue = false, char separator = '.')
    {
        var segments = SplitPath(path, separator);

        lock (_sync)
        {
            return TryGetNestedNodeUnsafe(segments, out var value) && TryReadBool(value, out var result) ? result : defaultValue;
        }
    }

    public object? GetNested(string path, object? defaultValue = null, char separator = '.')
    {
        var segments = SplitPath(path, separator);

        lock (_sync)
        {
            return TryGetNestedNodeUnsafe(segments, out var value) ? ToPlainObject(value) : defaultValue;
        }
    }

    public JsonNode? GetNestedNode(string path, char separator = '.')
    {
        var segments = SplitPath(path, separator);

        lock (_sync)
        {
            return TryGetNestedNodeUnsafe(segments, out var value) ? value?.DeepClone() : null;
        }
    }

    [RequiresDynamicCode("Use the overload that accepts JsonTypeInfo<T> for NativeAOT-safe config models.")]
    [RequiresUnreferencedCode("Use the overload that accepts JsonTypeInfo<T> for NativeAOT-safe config models.")]
    public T? GetNested<T>(string path, T? defaultValue = default, char separator = '.')
    {
        var segments = SplitPath(path, separator);

        lock (_sync)
        {
            return TryGetNestedNodeUnsafe(segments, out var value) ? Deserialize(value, defaultValue) : defaultValue;
        }
    }

    public T? GetNested<T>(string path, JsonTypeInfo<T> typeInfo, T? defaultValue = default, char separator = '.')
    {
        ArgumentNullException.ThrowIfNull(typeInfo);
        var segments = SplitPath(path, separator);

        lock (_sync)
        {
            return TryGetNestedNodeUnsafe(segments, out var value) ? Deserialize(value, typeInfo, defaultValue) : defaultValue;
        }
    }

    public void Set(string key, object? value, bool save = false)
    {
        ValidateKey(key, nameof(key));

        lock (_sync)
        {
            _root[key] = ToJsonNode(value);
            if (save)
            {
                SaveUnsafe();
            }
        }
    }

    public void SetNode(string key, JsonNode? value, bool save = false)
    {
        ValidateKey(key, nameof(key));

        lock (_sync)
        {
            _root[key] = value?.DeepClone();
            if (save)
            {
                SaveUnsafe();
            }
        }
    }

    [RequiresDynamicCode("Use Set(key, value, JsonTypeInfo<T>) for NativeAOT-safe config models.")]
    [RequiresUnreferencedCode("Use Set(key, value, JsonTypeInfo<T>) for NativeAOT-safe config models.")]
    public void Set<T>(string key, T value, bool save = false)
    {
        ValidateKey(key, nameof(key));

        lock (_sync)
        {
            _root[key] = JsonSerializer.SerializeToNode(value, JsonOptions);
            if (save)
            {
                SaveUnsafe();
            }
        }
    }

    public void Set<T>(string key, T value, JsonTypeInfo<T> typeInfo, bool save = false)
    {
        ArgumentNullException.ThrowIfNull(typeInfo);
        ValidateKey(key, nameof(key));

        lock (_sync)
        {
            _root[key] = JsonSerializer.SerializeToNode(value, typeInfo);
            if (save)
            {
                SaveUnsafe();
            }
        }
    }

    public void SetNested(string path, object? value, bool save = false, char separator = '.')
    {
        var segments = SplitPath(path, separator);

        lock (_sync)
        {
            SetNestedNodeUnsafe(segments, ToJsonNode(value));
            if (save)
            {
                SaveUnsafe();
            }
        }
    }

    public void SetNestedNode(string path, JsonNode? value, bool save = false, char separator = '.')
    {
        var segments = SplitPath(path, separator);

        lock (_sync)
        {
            SetNestedNodeUnsafe(segments, value?.DeepClone());
            if (save)
            {
                SaveUnsafe();
            }
        }
    }

    public bool Remove(string key, bool save = false)
    {
        ValidateKey(key, nameof(key));

        lock (_sync)
        {
            var removed = _root.Remove(key);
            if (removed && save)
            {
                SaveUnsafe();
            }

            return removed;
        }
    }

    public bool RemoveNested(string path, bool save = false, char separator = '.')
    {
        var segments = SplitPath(path, separator);

        lock (_sync)
        {
            var removed = RemoveNestedUnsafe(segments);
            if (removed && save)
            {
                SaveUnsafe();
            }

            return removed;
        }
    }

    private static string ReadString(JsonNode? node, string defaultValue)
    {
        return ToPlainObject(node) switch
        {
            null => defaultValue,
            string value => value,
            IFormattable formattable => formattable.ToString(null, CultureInfo.InvariantCulture),
            var value => value.ToString() ?? defaultValue,
        };
    }

    private static JsonSerializerOptions CreateDefaultOptions()
    {
        return new JsonSerializerOptions
        {
            AllowTrailingCommas = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
            WriteIndented = true,
        };
    }

    private JsonObject LoadFromDisk()
    {
        if (new FileInfo(FilePath).Length == 0)
        {
            return new JsonObject();
        }

        try
        {
            using var stream = File.OpenRead(FilePath);
            var nodeOptions = new JsonNodeOptions { PropertyNameCaseInsensitive = JsonOptions.PropertyNameCaseInsensitive };
            var documentOptions = new JsonDocumentOptions
            {
                AllowTrailingCommas = JsonOptions.AllowTrailingCommas,
                CommentHandling = JsonOptions.ReadCommentHandling,
            };
            var node = JsonNode.Parse(stream, nodeOptions, documentOptions);
            return node as JsonObject ?? throw new InvalidDataException($"Config root must be a JSON object: {FilePath}");
        }
        catch (JsonException exception)
        {
            throw new InvalidDataException($"Unable to parse JSON config: {FilePath}", exception);
        }
    }

    private void SaveUnsafe()
    {
        var directory = Path.GetDirectoryName(FilePath);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllText(FilePath, _root.ToJsonString(JsonOptions) + Environment.NewLine, Utf8NoBom);
    }

    private bool TryGetNestedNodeUnsafe(IReadOnlyList<string> segments, out JsonNode? value)
    {
        JsonNode? current = _root;
        foreach (var segment in segments)
        {
            if (current is JsonObject obj && obj.TryGetPropertyValue(segment, out current))
            {
                continue;
            }
            value = null;
            return false;
        }

        value = current;
        return true;
    }

    private void SetNestedNodeUnsafe(IReadOnlyList<string> segments, JsonNode? value)
    {
        var current = _root;
        for (var i = 0; i < segments.Count - 1; i++)
        {
            var segment = segments[i];
            if (!current.TryGetPropertyValue(segment, out var next) || next is not JsonObject nextObject)
            {
                nextObject = new JsonObject();
                current[segment] = nextObject;
            }

            current = nextObject;
        }

        current[segments[^1]] = value;
    }

    private bool RemoveNestedUnsafe(IReadOnlyList<string> segments)
    {
        var current = _root;
        for (var i = 0; i < segments.Count - 1; i++)
        {
            if (!current.TryGetPropertyValue(segments[i], out var next) || next is not JsonObject nextObject)
            {
                return false;
            }

            current = nextObject;
        }

        return current.Remove(segments[^1]);
    }

    private static bool MergeMissing(JsonObject target, JsonObject defaults)
    {
        var changed = false;
        foreach (var pair in defaults)
        {
            if (!target.TryGetPropertyValue(pair.Key, out var current))
            {
                target[pair.Key] = pair.Value?.DeepClone();
                changed = true;
                continue;
            }

            if (current is JsonObject currentObject && pair.Value is JsonObject defaultObject)
            {
                changed |= MergeMissing(currentObject, defaultObject);
            }
        }

        return changed;
    }

    private JsonObject ToJsonObject(IReadOnlyDictionary<string, object?> values)
    {
        var obj = new JsonObject();
        foreach (var pair in values)
        {
            ValidateKey(pair.Key, nameof(values));
            obj[pair.Key] = ToJsonNode(pair.Value);
        }

        return obj;
    }

    private static JsonObject CloneObject(JsonObject obj)
    {
        return (JsonObject)obj.DeepClone();
    }

    private JsonNode? ToJsonNode(object? value)
    {
        return value switch
        {
            null => null,
            JsonNode node => node.DeepClone(),
            JsonElement element => JsonNode.Parse(element.GetRawText()),
            string text => JsonValue.Create(text),
            char character => JsonValue.Create(character.ToString()),
            bool boolean => JsonValue.Create(boolean),
            byte number => JsonValue.Create(number),
            sbyte number => JsonValue.Create(number),
            short number => JsonValue.Create(number),
            ushort number => JsonValue.Create(number),
            int number => JsonValue.Create(number),
            uint number => JsonValue.Create(number),
            long number => JsonValue.Create(number),
            ulong number => JsonValue.Create(number),
            float number => JsonValue.Create(number),
            double number => JsonValue.Create(number),
            decimal number => JsonValue.Create(number),
            Guid guid => JsonValue.Create(guid),
            DateTime dateTime => JsonValue.Create(dateTime),
            DateTimeOffset dateTime => JsonValue.Create(dateTime),
            IReadOnlyDictionary<string, object?> dictionary => ToJsonObject(dictionary),
            IDictionary dictionary => ToJsonObject(dictionary),
            IEnumerable enumerable and not string => ToJsonArray(enumerable),
            _ => throw new NotSupportedException($"Config value type is not supported directly: {value.GetType().FullName}")
        };
    }

    private JsonObject ToJsonObject(IDictionary values)
    {
        var obj = new JsonObject();
        foreach (DictionaryEntry entry in values)
        {
            if (entry.Key is not string key)
            {
                throw new NotSupportedException("Config object keys must be strings.");
            }

            ValidateKey(key, nameof(values));
            obj[key] = ToJsonNode(entry.Value);
        }

        return obj;
    }

    private JsonArray ToJsonArray(IEnumerable values)
    {
        var array = new JsonArray();
        foreach (var item in values)
        {
            array.Add(ToJsonNode(item));
        }

        return array;
    }

    private static Dictionary<string, object?> ToObjectDictionary(JsonObject obj)
    {
        var values = new Dictionary<string, object?>(StringComparer.Ordinal);
        foreach (var pair in obj)
        {
            values[pair.Key] = ToPlainObject(pair.Value);
        }

        return values;
    }

    private static object? ToPlainObject(JsonNode? node)
    {
        return node switch
        {
            null => null,
            JsonObject obj => ToObjectDictionary(obj),
            JsonArray array => array.Select(ToPlainObject).ToList(),
            JsonValue value => ToPlainValue(value),
            _ => null,
        };
    }

    private static object? ToPlainValue(JsonValue value)
    {
        if (value.TryGetValue<JsonElement>(out var element))
        {
            return ToPlainObject(element);
        }

        if (value.TryGetValue<string>(out var text))
        {
            return text;
        }

        if (value.TryGetValue<bool>(out var boolean))
        {
            return boolean;
        }

        if (value.TryGetValue<int>(out var integer))
        {
            return integer;
        }

        if (value.TryGetValue<long>(out var longNumber))
        {
            return longNumber;
        }

        if (value.TryGetValue<double>(out var doubleNumber))
        {
            return doubleNumber;
        }

        if (value.TryGetValue<decimal>(out var decimalNumber))
        {
            return decimalNumber;
        }

        return value.ToJsonString();
    }

    private static object? ToPlainObject(JsonElement element)
    {
        return element.ValueKind switch
        {
            JsonValueKind.Object => element.EnumerateObject()
                .ToDictionary(property => property.Name, property => ToPlainObject(property.Value), StringComparer.Ordinal),
            JsonValueKind.Array => element.EnumerateArray().Select(ToPlainObject).ToList(),
            JsonValueKind.String => element.GetString(),
            JsonValueKind.Number when element.TryGetInt32(out var integer) => integer,
            JsonValueKind.Number when element.TryGetInt64(out var longNumber) => longNumber,
            JsonValueKind.Number when element.TryGetDecimal(out var decimalNumber) => decimalNumber,
            JsonValueKind.Number => element.GetDouble(),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            _ => null,
        };
    }

    [RequiresDynamicCode("Use the overload that accepts JsonTypeInfo<T> for NativeAOT-safe config models.")]
    [RequiresUnreferencedCode("Use the overload that accepts JsonTypeInfo<T> for NativeAOT-safe config models.")]
    private T? Deserialize<T>(JsonNode? node, T? defaultValue)
    {
        if (node is null)
        {
            return defaultValue;
        }

        try
        {
            return node.Deserialize<T>(JsonOptions) ?? defaultValue;
        }
        catch (JsonException)
        {
            return defaultValue;
        }
        catch (NotSupportedException)
        {
            return defaultValue;
        }
    }

    private static T? Deserialize<T>(JsonNode? node, JsonTypeInfo<T> typeInfo, T? defaultValue)
    {
        if (node is null)
        {
            return defaultValue;
        }

        try
        {
            return node.Deserialize(typeInfo) ?? defaultValue;
        }
        catch (JsonException)
        {
            return defaultValue;
        }
        catch (NotSupportedException)
        {
            return defaultValue;
        }
    }

    private static bool TryReadInt(JsonNode? node, out int value)
    {
        if (node is JsonValue jsonValue)
        {
            if (jsonValue.TryGetValue<JsonElement>(out var element))
            {
                return TryReadInt(element, out value);
            }

            if (jsonValue.TryGetValue<int>(out value))
            {
                return true;
            }

            if (jsonValue.TryGetValue<long>(out var longNumber) && longNumber is >= int.MinValue and <= int.MaxValue)
            {
                value = (int)longNumber;
                return true;
            }

            if (jsonValue.TryGetValue<string>(out var text))
            {
                return int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out value);
            }
        }

        value = 0;
        return false;
    }

    private static bool TryReadInt(JsonElement element, out int value)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Number:
                return element.TryGetInt32(out value);
            case JsonValueKind.String:
                return int.TryParse(element.GetString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out value);
            case JsonValueKind.Undefined:
            case JsonValueKind.Object:
            case JsonValueKind.Array:
            case JsonValueKind.True:
            case JsonValueKind.False:
            case JsonValueKind.Null:
            default:
                value = 0;
                return false;
        }
    }

    private static bool TryReadDouble(JsonNode? node, out double value)
    {
        if (node is JsonValue jsonValue)
        {
            if (jsonValue.TryGetValue<JsonElement>(out var element))
            {
                return TryReadDouble(element, out value);
            }

            if (jsonValue.TryGetValue(out value))
            {
                return true;
            }

            if (jsonValue.TryGetValue<int>(out var integer))
            {
                value = integer;
                return true;
            }

            if (jsonValue.TryGetValue<long>(out var longNumber))
            {
                value = longNumber;
                return true;
            }

            if (jsonValue.TryGetValue<string>(out var text))
            {
                return double.TryParse(text, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out value);
            }
        }

        value = 0;
        return false;
    }

    private static bool TryReadDouble(JsonElement element, out double value)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Number:
                return element.TryGetDouble(out value);
            case JsonValueKind.String:
                return double.TryParse(element.GetString(), NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out value);
            case JsonValueKind.Undefined:
            case JsonValueKind.Object:
            case JsonValueKind.Array:
            case JsonValueKind.True:
            case JsonValueKind.False:
            case JsonValueKind.Null:
            default:
                value = 0;
                return false;
        }
    }

    private static bool TryReadBool(JsonNode? node, out bool value)
    {
        if (node is JsonValue jsonValue)
        {
            if (jsonValue.TryGetValue<JsonElement>(out var element))
            {
                return TryReadBool(element, out value);
            }

            if (jsonValue.TryGetValue(out value))
            {
                return true;
            }

            if (jsonValue.TryGetValue<string>(out var text))
            {
                return bool.TryParse(text, out value);
            }
        }

        value = false;
        return false;
    }

    private static bool TryReadBool(JsonElement element, out bool value)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.True:
                value = true;
                return true;
            case JsonValueKind.False:
                value = false;
                return true;
            case JsonValueKind.String:
                return bool.TryParse(element.GetString(), out value);
            case JsonValueKind.Undefined:
            case JsonValueKind.Object:
            case JsonValueKind.Array:
            case JsonValueKind.Number:
            case JsonValueKind.Null:
            default:
                value = false;
                return false;
        }
    }

    private static void ValidateKey(string key, string paramName)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Config keys cannot be empty.", paramName);
        }
    }

    private static string[] SplitPath(string path, char separator)
    {
        ValidateKey(path, nameof(path));

        var segments = path.Split(separator);
        if (segments.Length == 0 || segments.Any(string.IsNullOrWhiteSpace))
        {
            throw new ArgumentException("Nested config paths cannot contain empty segments.", nameof(path));
        }

        return segments;
    }
}
