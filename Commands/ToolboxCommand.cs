using Dragonfly;

namespace Toolbox.Commands;

public abstract class ToolboxCommand : Cmd.Runnable
{
    public void Run(Cmd.Source src, Cmd.Output output, World.Tx? tx)
    {
        Execute(new CommandContext(src, output, tx));
    }

    protected abstract void Execute(CommandContext ctx);
}
