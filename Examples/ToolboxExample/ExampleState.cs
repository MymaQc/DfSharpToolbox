using Toolbox.Tasks;

namespace ToolboxExample;

internal sealed class ExampleState
{
    private RepeatingToolboxTask? _repeatingTask;

    public int JoinCount { get; private set; }

    public int RepeatingTaskRuns { get; private set; }

    public bool PacketLogEnabled { get; private set; }

    public string LastConnection { get; private set; } = "aucune connexion";

    public string LastPacket { get; private set; } = "aucun packet";

    public bool HasRepeatingTask()
    {
        return _repeatingTask is not null && !_repeatingTask.IsCancelled();
    }

    public void RecordJoin()
    {
        JoinCount++;
    }

    public void RecordConnection(string value)
    {
        LastConnection = value;
    }

    public void SetPacketLogEnabled(bool enabled)
    {
        PacketLogEnabled = enabled;
    }

    public void RecordPacket(string value)
    {
        LastPacket = value;
    }

    public void SetRepeatingTask(RepeatingToolboxTask task)
    {
        StopRepeatingTask();
        _repeatingTask = task;
        RepeatingTaskRuns = 0;
    }

    public int IncrementRepeatingTaskRuns()
    {
        RepeatingTaskRuns++;
        return RepeatingTaskRuns;
    }

    public void StopRepeatingTask()
    {
        _repeatingTask?.Cancel();
        _repeatingTask = null;
    }
}
