namespace Toolbox.Commands.Callbacks;

internal sealed class BuilderCommand(Action<CommandContext> executor) : ToolboxCommand
{
    protected override void Execute(CommandContext ctx)
    {
        executor(ctx);
    }
}
