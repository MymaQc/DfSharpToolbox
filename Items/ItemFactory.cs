using Dragonfly;

namespace Toolbox.Items;

public static class ItemFactory
{

    public static Item.Stack Air()
    {
        return default;
    }

    public static Item.Stack Create(World.Item item, int count = 1)
    {
        return Item.NewStack(item, count);
    }

    public static ItemStackBuilder Builder(World.Item item, int count = 1)
    {
        return new ItemStackBuilder(Create(item, count));
    }

    public static Item.Stack DiamondSword(int count = 1)
    {
        return Create(new Item.Sword(Item.ToolTierDiamond), count);
    }

    public static Item.Stack NetheriteSword(int count = 1)
    {
        return Create(new Item.Sword(Item.ToolTierNetherite), count);
    }

    public static Item.Stack DiamondPickaxe(int count = 1)
    {
        return Create(new Item.Pickaxe(Item.ToolTierDiamond), count);
    }

    public static Item.Stack NetheritePickaxe(int count = 1)
    {
        return Create(new Item.Pickaxe(Item.ToolTierNetherite), count);
    }

    public static Item.Stack Totem(int count = 1)
    {
        return Create(new Item.Totem(), count);
    }

    public static ArmorSet DiamondArmor()
    {
        var tier = new Item.ArmourTierDiamond();
        return new ArmorSet(Create(new Item.Helmet(tier)), Create(new Item.Chestplate(tier)), Create(new Item.Leggings(tier)), Create(new Item.Boots(tier)));
    }

    public static ArmorSet NetheriteArmor()
    {
        var tier = new Item.ArmourTierNetherite();
        return new ArmorSet(Create(new Item.Helmet(tier)), Create(new Item.Chestplate(tier)), Create(new Item.Leggings(tier)), Create(new Item.Boots(tier)));
    }
}
