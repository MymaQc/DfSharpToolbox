using Dragonfly;
using Toolbox.Timing;

namespace Toolbox.Presentation;

public static class TitleApi
{
    private static Title CreateTitle(params object?[] text)
    {
        return Title.New(text);
    }

    public static string GetText(Title title)
    {
        return title.Text();
    }

    public static Title CreateTitleWithText(params object?[] text)
    {
        return CreateTitle(text);
    }

    public static string GetSubtitle(Title title)
    {
        return title.Subtitle();
    }

    public static Title SetSubtitle(Title title, params object?[] text)
    {
        return title.WithSubtitle(text);
    }

    public static string GetActionText(Title title)
    {
        return title.ActionText();
    }

    public static Title SetActionText(Title title, params object?[] text)
    {
        return title.WithActionText(text);
    }

    public static TimeSpan GetDuration(Title title)
    {
        return title.Duration();
    }

    private static Title SetDuration(Title title, TimeSpan duration)
    {
        return title.WithDuration(duration);
    }

    public static Title SetDurationTicks(Title title, long ticks)
    {
        return SetDuration(title, Ticks(ticks));
    }

    public static TimeSpan GetFadeInDuration(Title title)
    {
        return title.FadeInDuration();
    }

    private static Title SetFadeInDuration(Title title, TimeSpan duration)
    {
        return title.WithFadeInDuration(duration);
    }

    public static Title SetFadeInDurationTicks(Title title, long ticks)
    {
        return SetFadeInDuration(title, Ticks(ticks));
    }

    public static TimeSpan GetFadeOutDuration(Title title)
    {
        return title.FadeOutDuration();
    }

    private static Title SetFadeOutDuration(Title title, TimeSpan duration)
    {
        return title.WithFadeOutDuration(duration);
    }

    public static Title SetFadeOutDurationTicks(Title title, long ticks)
    {
        return SetFadeOutDuration(title, Ticks(ticks));
    }

    public static void SendTitle(Player player, Title title)
    {
        player.SendTitle(title);
    }

    public static void SendTitle(Player player, params object?[] text)
    {
        SendTitle(player, CreateTitle(text));
    }

    public static void SendActionText(Player player, params object?[] text)
    {
        SendTitle(player, CreateTitle(string.Empty).WithActionText(text));
    }

    private static TimeSpan Ticks(long ticks)
    {
        return TimeApi.ConvertGameTicksToDuration(ticks);
    }
}
