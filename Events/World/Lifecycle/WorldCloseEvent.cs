using Toolbox.Events.World.Base;

namespace Toolbox.Events.World.Lifecycle;

public sealed class WorldCloseEvent(Dragonfly.World world) : WorldEvent(world);
