using Dragonfly;
using Toolbox.Worlds.States;

namespace Toolbox.Worlds;

public static class DimensionApi
{
    public static World.Dimension GetPresetDimension(DimensionPreset preset)
    {
        return preset switch
        {
            DimensionPreset.Overworld => World.Overworld,
            DimensionPreset.Nether => World.Nether,
            DimensionPreset.End => World.End,
            _ => throw new ArgumentOutOfRangeException(nameof(preset), preset, null),
        };
    }

    public static CustomDimension CreateCustomDimension(
        int minY = -64,
        int maxY = 319,
        bool waterEvaporates = false,
        TimeSpan lavaSpreadDuration = default,
        bool weatherCycle = true,
        bool timeCycle = true)
    {
        return new CustomDimension(
            new Cube.Range(minY, maxY),
            waterEvaporates,
            lavaSpreadDuration == TimeSpan.Zero ? TimeSpan.FromMilliseconds(1500) : lavaSpreadDuration,
            weatherCycle,
            timeCycle);
    }

    public static CustomDimension CreateNetherLikeDimension(int minY = 0, int maxY = 127)
    {
        return CreateCustomDimension(minY, maxY, waterEvaporates: true, TimeSpan.FromMilliseconds(250), weatherCycle: false, timeCycle: false);
    }

    public static Cube.Range GetRange(World.Dimension dimension)
    {
        return dimension.Range();
    }

    public static bool DoesWaterEvaporate(World.Dimension dimension)
    {
        return dimension.WaterEvaporates();
    }

    public static TimeSpan GetLavaSpreadDuration(World.Dimension dimension)
    {
        return dimension.LavaSpreadDuration();
    }

    public static bool HasWeatherCycle(World.Dimension dimension)
    {
        return dimension.WeatherCycle();
    }

    public static bool HasTimeCycle(World.Dimension dimension)
    {
        return dimension.TimeCycle();
    }
}
