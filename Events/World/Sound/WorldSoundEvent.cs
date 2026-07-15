using Dragonfly;
using Toolbox.Events.World.Base;

namespace Toolbox.Events.World.Sound;

public sealed class WorldSoundEvent(Dragonfly.World world, Dragonfly.World.Sound sound, Vector3 position) : CancellableWorldEvent(world)
{
    public Dragonfly.World.Sound Sound { get; } = sound;

    public Vector3 Position { get; } = position;
}
