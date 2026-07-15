using Dragonfly;

namespace Toolbox.Commands.Callbacks;

internal sealed class PlayerCallbackCommand(Action<CommandContext, Player> executor) : PlayerToolboxCommand
{
    protected override void Execute(CommandContext ctx, Player player)
    {
        executor(ctx, player);
    }
}
