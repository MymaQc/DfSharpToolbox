using Dragonfly;

namespace Toolbox.Items;

public sealed class ItemStackBuilder(Item.Stack stack)
{
    private Item.Stack _stack = stack;

    public ItemStackBuilder SetCustomName(params object?[] name)
    {
        _stack = _stack.WithCustomName(name);
        return this;
    }

    public ItemStackBuilder SetLore(params string[] lore)
    {
        _stack = _stack.WithLore(lore);
        return this;
    }

    public ItemStackBuilder AddEnchantment(Item.EnchantmentType type, int level, bool force = false)
    {
        var enchantment = Item.NewEnchantment(type, level);
        _stack = force ? _stack.WithForcedEnchantments(enchantment) : _stack.WithEnchantments(enchantment);
        return this;
    }

    public ItemStackBuilder SetUnbreakable(bool unbreakable = true)
    {
        _stack = unbreakable ? _stack.AsUnbreakable() : _stack.AsBreakable();
        return this;
    }

    public ItemStackBuilder SetDurability(int durability)
    {
        _stack = _stack.WithDurability(durability);
        return this;
    }

    public ItemStackBuilder SetNamedTag(string key, object? value)
    {
        _stack = _stack.WithValue(key, value);
        return this;
    }

    public ItemStackBuilder SetTag(string key, object value)
    {
        _stack = ItemNbtApi.SetTag(_stack, key, value);
        return this;
    }

    public ItemStackBuilder RemoveTag(string key)
    {
        _stack = ItemNbtApi.RemoveTag(_stack, key);
        return this;
    }

    public Item.Stack Build()
    {
        return _stack;
    }
}
