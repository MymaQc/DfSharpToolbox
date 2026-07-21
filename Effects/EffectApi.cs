using Dragonfly;
using Toolbox.Timing;

namespace Toolbox.Effects;

public static class EffectApi
{
    private static Effect.Value Create(Effect.LastingType type, int level, TimeSpan duration, bool hideParticles = false, bool ambient = false)
    {
        var effect = ambient
            ? Effect.NewAmbient(type, level, duration)
            : Effect.New(type, level, duration);
        return hideParticles ? effect.WithoutParticles() : effect;
    }

    public static Effect.Value Create(Effect.LastingType type, int level, long ticks, bool hideParticles = false, bool ambient = false)
    {
        return Create(type, level, TimeApi.ConvertGameTicksToDuration(ticks), hideParticles, ambient);
    }

    public static Effect.Value CreateInfiniteEffect(Effect.LastingType type, int level, bool hideParticles = false)
    {
        var effect = Effect.NewInfinite(type, level);
        return hideParticles ? effect.WithoutParticles() : effect;
    }

    public static Effect.Value CreateInstantEffect(Effect.Type type, int level, double potency = 1)
    {
        return Effect.NewInstantWithPotency(type, level, potency);
    }

    public static Effect.Value HideEffectParticles(Effect.Value effect)
    {
        return effect.WithoutParticles();
    }

    public static bool HasParticlesHidden(Effect.Value effect)
    {
        return effect.ParticlesHidden();
    }

    public static int GetLevel(Effect.Value effect)
    {
        return effect.Level();
    }

    public static TimeSpan GetDuration(Effect.Value effect)
    {
        return effect.Duration();
    }

    public static bool IsAmbient(Effect.Value effect)
    {
        return effect.Ambient();
    }

    public static bool IsInfinite(Effect.Value effect)
    {
        return effect.Infinite();
    }

    public static int GetEffectTick(Effect.Value effect)
    {
        return effect.Tick();
    }

    public static Effect.Type? GetEffectType(Effect.Value effect)
    {
        return effect.Type();
    }

    public static (Effect.Type? Type, bool Ok) GetEffectTypeById(int id)
    {
        return Effect.ByID(id);
    }

    public static Effect.Type RequireEffectTypeById(int id)
    {
        var (type, ok) = Effect.ByID(id);
        if (!ok || type is null)
        {
            throw new ArgumentException($"Unknown effect id: {id}", nameof(id));
        }

        return type;
    }

    public static (int Id, bool Ok) GetEffectTypeId(Effect.Type type)
    {
        return Effect.ID(type);
    }

    public static Color.RGBA GetColor(Effect.Type type)
    {
        return type.RGBA();
    }

    public static (Color.RGBA Color, bool Ambient) GetResultingColor(IReadOnlyList<Effect.Value> effects)
    {
        return Effect.ResultingColour(effects);
    }

    public static void AddEffect(Player player, Effect.Value effect)
    {
        player.AddEffect(effect);
    }

    public static void RemoveEffect(Player player, Effect.Type effect)
    {
        player.RemoveEffect(effect);
    }

    public static (Effect.Value Effect, bool Ok) GetEffect(Player player, Effect.Type effect)
    {
        return player.Effect(effect);
    }

    public static bool HasEffect(Player player, Effect.Type effect)
    {
        return player.Effect(effect).Ok;
    }

    public static IReadOnlyList<Effect.Value> GetEffects(Player player)
    {
        return player.Effects();
    }

    public static TimeSpan ConvertTicksToDuration(long ticks)
    {
        return TimeApi.ConvertGameTicksToDuration(ticks);
    }
}
