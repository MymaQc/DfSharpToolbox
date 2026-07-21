using Dragonfly;
using Toolbox;
using Toolbox.Configs;
using Toolbox.Entities;

namespace ToolboxExample;

public sealed class ExampleMain : ToolboxPlugin
{
    private readonly ExampleState _state = new();
    private readonly ExampleCustomItemSet? _customItems = null;
    private readonly ExampleCustomBlockSet? _customBlocks = ExampleCustomBlocks.Register();
    private readonly CustomEntityType<ExampleEntities.ExampleEntityMarker> _customEntity = ExampleEntities.Register();
    private JsonConfig? _config;

    public override void OnEnable()
    {
        base.OnEnable();
        _config = ExampleConfigDemo.Open(this);
        ExampleConfigDemo.Register(_config);
        ExampleListeners.Register(this, _state);
        ExampleEventDiagnostics.Register(this, _state);
        ExampleCommands.Register(this, _state, _customItems, _customBlocks);
        ExampleEntities.RegisterCommand(_customEntity);
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
