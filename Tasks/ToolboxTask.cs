using Dragonfly;

namespace Toolbox.Tasks;

public sealed class ToolboxTask
{
    private readonly World.Task _task;

    internal ToolboxTask(World.Task task)
    {
        _task = task;
    }

    public World.Task GetDragonflyTask()
    {
        return _task;
    }

    public Task GetDoneTask()
    {
        return _task.Done();
    }

    public Exception? GetError()
    {
        return _task.Err();
    }

    public bool Cancel()
    {
        return _task.Cancel();
    }
}
