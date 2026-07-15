using Dragonfly;
using Toolbox.Players.Controls;

namespace Toolbox.Players;

public static class PlayerControlApi
{
    private static IReadOnlyList<PlayerHudElement> HudElements { get; } = Enum.GetValues<PlayerHudElement>();

    public static IReadOnlyList<PlayerInputLock> InputLocks { get; } = Enum.GetValues<PlayerInputLock>();

    private static Hud.Element ToDragonfly(PlayerHudElement element)
    {
        return element switch
        {
            PlayerHudElement.PaperDoll => Hud.PaperDoll(),
            PlayerHudElement.Armor => Hud.Armour(),
            PlayerHudElement.ToolTips => Hud.ToolTips(),
            PlayerHudElement.TouchControls => Hud.TouchControls(),
            PlayerHudElement.Crosshair => Hud.Crosshair(),
            PlayerHudElement.HotBar => Hud.HotBar(),
            PlayerHudElement.Health => Hud.Health(),
            PlayerHudElement.ProgressBar => Hud.ProgressBar(),
            PlayerHudElement.Hunger => Hud.Hunger(),
            PlayerHudElement.AirBubbles => Hud.AirBubbles(),
            PlayerHudElement.HorseHealth => Hud.HorseHealth(),
            PlayerHudElement.StatusEffects => Hud.StatusEffects(),
            PlayerHudElement.ItemText => Hud.ItemText(),
            _ => throw new ArgumentOutOfRangeException(nameof(element), element, null),
        };
    }

    private static Input.Lock ToDragonfly(PlayerInputLock input)
    {
        return input switch
        {
            PlayerInputLock.Camera => Input.Camera(),
            PlayerInputLock.Movement => Input.Movement(),
            PlayerInputLock.LateralMovement => Input.LateralMovement(),
            PlayerInputLock.Sneak => Input.Sneak(),
            PlayerInputLock.Jump => Input.Jump(),
            PlayerInputLock.Mount => Input.Mount(),
            PlayerInputLock.Dismount => Input.Dismount(),
            PlayerInputLock.MoveForward => Input.MoveForward(),
            PlayerInputLock.MoveBackward => Input.MoveBackward(),
            PlayerInputLock.MoveLeft => Input.MoveLeft(),
            PlayerInputLock.MoveRight => Input.MoveRight(),
            _ => throw new ArgumentOutOfRangeException(nameof(input), input, null),
        };
    }

    public static void ShowHud(Player player, PlayerHudElement element)
    {
        ShowHud(player, ToDragonfly(element));
    }

    private static void ShowHud(Player player, Hud.Element element)
    {
        player.ShowHudElement(element);
    }

    public static void HideHud(Player player, PlayerHudElement element)
    {
        HideHud(player, ToDragonfly(element));
    }

    private static void HideHud(Player player, Hud.Element element)
    {
        player.HideHudElement(element);
    }

    public static bool IsHudHidden(Player player, PlayerHudElement element)
    {
        return IsHudHidden(player, ToDragonfly(element));
    }

    private static bool IsHudHidden(Player player, Hud.Element element)
    {
        return player.HudElementHidden(element);
    }

    public static void ShowAllHud(Player player)
    {
        foreach (var element in HudElements)
        {
            ShowHud(player, element);
        }
    }

    public static void HideAllHud(Player player)
    {
        foreach (var element in HudElements)
        {
            HideHud(player, element);
        }
    }

    public static void LockInput(Player player, PlayerInputLock input)
    {
        LockInput(player, ToDragonfly(input));
    }

    private static void LockInput(Player player, Input.Lock input)
    {
        player.LockInput(input);
    }

    public static void UnlockInput(Player player, PlayerInputLock input)
    {
        UnlockInput(player, ToDragonfly(input));
    }

    private static void UnlockInput(Player player, Input.Lock input)
    {
        player.UnlockInput(input);
    }

    public static bool IsInputLocked(Player player, PlayerInputLock input)
    {
        return IsInputLocked(player, ToDragonfly(input));
    }

    private static bool IsInputLocked(Player player, Input.Lock input)
    {
        return player.InputLocked(input);
    }

    public static void ClearInputLocks(Player player)
    {
        player.ClearInputLocks();
    }

    public static void ShowCoordinates(Player player)
    {
        player.ShowCoordinates();
    }

    public static void HideCoordinates(Player player)
    {
        player.HideCoordinates();
    }

    public static void SendSleepingIndicator(Player player, int sleeping, int max)
    {
        player.SendSleepingIndicator(sleeping, max);
    }

    public static void CloseDialogue(Player player)
    {
        player.CloseDialogue();
    }

    public static void RemoveBossBar(Player player)
    {
        player.RemoveBossBar();
    }
}
