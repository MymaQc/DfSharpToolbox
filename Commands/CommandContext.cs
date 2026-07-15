using Dragonfly;

namespace Toolbox.Commands;

public sealed class CommandContext
{

    internal CommandContext(Cmd.Source source, Cmd.Output output, World.Tx? transaction)
    {
        Sender = source;
        Output = output;
        Transaction = transaction;
    }

    private Cmd.Source Sender { get; }

    private Cmd.Output Output { get; }

    private World.Tx? Transaction { get; }

    private bool IsPlayer => Sender is Player;
    private Player? Player => Sender as Player;

    public Cmd.Source GetSender()
    {
        return Sender;
    }

    public Cmd.Output GetOutput()
    {
        return Output;
    }

    public World.Tx? GetTransaction()
    {
        return Transaction;
    }

    public bool HasPlayer()
    {
        return IsPlayer;
    }

    public Player? GetPlayer()
    {
        return Player;
    }

    public void SendMessage(params object?[] values)
    {
        Output.Print(values);
    }

    public void SendError(params object?[] values)
    {
        Output.Error(values);
    }

    public bool RequirePlayer(out Player player)
    {
        if (Sender is Player value)
        {
            player = value;
            return true;
        }

        Output.Error("This command can only be used in game.");
        player = null!;
        return false;
    }
}
