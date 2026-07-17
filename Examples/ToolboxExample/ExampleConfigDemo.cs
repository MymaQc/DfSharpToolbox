using Dragonfly;
using Toolbox;
using Toolbox.Commands;
using Toolbox.Configs;

namespace ToolboxExample;

internal static class ExampleConfigDemo
{
    public static JsonConfig Open(ToolboxPlugin plugin)
    {
        return ConfigApi.OpenPluginConfig(plugin, defaults: CreateDefaults());
    }

    public static void Register(JsonConfig config)
    {
        ToolboxConfigCommand.Config = config;
        CommandApi.RegisterCommand<ToolboxConfigCommand>(
            "tbxconfig",
            "Teste JsonConfig: <show|write|reload|defaults|remove|reset> [value].",
            "tbxconf");
    }

    public static IReadOnlyDictionary<string, object?> CreateDefaults()
    {
        return new Dictionary<string, object?>
        {
            ["enabled"] = true,
            ["motd"] = "Bienvenue sur ToolboxExample",
            ["limits"] = new Dictionary<string, object?>
            {
                ["maxHomes"] = 3,
                ["warningThreshold"] = 0.75,
            },
            ["messages"] = new Dictionary<string, object?>
            {
                ["prefix"] = "[ToolboxExample]",
            },
        };
    }
}

public enum ToolboxConfigAction
{
    Show,
    Write,
    Reload,
    Defaults,
    Remove,
    Reset,
}

public sealed class ToolboxConfigCommand : ToolboxCommand
{
    internal static JsonConfig? Config { get; set; }

    [Cmd.Tag("action")]
    public ToolboxConfigAction Action;

    [Cmd.Tag("value")]
    public Cmd.Optional<int> Value;

    protected override void Execute(CommandContext ctx)
    {
        var config = Config;
        if (config is null)
        {
            ctx.SendError("La config d'exemple n'est pas initialisee.");
            return;
        }

        switch (Action)
        {
            case ToolboxConfigAction.Show:
                Show(ctx, config);
                break;
            case ToolboxConfigAction.Write:
                Write(ctx, config);
                break;
            case ToolboxConfigAction.Reload:
                config.Reload();
                ctx.SendMessage("Config rechargee depuis le fichier JSON.");
                Show(ctx, config);
                break;
            case ToolboxConfigAction.Defaults:
                AddDefaults(ctx, config);
                break;
            case ToolboxConfigAction.Remove:
                Remove(ctx, config);
                break;
            case ToolboxConfigAction.Reset:
                config.SetAll(ExampleConfigDemo.CreateDefaults(), save: true);
                ctx.SendMessage("Config remise a ses valeurs d'exemple et sauvegardee.");
                Show(ctx, config);
                break;
        }
    }

    private static void Show(CommandContext ctx, JsonConfig config)
    {
        var enabled = config.GetBool("enabled");
        var motd = config.GetString("motd", "motd manquant");
        var maxHomes = config.GetNestedInt("limits.maxHomes", -1);
        var threshold = config.GetNestedDouble("limits.warningThreshold", -1);
        var prefix = config.GetNestedString("messages.prefix", "sans prefixe");
        var hasLastWrite = config.ExistsNested("runtime.lastWriteUtc");
        var keys = string.Join(", ", config.GetAll().Keys);

        ctx.SendMessage($"enabled={enabled}, motd={motd}");
        ctx.SendMessage($"limits.maxHomes={maxHomes}, warningThreshold={threshold:0.00}");
        ctx.SendMessage($"messages.prefix={prefix}, runtime.lastWriteUtc existe={hasLastWrite}");
        ctx.SendMessage($"Cles racine via GetAll(): [{keys}]");
    }

    private void Write(CommandContext ctx, JsonConfig config)
    {
        var current = config.GetNestedInt("limits.maxHomes", 3);
        var maxHomes = System.Math.Clamp(Value.LoadOr(current + 1), 0, 1000);
        var writeCount = config.GetNestedInt("runtime.writeCount") + 1;

        config.Set("enabled", (object?)!config.GetBool("enabled"));
        config.SetNested("limits.maxHomes", maxHomes);
        config.SetNested("runtime.writeCount", writeCount);
        config.SetNested("runtime.lastWriteUtc", DateTimeOffset.UtcNow.ToString("O"));
        config.Save();

        ctx.SendMessage($"Valeurs modifiees et sauvegardees: maxHomes={maxHomes}, writeCount={writeCount}.");
        Show(ctx, config);
    }

    private static void AddDefaults(CommandContext ctx, JsonConfig config)
    {
        var changed = config.AddDefaults(new Dictionary<string, object?>
        {
            ["features"] = new Dictionary<string, object?>
            {
                ["teleport"] = true,
                ["economy"] = false,
            },
            ["limits"] = new Dictionary<string, object?>
            {
                ["maxWarps"] = 5,
            },
        });

        ctx.SendMessage($"AddDefaults termine. Nouvelles valeurs ajoutees={changed}.");
        ctx.SendMessage($"features.teleport={config.GetNestedBool("features.teleport")}, limits.maxWarps={config.GetNestedInt("limits.maxWarps")}");
    }

    private static void Remove(CommandContext ctx, JsonConfig config)
    {
        var removed = config.RemoveNested("runtime.lastWriteUtc", save: true);
        ctx.SendMessage($"RemoveNested runtime.lastWriteUtc: removed={removed}, exists={config.ExistsNested("runtime.lastWriteUtc")}.");
    }
}
