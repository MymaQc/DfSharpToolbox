using Dragonfly;

namespace Toolbox.Inventories.Menus;

public sealed class InventoryMenuClickContext
{
    internal InventoryMenuClickContext(
        Player player,
        World.Tx transaction,
        InventoryMenu menu,
        InventoryMenuSlot slot,
        Item.Stack item)
    {
        Player = player;
        Transaction = transaction;
        Menu = menu;
        Slot = slot;
        Item = item;
    }

    public Player Player { get; }
    public World.Tx Transaction { get; }
    public InventoryMenu Menu { get; }
    public InventoryMenuSlot Slot { get; }
    public Item.Stack Item { get; }
    public int Index => Slot.Index;

    public T? GetValue<T>() => Slot.GetValue<T>();

    public void Close() => InventoryMenuApi.Close(Player);

    public void Update() => InventoryMenuApi.Update(Player, Menu);

    public void Open(InventoryMenu menu)
    {
        ArgumentNullException.ThrowIfNull(menu);
        InventoryMenuApi.Send(Player, menu);
    }
}

public sealed class InventoryMenuCloseContext
{
    internal InventoryMenuCloseContext(Player player, World.Tx transaction, InventoryMenu menu)
    {
        Player = player;
        Transaction = transaction;
        Menu = menu;
    }

    public Player Player { get; }
    public World.Tx Transaction { get; }
    public InventoryMenu Menu { get; }
}
