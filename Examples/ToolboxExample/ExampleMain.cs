using Toolbox;

namespace ToolboxExample;

public sealed class ExampleMain : ToolboxPlugin
{
    private readonly ExampleState _state = new();

    public override void OnEnable()
    {
        base.OnEnable();
        ExampleListeners.Register(this, _state);
        ExampleCommands.Register(_state);
        Console.WriteLine("ToolboxExample enabled. Use /tbxhelp in game.");
    }

    public override void OnDisable()
    {
        _state.StopRepeatingTask();
        base.OnDisable();
        Console.WriteLine("ToolboxExample disabled.");
    }
}
