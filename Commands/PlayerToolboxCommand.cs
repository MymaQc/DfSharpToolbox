using Dragonfly;

namespace Toolbox.Commands;

public abstract class PlayerToolboxCommand : ToolboxCommand, Cmd.Allower
{
    public virtual bool Allow(Cmd.Source source)
    {
        return source is Player;
    }

    protected sealed override void Execute(CommandContext ctx)
    {
        if (ctx.RequirePlayer(out var player))
        {
            Execute(ctx, player);
        }
    }

    protected abstract void Execute(CommandContext ctx, Player player);
}
