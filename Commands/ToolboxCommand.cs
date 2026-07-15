using Dragonfly;
using Toolbox.Diagnostics;

namespace Toolbox.Commands;

public abstract class ToolboxCommand : Cmd.Runnable
{
    public void Run(Cmd.Source src, Cmd.Output output, World.Tx? tx)
    {
        try
        {
            Execute(new CommandContext(src, output, tx));
        }
        catch (Exception exception)
        {
            ToolboxLogger.Error(exception, $"Command {GetType().Name}");
            output.Error("An internal command error occurred. Check the Toolbox logs.");
        }
    }

    protected abstract void Execute(CommandContext ctx);
}
