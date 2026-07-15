using Dragonfly;
using Toolbox.Events.Player.Base;

namespace Toolbox.Events.Player.Commands;

public sealed class PlayerCommandExecutionEvent(Dragonfly.Player player, Cmd.Command command, string[] arguments) : CancellablePlayerEvent(player)
{
    public Cmd.Command Command { get; } = command;

    public string[] Arguments { get; } = arguments;
}
