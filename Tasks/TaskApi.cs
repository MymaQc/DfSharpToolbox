using Dragonfly;

namespace Toolbox.Tasks;

public static class TaskApi
{
    private static readonly TimeSpan TickDuration = TimeSpan.FromMilliseconds(50);

    public static ToolboxTask RunTask(World world, Action<World.Tx> callback)
    {
        ArgumentNullException.ThrowIfNull(world);
        ArgumentNullException.ThrowIfNull(callback);
        return new ToolboxTask(world.Do(callback));
    }

    private static ToolboxTask RunTaskLater(World world, TimeSpan delay, Action<World.Tx> callback)
    {
        ArgumentNullException.ThrowIfNull(world);
        ArgumentNullException.ThrowIfNull(callback);
        return new ToolboxTask(world.DoAfter(delay, callback));
    }

    public static ToolboxTask RunTaskLater(World world, long delayTicks, Action<World.Tx> callback)
    {
        return RunTaskLater(world, Ticks(delayTicks), callback);
    }

    private static RepeatingToolboxTask RunTaskTimer(World world, TimeSpan delay, TimeSpan period, Action<World.Tx> callback)
    {
        ArgumentNullException.ThrowIfNull(world);
        ArgumentNullException.ThrowIfNull(callback);
        return period <= TimeSpan.Zero ? throw new ArgumentOutOfRangeException(nameof(period), "Task period must be greater than zero.")
            : new RepeatingToolboxTask(world, delay, period, callback);
    }

    public static RepeatingToolboxTask RunTaskTimer(World world, long delayTicks, long periodTicks, Action<World.Tx> callback)
    {
        return RunTaskTimer(world, Ticks(delayTicks), Ticks(periodTicks), callback);
    }

    public static TimeSpan Ticks(long ticks)
    {
        return TimeSpan.FromTicks(checked(TickDuration.Ticks * System.Math.Max(0, ticks)));
    }
}
