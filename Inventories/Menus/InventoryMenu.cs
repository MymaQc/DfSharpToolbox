using Dragonfly;
using Toolbox.Forms;

namespace Toolbox.Inventories.Menus;

public sealed class InventoryMenu
{
    private readonly InventoryMenuSlot?[] _slots;
    private Action<InventoryMenuClickContext>? _onClick;
    private Action<InventoryMenuCloseContext>? _onClose;

    public InventoryMenu(string title, InventoryMenuType type = InventoryMenuType.Chest)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        Title = title;
        Type = type;
        _slots = new InventoryMenuSlot?[InventoryMenuApi.GetSize(type)];
    }

    public string Title { get; private set; }

    public InventoryMenuType Type { get; }

    public int SlotCount => _slots.Length;

    public InventoryMenu SetTitle(string title)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        Title = title;
        return this;
    }

    public InventoryMenu SetSlot(InventoryMenuSlot slot)
    {
        ArgumentNullException.ThrowIfNull(slot);
        ValidateIndex(slot.Index);
        _slots[slot.Index] = slot;
        return this;
    }

    public InventoryMenu SetItem(
        int index,
        Item.Stack item,
        Action<InventoryMenuClickContext>? onClick = null,
        object? value = null)
    {
        var slot = new InventoryMenuSlot(index, item, value);
        if (onClick is not null) slot.OnClick(onClick);
        return SetSlot(slot);
    }

    public InventoryMenu AddItem(
        Item.Stack item,
        Action<InventoryMenuClickContext>? onClick = null,
        object? value = null)
    {
        for (var index = 0; index < _slots.Length; index++)
        {
            if (_slots[index] is null) return SetItem(index, item, onClick, value);
        }
        throw new InvalidOperationException("The inventory menu has no empty slot left.");
    }

    public InventoryMenu SetItems(int startIndex, params Item.Stack[] items)
    {
        ArgumentNullException.ThrowIfNull(items);
        if (startIndex < 0 || startIndex > _slots.Length - items.Length)
            throw new ArgumentOutOfRangeException(nameof(startIndex));
        for (var index = 0; index < items.Length; index++) SetItem(startIndex + index, items[index]);
        return this;
    }

    public InventoryMenu ClearSlot(int index)
    {
        ValidateIndex(index);
        _slots[index] = null;
        return this;
    }

    public InventoryMenu Clear()
    {
        Array.Clear(_slots);
        return this;
    }

    public InventoryMenuSlot? GetSlot(int index)
    {
        ValidateIndex(index);
        return _slots[index];
    }

    public Item.Stack GetItem(int index) => GetSlot(index)?.Item ?? default;

    public InventoryMenu OnClick(Action<InventoryMenuClickContext> onClick)
    {
        ArgumentNullException.ThrowIfNull(onClick);
        _onClick = onClick;
        return this;
    }

    public InventoryMenu OnClose(Action<InventoryMenuCloseContext> onClose)
    {
        ArgumentNullException.ThrowIfNull(onClose);
        _onClose = onClose;
        return this;
    }

    public void Send(Player player) => InventoryMenuApi.Send(player, this);

    public void Update(Player player) => InventoryMenuApi.Update(player, this);

    internal InventoryMenuSnapshot Snapshot()
    {
        var slots = new InventoryMenuSlot?[_slots.Length];
        for (var index = 0; index < _slots.Length; index++) slots[index] = _slots[index]?.Snapshot();
        return new InventoryMenuSnapshot(this, Title, Type, slots, _onClick, _onClose);
    }

    private void ValidateIndex(int index)
    {
        if ((uint)index >= (uint)_slots.Length)
            throw new ArgumentOutOfRangeException(nameof(index), index, $"Slot index must be between 0 and {_slots.Length - 1}.");
    }
}

internal sealed class InventoryMenuSnapshot(
    InventoryMenu menu,
    string title,
    InventoryMenuType type,
    InventoryMenuSlot?[] slots,
    Action<InventoryMenuClickContext>? onClick,
    Action<InventoryMenuCloseContext>? onClose) : ContainerMenu.Value
{
    private readonly Item.Stack[] _items = slots.Select(slot => slot?.Item ?? default).ToArray();

    public string Title() => title;

    public ContainerMenu.Type ContainerType() => InventoryMenuApi.ToContainerType(type);

    public IReadOnlyList<Item.Stack> Items() => _items;

    public void Submit(Player player, int slot, Item.Stack item, World.Tx transaction)
    {
        if ((uint)slot >= (uint)slots.Length) return;
        var selected = slots[slot] ?? new InventoryMenuSlot(slot, item);
        var context = new InventoryMenuClickContext(player, transaction, menu, selected, item);
        Run(selected.ClickHandler, context);
        Run(onClick, context);
    }

    public void Close(Player player, World.Tx transaction) =>
        Run(onClose, new InventoryMenuCloseContext(player, transaction, menu));

    private static void Run<T>(Action<T>? callback, T context)
    {
        if (callback is null) return;
        try { callback(context); }
        catch (Exception exception) { FormCallbackRunner.Report(exception); }
    }
}
