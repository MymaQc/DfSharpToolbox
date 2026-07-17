namespace Toolbox.Configs;

public static class ConfigApi
{
    public const string DefaultConfigFileName = "config.json";

    public static JsonConfig OpenJsonConfig(string filePath,
        IReadOnlyDictionary<string, object?>? defaults = null,
        bool saveDefaults = true)
    {
        return new JsonConfig(filePath, defaults, saveDefaults);
    }

    public static JsonConfig OpenPluginConfig(ToolboxPlugin plugin,
        string fileName = DefaultConfigFileName,
        IReadOnlyDictionary<string, object?>? defaults = null,
        bool saveDefaults = true)
    {
        ArgumentNullException.ThrowIfNull(plugin);

        if (string.IsNullOrWhiteSpace(fileName))
        {
            fileName = DefaultConfigFileName;
        }

        var filePath = Path.IsPathRooted(fileName) ? fileName : Path.Combine(GetPluginDataFolder(plugin), fileName);
        return new JsonConfig(filePath, defaults, saveDefaults);
    }

    public static string GetPluginDataFolder(ToolboxPlugin plugin)
    {
        ArgumentNullException.ThrowIfNull(plugin);

        var pluginName = plugin.GetType().Assembly.GetName().Name;
        if (string.IsNullOrWhiteSpace(pluginName))
        {
            pluginName = plugin.GetType().Name;
        }

        return Path.Combine(Environment.CurrentDirectory, "plugins", SanitizePathSegment(pluginName));
    }

    private static string SanitizePathSegment(string value)
    {
        var invalidChars = Path.GetInvalidFileNameChars();
        var chars = value.Select(character => invalidChars.Contains(character) ? '_' : character).ToArray();
        var sanitized = new string(chars).Trim();
        return string.IsNullOrWhiteSpace(sanitized) ? "Plugin" : sanitized;
    }
}
