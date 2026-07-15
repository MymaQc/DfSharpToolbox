using Dragonfly;
using Toolbox.Commands.Callbacks;

namespace Toolbox.Commands;

public sealed class CommandBuilder
{
    private readonly List<string> _aliases = [];
    private readonly string _description;
    private readonly string _name;
    private readonly List<Cmd.Runnable> _overloads = [];

    internal CommandBuilder(string name, string description)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        _name = name;
        _description = description;
    }

    public CommandBuilder AddAlias(params string[] aliases)
    {
        ArgumentNullException.ThrowIfNull(aliases);
        _aliases.AddRange(aliases);
        return this;
    }

    private CommandBuilder AddOverload(Cmd.Runnable runnable)
    {
        ArgumentNullException.ThrowIfNull(runnable);
        _overloads.Add(runnable);
        return this;
    }

    public CommandBuilder AddOverload<TCommand>() where TCommand : Cmd.Runnable, new()
    {
        return AddOverload(new TCommand());
    }

    public CommandBuilder OnExecute(Action<CommandContext> executor)
    {
        ArgumentNullException.ThrowIfNull(executor);
        return AddOverload(new BuilderCommand(executor));
    }

    public CommandBuilder OnPlayerExecute(Action<CommandContext, Player> executor)
    {
        ArgumentNullException.ThrowIfNull(executor);
        return AddOverload(new BuilderPlayerCommand(executor));
    }

    private Cmd.Command Build()
    {
        return _overloads.Count == 0 ? throw new InvalidOperationException($"Command '{_name}' needs at least one overload.")
            : Cmd.New(_name, _description, [.. _aliases], [.. _overloads]);
    }

    public void RegisterCommand()
    {
        CommandApi.RegisterCommand(Build());
    }
}
