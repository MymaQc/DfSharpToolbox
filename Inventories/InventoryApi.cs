using Dragonfly;
using Toolbox.Items;

namespace Toolbox.Inventories;

public static class InventoryApi
{
    public static Inventory.Value GetMainInventory(Player player)
    {
        return player.Inventory();
    }

    public static Inventory.Value GetEnderChestInventory(Player player)
    {
        return player.EnderChestInventory();
    }

    public static Inventory.Armour GetArmorInventory(Player player)
    {
        return player.Armour();
    }

    public static int GiveItem(Player player, Item.Stack item)
    {
        return player.Inventory().AddItem(item);
    }

    public static Item.Stack GetItem(Player player, int slot)
    {
        return player.Inventory().Item(slot);
    }

    public static void SetItem(Player player, int slot, Item.Stack item)
    {
        player.Inventory().SetItem(slot, item);
    }

    public static (Item.Stack MainHand, Item.Stack OffHand) GetHeldItems(Player player)
    {
        return player.HeldItems();
    }

    public static void SetHeldItems(Player player, Item.Stack mainHand, Item.Stack offHand)
    {
        player.SetHeldItems(mainHand, offHand);
    }

    public static void SetHeldSlot(Player player, int slot)
    {
        player.SetHeldSlot(slot);
    }

    public static int GetSize(Inventory.Value inventory)
    {
        return inventory.Size();
    }

    public static bool IsValidSlot(Inventory.Value inventory, int slot)
    {
        return slot >= 0 && slot < inventory.Size();
    }

    public static Item.Stack GetItem(Inventory.Value inventory, int slot)
    {
        return inventory.Item(slot);
    }

    public static void SetItem(Inventory.Value inventory, int slot, Item.Stack item)
    {
        inventory.SetItem(slot, item);
    }

    public static int AddItem(Inventory.Value inventory, Item.Stack item)
    {
        return inventory.AddItem(item);
    }

    public static IEnumerable<(int Slot, Item.Stack Item)> GetContents(Inventory.Value inventory)
    {
        for (var slot = 0; slot < inventory.Size(); slot++)
        {
            yield return (slot, inventory.Item(slot));
        }
    }

    public static void ClearInventorySlot(Inventory.Value inventory, int slot)
    {
        inventory.SetItem(slot, default);
    }

    public static void ClearInventory(Inventory.Value inventory)
    {
        for (var slot = 0; slot < inventory.Size(); slot++)
        {
            inventory.SetItem(slot, default);
        }
    }

    public static Item.Stack GetHelmet(Inventory.Armour armor)
    {
        return armor.Helmet();
    }

    public static Item.Stack GetChestplate(Inventory.Armour armor)
    {
        return armor.Chestplate();
    }

    public static Item.Stack GetLeggings(Inventory.Armour armor)
    {
        return armor.Leggings();
    }

    public static Item.Stack GetBoots(Inventory.Armour armor)
    {
        return armor.Boots();
    }

    public static ArmorSet GetArmorSet(Inventory.Armour armor)
    {
        return new ArmorSet(armor.Helmet(), armor.Chestplate(), armor.Leggings(), armor.Boots());
    }

    public static void SetHelmet(Inventory.Armour armor, Item.Stack helmet)
    {
        armor.SetHelmet(helmet);
    }

    public static void SetChestplate(Inventory.Armour armor, Item.Stack chestplate)
    {
        armor.SetChestplate(chestplate);
    }

    public static void SetLeggings(Inventory.Armour armor, Item.Stack leggings)
    {
        armor.SetLeggings(leggings);
    }

    public static void SetBoots(Inventory.Armour armor, Item.Stack boots)
    {
        armor.SetBoots(boots);
    }

    private static void SetArmor(Inventory.Armour armor, Item.Stack helmet, Item.Stack chestplate, Item.Stack leggings, Item.Stack boots)
    {
        armor.Set(helmet, chestplate, leggings, boots);
    }

    private static void SetArmor(Inventory.Armour armor, ArmorSet armorSet)
    {
        armor.Set(armorSet.Helmet, armorSet.Chestplate, armorSet.Leggings, armorSet.Boots);
    }

    public static void SetArmor(Player player, Item.Stack helmet, Item.Stack chestplate, Item.Stack leggings, Item.Stack boots)
    {
        SetArmor(player.Armour(), helmet, chestplate, leggings, boots);
    }

    public static void SetArmor(Player player, ArmorSet armorSet)
    {
        SetArmor(player.Armour(), armorSet);
    }
}
