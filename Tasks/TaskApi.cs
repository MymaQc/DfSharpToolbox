using Dragonfly;
using Toolbox.Timing;

namespace Toolbox.Tasks;

public static class TaskApi
{
    public static ToolboxTask RunTask(World world, Action<World.Tx> callback)
    {
        ArgumentNullException.ThrowIfNull(world);
        ArgumentNullException.ThrowIfNull(callback);
        return new ToolboxTask(world.Do(callback));
    }

    public static ToolboxTask RunTaskLater(World world, TimeSpan delay, Action<World.Tx> callback)
    {
        ArgumentNullException.ThrowIfNull(world);
        ArgumentNullException.ThrowIfNull(callback);
        return new ToolboxTask(world.DoAfter(delay, callback));
    }

    public static ToolboxTask RunTaskLaterTicks(World world, long delayTicks, Action<World.Tx> callback)
    {
        return RunTaskLater(world, Ticks(delayTicks), callback);
    }

    public static RepeatingToolboxTask RunTaskTimer(World world, TimeSpan delay, TimeSpan period, Action<World.Tx> callback)
    {
        ArgumentNullException.ThrowIfNull(world);
        ArgumentNullException.ThrowIfNull(callback);
        return period <= TimeSpan.Zero ? throw new ArgumentOutOfRangeException(nameof(period), "Task period must be greater than zero.")
            : new RepeatingToolboxTask(world, delay, period, callback);
    }

    public static RepeatingToolboxTask RunTaskTimerTicks(World world, long delayTicks, long periodTicks, Action<World.Tx> callback)
    {
        return RunTaskTimer(world, Ticks(delayTicks), Ticks(periodTicks), callback);
    }

    private static TimeSpan Ticks(long ticks)
    {
        return TimeApi.ConvertGameTicksToDuration(ticks);
    }
}
