using Dragonfly;

namespace Toolbox.Presentation;

public static class ScoreboardApi
{
    public static Scoreboard CreateScoreboard(params object?[] name)
    {
        return Scoreboard.New(name);
    }

    public static string GetName(Scoreboard scoreboard)
    {
        return scoreboard.Name();
    }

    public static string[] GetLines(Scoreboard scoreboard)
    {
        return scoreboard.Lines();
    }

    public static bool IsDescending(Scoreboard scoreboard)
    {
        return scoreboard.Descending();
    }

    public static Scoreboard AddLine(this Scoreboard scoreboard, string text)
    {
        scoreboard.WriteString(text);
        return scoreboard;
    }

    public static Scoreboard SetLine(this Scoreboard scoreboard, int index, string text)
    {
        scoreboard.Set(index, text);
        return scoreboard;
    }

    public static Scoreboard RemoveLine(this Scoreboard scoreboard, int index)
    {
        scoreboard.Remove(index);
        return scoreboard;
    }

    public static Scoreboard RemovePadding(this Scoreboard scoreboard)
    {
        scoreboard.RemovePadding();
        return scoreboard;
    }

    public static Scoreboard SetDescending(this Scoreboard scoreboard)
    {
        scoreboard.SetDescending();
        return scoreboard;
    }

    public static void SendScoreboard(Player player, Scoreboard scoreboard)
    {
        player.SendScoreboard(scoreboard);
    }

    public static void RemoveScoreboard(Player player)
    {
        player.RemoveScoreboard();
    }
}
