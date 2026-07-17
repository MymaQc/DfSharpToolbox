# Inventory menus

`InventoryMenuApi` ouvre de vrais faux conteneurs Bedrock remplis avec des
`Item.Stack`. Aucun formulaire ni resource pack n'est utilise.

Types disponibles: `Chest`, `DoubleChest`, `Hopper`, `Dropper`, `Barrel` et
`EnderChest`.

## Callback par slot

```csharp
var apple = ItemFactory.CreateBuilder(new Item.Apple())
    .SetCustomName("Recevoir une pomme")
    .Build();

var menu = InventoryMenuApi.Create("Kits", InventoryMenuType.Chest)
    .SetItem(10, apple, click =>
    {
        InventoryApi.GiveItem(click.Player, Item.NewStack(new Item.Apple(), 1));
        click.Close();
    })
    .OnClose(closed => PlayerApi.SendTip(closed.Player, "Menu ferme."));

menu.Send(player);
```

## Valeurs et callback global

```csharp
var menu = InventoryMenuApi.Create("Choix", InventoryMenuType.Hopper)
    .SetItem(1, Item.NewStack(new Item.Diamond(), 1), value: "diamond")
    .SetItem(3, Item.NewStack(new Item.Emerald(), 1), value: "emerald")
    .OnClick(click =>
    {
        PlayerApi.SendMessage(click.Player, $"Choix: {click.GetValue<string>()}");
        click.Close();
    });
```

## Mise a jour en place

```csharp
var count = 0;
var menu = InventoryMenuApi.Create("Compteur", InventoryMenuType.Barrel);

void Refresh()
{
    var counter = ItemFactory.CreateBuilder(new Item.Emerald(), Math.Max(count, 1))
        .SetCustomName($"Clics: {count}")
        .Build();
    menu.SetItem(13, counter, click =>
    {
        count++;
        Refresh();
        click.Update();
    });
}

Refresh();
menu.Send(player);
```

`click.Open(otherMenu)` remplace le menu actuel et `click.Transaction` expose
la transaction Dragonfly du clic.
