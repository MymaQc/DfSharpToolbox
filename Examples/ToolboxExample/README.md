# ToolboxExample

Plugin d'exemple pour tester rapidement les API de Toolbox.

Commandes utiles:

- `/tbxhelp`: liste les commandes.
- `/tbxprofile`: affiche les informations joueur.
- `/tbxitems`: teste items, NBT, inventaire, armure et cooldown.
- `/tbxcustomitem`: donne huit exemplaires du véritable item `toolbox:ruby`.
- `/tbxcustomblock`: donne trois blocs personnalisés plaçables (solide, lumineux et demi-hauteur).
- `/tbxforms`: ouvre une simple form, une modal et une custom form.
- `/tbxinv`: ouvre un faux inventaire interactif avec callbacks, valeurs et mise a jour.
- `/tbxui`: teste title, toast, scoreboard, HUD et input locks.
- `/tbxworld`: teste monde, blocks, entites et dimensions.
- `/tbxentity`: cree une entite custom dont le nom est actualise par son tick.
- `/tbxtask`: lance des tasks immediate, delayed et repeating.
- `/tbxpacketlog`: active/desactive l'inspection des packets dans la console.
- `/tbxkit <count> [name]`: exemple de commande avec parametres.
- `/tbxeffect <speed|jumpboost|nightvision> [seconds]`: autre commande avec parametres.
- `/tbxecho <message...>`: exemple de `Cmd.Varargs`.

Logs:

- Toolbox ecrit les erreurs dans la console du serveur.
- Toolbox ecrit aussi les erreurs dans `logs/toolbox-errors.log`, depuis le dossier ou le serveur est lance.
- Les forms, commandes, events et tasks sont proteges pour eviter qu'une exception C# disparaisse sans trace.

## Item personnalise

Les items doivent être enregistrés dans le constructeur du plugin. Dragonfly construit le registre et le resource pack avant `OnEnable()`.
La génération automatique du resource pack doit rester activée dans la configuration Dragonfly du serveur.

```csharp
private readonly Item.Custom _rubySword;

public MyPlugin()
{
    _rubySword = CustomItemApi.CreateEmbedded<MyPlugin>(
            "myplugin:ruby_sword",
            "Epee en rubis",
            "MyPlugin.Assets.ruby_sword.png")
        .Category(Item.CustomItemCategory.Equipment)
        .MaxStackSize(1)
        .AttackDamage(8)
        .Durability(850)
        .HandEquipped()
        .Glinted()
        .Enchantable(CustomItemEnchantSlot.Sword, 18)
        .Cooldown(TimeSpan.FromMilliseconds(500))
        .Register();
}

public override void OnEnable()
{
    var stack = CustomItemApi.CreateBuilder(_rubySword)
        .SetLore("Un vrai nouvel item")
        .Build();
}
```

Le builder fournit aussi `Food`, `Armor`, `Wearable`, `Fuel`, `FireResistant`,
`Projectile`, `Throwable`, `Compostable`, `Record`, `EntityPlacer`, `BlockPlacer`,
`Tags`, `UseModifiers`, `SwingDuration`, `BundleInteraction`, `Storage`, `Dyeable`,
`DamageAbsorption`, `SwingSounds`, `PiercingWeapon` et `KineticWeapon`. Pour un composant Bedrock plus recent,
`AddComponent(name, values)` et `SetProperty(name, value)` permettent de fournir
directement sa structure data-driven.

La texture doit être ajoutée au projet comme ressource embarquée:

```xml
<ItemGroup>
    <EmbeddedResource Include="Assets\ruby.png" />
</ItemGroup>
```

## Bloc personnalise

Les blocs doivent eux aussi etre enregistres dans le constructeur. Le meme objet
`Block.Custom` sert de bloc de monde et d'item placable.

```csharp
private readonly Block.Custom _rubyLamp;

public MyPlugin()
{
    _rubyLamp = CustomBlockApi.CreateEmbedded<MyPlugin>(
            "myplugin:ruby_lamp",
            "Lampe de rubis",
            "MyPlugin.Assets.ruby.png")
        .Category(Item.CustomItemCategory.Construction)
        .Hardness(2, blastResistance: 12)
        .LightEmission(15)
        .LightDampening(0)
        .RenderMethod(Block.CustomBlockRenderMethod.Blend)
        .CollisionBox(new CustomBlockBox(0, 0, 0, 1, .75f, 1))
        .SelectionBox(new CustomBlockBox(0, 0, 0, 1, .75f, 1))
        .MapColor("#ff405f")
        .Register();
}

public override void OnEnable()
{
    var stack = CustomBlockApi.CreateBuilder(_rubyLamp, 16)
        .SetLore("Nouveau bloc placable", "Lumiere: 15")
        .Build();
}
```

Le builder expose uniquement les capacites que Dragonfly transforme nativement
en composants de bloc:

| Capacite Dragonfly | Methode principale |
| --- | --- |
| collision et selection | `CollisionBox(...)`, `SelectionBox(...)` |
| durete et resistance aux explosions | `Hardness(...)`, `BlastResistance(...)` |
| friction et inflammabilite | `Friction(...)`, `Flammable(...)` |
| geometrie | `Geometry(...)` |
| lumiere | `LightEmission(...)`, `LightDampening(...)` |
| materiau et faces texturees | `Material(...)`, `TextureFaces(...)` |
| couleur de carte | `MapColor(...)` |
| transformation | `Scale(...)`, `Translation(...)`, `Rotation(...)` |
| etats et permutations | `State(...)`, `Permutation(...)` |
| comportement serveur | `Replaceable(...)`, `PlacementFilter(...)`, `LiquidDisplacement(...)` |

Les composants Bedrock libres ne sont volontairement pas exposes: Dragonfly ne les
prend pas en charge dans son API de blocs et les injecter manuellement rendait le
registre envoye au client instable.

`Friction(...)` utilise l'echelle native Dragonfly: `0.6` correspond a un bloc
normal et une valeur proche de `0.98` a un bloc glissant. La librairie convertit
automatiquement cette valeur vers l'echelle inverse utilisee par Bedrock.

### Etats, permutations et callbacks

```csharp
var lamp = CustomBlockApi.Create("myplugin:lamp", "Lampe", texturePng)
    .State("myplugin:active", false, true)
    .Permutation(
        "q.block_state('myplugin:active') == true",
        permutation => permutation
            .Scale(.8f, .8f, .8f)
            .MapColor("#ff405f"))
    .OnInteract(context =>
    {
        if (context.Transaction.Block(context.Position) is not Block.Custom current)
            return;

        var active = current.Properties.TryGetValue("myplugin:active", out var value) && value is true;
        context.Transaction.SetBlock(context.Position, current.WithState("myplugin:active", !active));
        context.Cancel();
    })
    .OnPlayerPlacing(context => context.Player.Message("Lampe placee"))
    .Register();
```

Les callbacks sont executes cote serveur par Toolbox. Les valeurs d'etat sont
enregistrees comme de vrais variants Dragonfly et `WithState(...)` permet de passer
d'un variant a l'autre. Le produit cartesien est limite a 4096 variants par bloc.

## Entite personnalisee

Les types d'entites doivent etre definis et enregistres dans le constructeur du
plugin. L'identifiant reseau choisit l'entite vanilla affichee par le client.

```csharp
private readonly CustomEntityType<MyPlugin> _marker = EntityApi.Define<MyPlugin>(
        "myplugin:marker",
        "minecraft:armor_stand")
    .DefaultProperty("seconds_alive", 0)
    .BoundingBox(Cube.Box(-0.3, 0, -0.3, 0.3, 1.8, 0.3))
    .TickEvery(20, tick =>
    {
        var seconds = tick.Properties.Get("seconds_alive", 0) + 1;
        tick.Properties.Set("seconds_alive", seconds);
        tick.NameTag = $"Actif depuis {seconds}s";
    })
    .Register();
```

Une entite peut ensuite etre creee depuis une transaction world:

```csharp
var spawned = EntityApi.Spawn(_marker, transaction)
    .At(position)
    .Rotated(rotation)
    .Moving(new Vector3(0, 0.1, 0))
    .NameTag("Mon entite")
    .Property("owner", PlayerApi.GetName(player))
    .Create();

spawned.Properties.Set("custom_value", 42);
spawned.Despawn(transaction);
```
