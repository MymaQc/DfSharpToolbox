using Toolbox;
using Toolbox.Configs;

namespace ToolboxExample;

public sealed class ExampleMain : ToolboxPlugin
{
    private readonly ExampleState _state = new();
    private JsonConfig? _config;

    public override void OnEnable()
    {
        base.OnEnable();
        _config = ExampleConfigDemo.Open(this);
        ExampleConfigDemo.Register(_config);
        ExampleListeners.Register(this, _state);
        ExampleEventDiagnostics.Register(this, _state);
        ExampleCommands.Register(this, _state);
        Console.WriteLine("ToolboxExample enabled. Use /tbxhelp in game.");
    }

    public override void OnDisable()
    {
        _config?.Save();
        _state.StopRepeatingTask();
        base.OnDisable();
        Console.WriteLine("ToolboxExample disabled.");
    }
}
