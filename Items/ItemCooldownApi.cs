using Dragonfly;
using Toolbox.Timing;

namespace Toolbox.Items;

public static class ItemCooldownApi
{
    private static bool HasCooldown(Player player, World.Item? item)
    {
        return player.HasCooldown(item);
    }

    public static bool HasCooldown(Player player, Item.Stack item)
    {
        return HasCooldown(player, item.Item());
    }

    private static void SetCooldown(Player player, World.Item? item, TimeSpan duration)
    {
        player.SetCooldown(item, duration);
    }

    public static void SetCooldown(Player player, Item.Stack item, TimeSpan duration)
    {
        SetCooldown(player, item.Item(), duration);
    }

    public static void SetCooldownTicks(Player player, World.Item? item, long ticks)
    {
        SetCooldown(player, item, TimeApi.ConvertGameTicksToDuration(ticks));
    }

    public static void ClearCooldown(Player player, World.Item? item)
    {
        SetCooldown(player, item, TimeSpan.Zero);
    }
}
