using Dragonfly;

namespace Toolbox.Worlds.States;

public sealed record CustomDimension(
    Cube.Range BuildRange,
    bool EvaporatesWater,
    TimeSpan LavaDuration,
    bool HasWeatherCycle,
    bool HasTimeCycle) : World.Dimension
{
    public Cube.Range Range()
    {
        return BuildRange;
    }

    public bool WaterEvaporates()
    {
        return EvaporatesWater;
    }

    public TimeSpan LavaSpreadDuration()
    {
        return LavaDuration;
    }

    public bool WeatherCycle()
    {
        return HasWeatherCycle;
    }

    public bool TimeCycle()
    {
        return HasTimeCycle;
    }
}
