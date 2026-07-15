namespace Toolbox.Timing;

public static class TimeApi
{
    private const long TickMilliseconds = 50;

    public static TimeSpan ConvertGameTicksToDuration(long ticks)
    {
        return TimeSpan.FromMilliseconds(System.Math.Max(0, ticks) * TickMilliseconds);
    }

    public static long ConvertDurationToGameTicks(TimeSpan duration)
    {
        return System.Math.Max(0, (long)System.Math.Ceiling(duration.TotalMilliseconds / TickMilliseconds));
    }
}
