using Dragonfly;

namespace Toolbox.Items;

public static class ItemFactory
{
    public static Item.Stack CreateAir()
    {
        return default;
    }

    private static Item.Stack CreateStack(World.Item item, int count = 1)
    {
        return Item.NewStack(item, count);
    }

    public static ItemStackBuilder CreateBuilder(World.Item item, int count = 1)
    {
        return new ItemStackBuilder(CreateStack(item, count));
    }

    public static Item.Stack CreateDiamondSword(int count = 1)
    {
        return CreateStack(new Item.Sword(Item.ToolTierDiamond), count);
    }

    public static Item.Stack CreateNetheriteSword(int count = 1)
    {
        return CreateStack(new Item.Sword(Item.ToolTierNetherite), count);
    }

    public static Item.Stack CreateDiamondPickaxe(int count = 1)
    {
        return CreateStack(new Item.Pickaxe(Item.ToolTierDiamond), count);
    }

    public static Item.Stack CreateNetheritePickaxe(int count = 1)
    {
        return CreateStack(new Item.Pickaxe(Item.ToolTierNetherite), count);
    }

    public static Item.Stack CreateTotem(int count = 1)
    {
        return CreateStack(new Item.Totem(), count);
    }

    public static ArmorSet CreateDiamondArmor()
    {
        var tier = new Item.ArmourTierDiamond();
        return new ArmorSet(
            CreateStack(new Item.Helmet(tier)),
            CreateStack(new Item.Chestplate(tier)),
            CreateStack(new Item.Leggings(tier)),
            CreateStack(new Item.Boots(tier)));
    }

    public static ArmorSet CreateNetheriteArmor()
    {
        var tier = new Item.ArmourTierNetherite();
        return new ArmorSet(
            CreateStack(new Item.Helmet(tier)),
            CreateStack(new Item.Chestplate(tier)),
            CreateStack(new Item.Leggings(tier)),
            CreateStack(new Item.Boots(tier)));
    }
}
