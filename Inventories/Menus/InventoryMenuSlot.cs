using Dragonfly;

namespace Toolbox.Inventories.Menus;

public sealed class InventoryMenuSlot
{
    public InventoryMenuSlot(int index, Item.Stack item, object? value = null)
    {
        Index = index;
        Item = item;
        Value = value;
    }

    public int Index { get; }
    public Item.Stack Item { get; }
    public object? Value { get; }

    public InventoryMenuSlot OnClick(Action<InventoryMenuClickContext> onClick)
    {
        ArgumentNullException.ThrowIfNull(onClick);
        ClickHandler = onClick;
        return this;
    }

    public T? GetValue<T>() => Value is T value ? value : default;

    internal Action<InventoryMenuClickContext>? ClickHandler { get; private set; }

    internal InventoryMenuSlot Snapshot() => new(Index, Item, Value) { ClickHandler = ClickHandler };
}
