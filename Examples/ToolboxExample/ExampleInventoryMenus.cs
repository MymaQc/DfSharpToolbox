using Dragonfly;
using Toolbox.Inventories;
using Toolbox.Inventories.Menus;
using Toolbox.Items;
using Toolbox.Players;

namespace ToolboxExample;

internal static class ExampleInventoryMenus
{
    public static void Open(Player player)
    {
        var clickCount = 0;
        InventoryMenu menu = null!;

        void RefreshCounter()
        {
            var counter = ItemFactory.CreateBuilder(new Item.Emerald(), Math.Clamp(clickCount, 1, 64))
                .SetCustomName($"Clics: {clickCount}")
                .SetLore("Clique pour tester Update().")
                .Build();
            menu.SetItem(13, counter, click =>
            {
                clickCount++;
                RefreshCounter();
                click.Update();
            });
        }

        var apple = ItemFactory.CreateBuilder(new Item.Apple())
            .SetCustomName("Recevoir une pomme")
            .Build();
        var containers = ItemFactory.CreateBuilder(new Item.Compass())
            .SetCustomName("Tester les conteneurs")
            .SetLore("Hopper, dropper, barrel, double chest et ender chest")
            .Build();
        var close = ItemFactory.CreateBuilder(new Item.RedstoneWire())
            .SetCustomName("Fermer")
            .Build();

        menu = InventoryMenuApi.Create("Toolbox Inventory", InventoryMenuType.Chest)
            .SetItem(10, apple, click =>
            {
                var reward = ItemFactory.CreateBuilder(new Item.Apple())
                    .SetCustomName("Pomme du faux inventaire")
                    .Build();
                InventoryApi.GiveItem(click.Player, reward);
                PlayerApi.SendMessage(click.Player, "Pomme ajoutee depuis le slot 10.");
                click.Update();
            })
            .SetItem(16, containers, click => click.Open(CreateContainerSelector(menu)))
            .SetItem(22, close, click => click.Close())
            .OnClick(click => PlayerApi.SendTip(click.Player, $"Slot {click.Index}: {click.Item.CustomName()}"))
            .OnClose(closed => PlayerApi.SendTip(closed.Player, "Faux inventaire ferme."));

        RefreshCounter();
        menu.Send(player);
    }

    private static InventoryMenu CreateContainerSelector(InventoryMenu parent)
    {
        (InventoryMenuType Type, World.Item Item)[] choices =
        {
            (InventoryMenuType.Hopper, new Item.Diamond()),
            (InventoryMenuType.Dropper, new Item.Stick()),
            (InventoryMenuType.Barrel, new Item.Book()),
            (InventoryMenuType.DoubleChest, new Item.GoldenApple()),
            (InventoryMenuType.EnderChest, new Item.EnderPearl()),
        };
        var menu = InventoryMenuApi.Create("Types de conteneurs", InventoryMenuType.Chest);
        var indices = new[] { 9, 11, 13, 15, 17 };
        for (var index = 0; index < choices.Length; index++)
        {
            var choice = choices[index];
            var icon = ItemFactory.CreateBuilder(choice.Item)
                .SetCustomName(choice.Type.ToString())
                .Build();
            menu.SetItem(indices[index], icon, click => click.Open(CreateContainerDemo(choice.Type, parent)));
        }
        return menu.SetItem(22, ItemFactory.CreateBuilder(new Item.Compass()).SetCustomName("Retour").Build(), click => click.Open(parent));
    }

    private static InventoryMenu CreateContainerDemo(InventoryMenuType type, InventoryMenu parent)
    {
        var menu = InventoryMenuApi.Create($"Test {type}", type);
        var center = menu.SlotCount / 2;
        return menu
            .SetItem(center, ItemFactory.CreateBuilder(new Item.Emerald()).SetCustomName($"Conteneur {type}").Build(),
                click => PlayerApi.SendMessage(click.Player, $"Clic recu dans {type}, slot {click.Index}."))
            .SetItem(menu.SlotCount - 1, ItemFactory.CreateBuilder(new Item.Compass()).SetCustomName("Retour").Build(),
                click => click.Open(parent));
    }
}
