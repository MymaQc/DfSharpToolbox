namespace Toolbox.Commands.Callbacks;

internal sealed class CallbackCommand(Action<CommandContext> executor) : ToolboxCommand
{
    protected override void Execute(CommandContext ctx)
    {
        executor(ctx);
    }
}
