using Dragonfly;

namespace Toolbox.Inventories.Menus;

public sealed class InventoryMenuSlot(int index, Item.Stack item, object? value = null)
{

    public int Index { get; } = index;
    public Item.Stack Item { get; } = item;
    public object? Value { get; } = value;

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
