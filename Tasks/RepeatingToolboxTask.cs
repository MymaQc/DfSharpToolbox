using Dragonfly;

namespace Toolbox.Tasks;

public sealed class RepeatingToolboxTask
{
    private readonly Action<World.Tx> _callback;
    private readonly Lock _lock = new();
    private readonly TimeSpan _period;
    private readonly World _world;
    private bool _cancelled;
    private ToolboxTask? _current;

    internal RepeatingToolboxTask(World world, TimeSpan delay, TimeSpan period, Action<World.Tx> callback)
    {
        _world = world;
        _period = period;
        _callback = callback;
        Schedule(delay);
    }

    public bool IsCancelled()
    {
        lock (_lock)
        {
            return _cancelled;
        }
    }

    public ToolboxTask? GetCurrentTask()
    {
        lock (_lock)
        {
            return _current;
        }
    }

    public void Cancel()
    {
        ToolboxTask? current;
        lock (_lock)
        {
            if (_cancelled)
            {
                return;
            }

            _cancelled = true;
            current = _current;
        }

        current?.Cancel();
    }

    private void Schedule(TimeSpan delay)
    {
        lock (_lock)
        {
            if (_cancelled)
            {
                return;
            }

            _current = new ToolboxTask(_world.DoAfter(delay, Run));
        }
    }

    private void Run(World.Tx tx)
    {
        if (IsCancelled())
        {
            return;
        }

        _callback(tx);
        Schedule(_period);
    }
}
