using Dragonfly;
using Toolbox.Events.Player.Base;

namespace Toolbox.Events.Player.Diagnostics;

public sealed class PlayerDiagnosticsEvent(Dragonfly.Player player, Session.Diagnostics diagnostics) : PlayerEvent(player)
{
    public Session.Diagnostics Diagnostics { get; } = diagnostics;
}
