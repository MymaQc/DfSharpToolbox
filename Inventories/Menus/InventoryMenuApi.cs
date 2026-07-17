using Dragonfly;

namespace Toolbox.Inventories.Menus;

public enum InventoryMenuType
{
    Chest,
    DoubleChest,
    Hopper,
    Dropper,
    Barrel,
    EnderChest,
}

public static class InventoryMenuApi
{
    public static InventoryMenu Create(string title, InventoryMenuType type = InventoryMenuType.Chest) =>
        new(title, type);

    public static void Send(Player player, InventoryMenu menu)
    {
        ArgumentNullException.ThrowIfNull(player);
        ArgumentNullException.ThrowIfNull(menu);
        player.SendContainerMenu(menu.Snapshot());
    }

    public static void Update(Player player, InventoryMenu menu)
    {
        ArgumentNullException.ThrowIfNull(player);
        ArgumentNullException.ThrowIfNull(menu);
        player.SendContainerMenu(menu.Snapshot(), update: true);
    }

    public static void Close(Player player)
    {
        ArgumentNullException.ThrowIfNull(player);
        player.CloseContainerMenu();
    }

    public static int GetSize(InventoryMenuType type) => ContainerMenu.Size(ToContainerType(type));

    internal static ContainerMenu.Type ToContainerType(InventoryMenuType type) => type switch
    {
        InventoryMenuType.Chest => ContainerMenu.Type.Chest,
        InventoryMenuType.DoubleChest => ContainerMenu.Type.DoubleChest,
        InventoryMenuType.Hopper => ContainerMenu.Type.Hopper,
        InventoryMenuType.Dropper => ContainerMenu.Type.Dropper,
        InventoryMenuType.Barrel => ContainerMenu.Type.Barrel,
        InventoryMenuType.EnderChest => ContainerMenu.Type.EnderChest,
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown inventory menu type."),
    };
}
