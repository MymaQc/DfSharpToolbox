using Dragonfly;
using Toolbox.Items;

namespace ToolboxExample;

internal static class ExampleCustomItems
{
    // A tiny valid PNG keeps the example self-contained. Real plugins should use RegisterEmbedded.
    private const string RubyTexture =
        "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mNk+A8AAQUBAScY42YAAAAASUVORK5CYII=";

    internal static ExampleCustomItemSet Register()
    {
        var texture = Convert.FromBase64String(RubyTexture);
        var sword = CustomItemApi.Create("toolbox:ruby_sword", "Epee en rubis", texture)
            .Category(Item.CustomItemCategory.Equipment)
            .MaxStackSize(1)
            .AttackDamage(8)
            .Durability(850)
            .HandEquipped()
            .Glinted()
            .Enchantable(CustomItemEnchantSlot.Sword, 18)
            .FireResistant()
            .Tags("toolbox:ruby", "toolbox:weapon")
            .Register();

        var apple = CustomItemApi.Create("toolbox:ruby_apple", "Pomme de rubis", texture)
            .MaxStackSize(16)
            .Food(nutrition: 6, saturationModifier: 7.2f, canAlwaysEat: true)
            .UseAnimation(CustomItemUseAnimation.Eat)
            .UseDuration(TimeSpan.FromSeconds(1.6))
            .Cooldown(TimeSpan.FromSeconds(1))
            .Compostable(1)
            .Register();

        var helmet = CustomItemApi.Create("toolbox:ruby_helmet", "Casque en rubis", texture)
            .Category(Item.CustomItemCategory.Equipment)
            .MaxStackSize(1)
            .Armor(4, CustomItemWearableSlot.Head, toughness: 3, knockbackResistance: 0.1f)
            .Durability(600)
            .Glinted()
            .Enchantable(CustomItemEnchantSlot.ArmorHead, 20)
            .Register();

        return new ExampleCustomItemSet(sword, apple, helmet);
    }
}

internal readonly record struct ExampleCustomItemSet(
    Item.Custom Sword,
    Item.Custom Apple,
    Item.Custom Helmet);
