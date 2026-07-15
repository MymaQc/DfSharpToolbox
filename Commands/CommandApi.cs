using Dragonfly;
using Toolbox.Commands.Callbacks;

namespace Toolbox.Commands;

public static class CommandApi
{
    public static CommandBuilder CreateCommandBuilder(string name, string description)
    {
        return new CommandBuilder(name, description);
    }

    public static void RegisterCommand(string name, string description, Action<CommandContext> executor, params string[] aliases)
    {
        ArgumentNullException.ThrowIfNull(executor);
        RegisterCommand(name, description, new CallbackCommand(executor), aliases);
    }

    private static void RegisterCommand(string name, string description, Cmd.Runnable runnable, params string[] aliases)
    {
        ArgumentNullException.ThrowIfNull(runnable);
        RegisterCommand(Cmd.New(name, description, aliases, runnable));
    }

    public static void RegisterCommand<TCommand>(string name, string description, params string[] aliases)
        where TCommand : Cmd.Runnable, new()
    {
        RegisterCommand(name, description, new TCommand(), aliases);
    }

    public static void RegisterPlayerCommand(string name, string description, Action<CommandContext, Player> executor, params string[] aliases)
    {
        ArgumentNullException.ThrowIfNull(executor);
        RegisterCommand(name, description, new PlayerCallbackCommand(executor), aliases);
    }

    public static void RegisterPlayerCommand<TCommand>(string name, string description, params string[] aliases) where TCommand : PlayerToolboxCommand, new()
    {
        RegisterCommand(name, description, new TCommand(), aliases);
    }

    public static void RegisterCommand(Cmd.Command command)
    {
        Cmd.Register(command);
    }
}
