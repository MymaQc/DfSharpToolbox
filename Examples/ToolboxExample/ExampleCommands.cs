using Dragonfly;
using Toolbox;
using Toolbox.Blocks;
using Toolbox.Commands;
using Toolbox.Effects;
using Toolbox.Entities;
using Toolbox.Forms;
using Toolbox.Inventories;
using Toolbox.Items;
using Toolbox.Math;
using Toolbox.Players;
using Toolbox.Players.Controls;
using Toolbox.Presentation;
using Toolbox.Servers;
using Toolbox.Sounds;
using Toolbox.Tasks;
using Toolbox.Timing;
using Toolbox.Worlds;
using Toolbox.Worlds.States;
// ReSharper disable UnassignedField.Global

namespace ToolboxExample;

internal static class ExampleCommands
{
    public static void Register(ToolboxPlugin plugin, ExampleState state, ExampleCustomItemSet? customItems, ExampleCustomBlockSet? customBlocks)
    {
        ToolboxKitCommand.State = state;
        ToolboxEffectCommand.State = state;

        CommandApi.RegisterCommand("tbxhelp", "Affiche les commandes de test Toolbox.", ShowHelp, "tbx");
        CommandApi.RegisterPlayerCommand("tbxprofile", "Teste PlayerApi et les snapshots joueur.", ShowProfile);
        CommandApi.RegisterPlayerCommand("tbxitems", "Teste ItemFactory, ItemNbtApi, InventoryApi et ItemCooldownApi.", (_, player) => GiveItems(player));
        if (customItems is { } items)
        {
            CommandApi.RegisterPlayerCommand("tbxcustomitem", "Donne des items personnalises avec composants.", (_, player) => GiveCustomItems(player, items));
        }
        if (customBlocks is { } blocks)
        {
            CommandApi.RegisterPlayerCommand("tbxcustomblock", "Donne trois blocs personnalises placables.", (_, player) => GiveCustomBlocks(player, blocks));
        }
        CommandApi.RegisterPlayerCommand("tbxforms", "Ouvre les forms Toolbox.", (_, player) => ExampleForms.OpenMain(player, state));
        CommandApi.RegisterPlayerCommand("tbxinv", "Ouvre un faux inventaire interactif.", (_, player) => ExampleInventoryMenus.Open(player));
        CommandApi.RegisterPlayerCommand("tbxui", "Teste TitleApi, ToastApi, ScoreboardApi et PlayerControlApi.", (_, player) => ShowUi(player));
        CommandApi.RegisterPlayerCommand("tbxworld", "Teste WorldApi, BlockApi, DimensionApi et EntityApi.", World);
        CommandApi.RegisterPlayerCommand("tbxtask", "Teste TaskApi.", (ctx, player) => Tasks(ctx, player, state));
        CommandApi.RegisterPlayerCommand("tbxpacketlog", "Active ou desactive le log des packets.", (_, player) => TogglePacketLog(player, state));
        CommandApi.RegisterPlayerCommand("tbxevents", "Active ou desactive les logs des events Toolbox.", (_, player) => ToggleEventDiagnostics(player, state));
        CommandApi.RegisterPlayerCommand("tbxserver", "Teste ServerApi.", (ctx, player) => Server(ctx, player, plugin));
        CommandApi.RegisterPlayerCommand("tbxcombat", "Teste les API combat, heal, velocity et quelques events.", (_, player) => Combat(player));
        CommandApi.RegisterPlayerCommand("tbxinteract", "Prepare des items pour tester les events d'interaction.", (_, player) => Interact(player));
        CommandApi.RegisterPlayerCommand("tbxblocks", "Teste les helpers blocks, lumiere, biome, meteo et liquides.", Blocks);
        CommandApi.RegisterPlayerCommand("tbxentities", "Teste EntityApi et les couches de visibilite.", Entities);
        CommandApi.RegisterPlayerCommand("tbxworldstate", "Teste spawn, temps, difficulte et dimensions.", WorldState);

        CommandApi.CreateCommandBuilder("tbxbuilder", "Exemple de CommandBuilder.")
            .AddAlias("tbxb")
            .OnExecute(ctx => ctx.SendMessage("CommandBuilder fonctionne. Essaie aussi /tbxkit 3 Test ou /tbxeffect speed 10."))
            .RegisterCommand();

        CommandApi.RegisterPlayerCommand<ToolboxKitCommand>("tbxkit", "Commande a parametres: <count> [name].", "tbxgive");
        CommandApi.RegisterPlayerCommand<ToolboxEffectCommand>("tbxeffect", "Commande a parametres: <effect> [seconds].");
        CommandApi.RegisterCommand<ToolboxEchoCommand>("tbxecho", "Commande raw text: <message...>.");
    }

    private static void ShowHelp(CommandContext ctx)
    {
        ctx.SendMessage("ToolboxExample:");
        ctx.SendMessage("/tbxprofile - infos joueur");
        ctx.SendMessage("/tbxitems - items, NBT, inventaire, cooldown");
        ctx.SendMessage("/tbxcustomitem - arme, nourriture et armure custom avec composants");
        ctx.SendMessage("/tbxcustomblock - blocs custom solide, lumineux et demi-hauteur");
        ctx.SendMessage("/tbxforms - simple/modal/custom forms");
        ctx.SendMessage("/tbxinv - faux conteneurs interactifs avec de vrais items");
        ctx.SendMessage("/tbxui - title, toast, scoreboard, HUD/input");
        ctx.SendMessage("/tbxworld - monde, blocks, entites, dimensions");
        ctx.SendMessage("/tbxtask - task immediate/later/repeating");
        ctx.SendMessage("/tbxpacketlog - inspecte les packets via events");
        ctx.SendMessage("/tbxconf <show|write|reload|defaults|remove|reset> [value] - config JSON");
        ctx.SendMessage("/tbxevents - inspecte les events rares en console/tip");
        ctx.SendMessage("/tbxserver - infos ServerApi");
        ctx.SendMessage("/tbxcombat - damage/heal/knockback");
        ctx.SendMessage("/tbxinteract - items pour tester interactions");
        ctx.SendMessage("/tbxblocks - blocks, biome, lumiere, liquides");
        ctx.SendMessage("/tbxentities - visibilite/entity helpers");
        ctx.SendMessage("/tbxentity - entite custom, proprietes et tick");
        ctx.SendMessage("/tbxworldstate - temps, spawn, difficulte, dimension");
        ctx.SendMessage("/tbxkit <count> [name] - commande a parametres");
        ctx.SendMessage("/tbxeffect <speed|jumpboost|nightvision> [seconds]");
        ctx.SendMessage("/tbxecho <message...>");
    }

    private static void ShowProfile(CommandContext ctx, Player player)
    {
        var identity = PlayerApi.GetIdentitySnapshot(player);
        var movement = PlayerApi.GetMovementState(player);
        var physical = PlayerApi.GetPhysicalState(player);
        var food = PlayerApi.GetFoodState(player);
        var xp = PlayerApi.GetExperienceState(player);

        ctx.SendMessage($"Name={identity.Name} UUID={identity.UniqueId} XUID={identity.Xuid}");
        ctx.SendMessage($"Position={PlayerApi.GetPosition(player)} Ping={PlayerApi.GetPing(player).TotalMilliseconds:0}ms");
        ctx.SendMessage($"Health={PlayerApi.GetHealth(player):0.0}/{PlayerApi.GetMaxHealth(player):0.0} Food={food.Food} XP={xp.Experience}");
        ctx.SendMessage($"Move sprint={movement.Sprinting} sneak={movement.Sneaking} flying={movement.Flying}");
        ctx.SendMessage($"Ground={physical.OnGround} fall={physical.FallDistance:0.0} breathing={physical.Breathing}");
    }

    public static void GiveItems(Player player)
    {
        var sword = ItemFactory.CreateBuilder(new Item.Sword(Item.ToolTierDiamond))
            .SetCustomName("Toolbox sword")
            .SetLore("Cree avec ItemStackBuilder", "NBT via ItemNbtApi")
            .AddEnchantment(Item.Sharpness, 3, force: true)
            .AddEnchantment(Item.Unbreaking, 2, force: true)
            .SetUnbreakable()
            .SetTag("toolbox.owner", PlayerApi.GetName(player))
            .SetTag("toolbox.demo", true)
            .Build();

        var apples = ItemFactory.CreateBuilder(new Item.GoldenApple(), 4)
            .SetCustomName("Toolbox apples")
            .SetTag("toolbox.kind", "food")
            .Build();

        var owner = ItemNbtApi.GetString(sword, "toolbox.owner", "inconnu");
        InventoryApi.GiveItem(player, sword);
        InventoryApi.GiveItem(player, apples);
        InventoryApi.GiveItem(player, ItemFactory.CreateTotem(2));
        InventoryApi.SetArmor(player, ItemFactory.CreateDiamondArmor());
        ItemCooldownApi.SetCooldown(player, apples, TimeSpan.FromSeconds(3));

        PlayerApi.Feed(player);
        PlayerApi.HealFull(player);
        PlayerApi.AddExperience(player, 15);
        SoundApi.PlaySound(player, ToolboxSound.LevelUp);
        PlayerApi.SendMessage(player, $"Items envoyes. NBT owner={owner}, cooldown golden apple={ItemCooldownApi.HasCooldown(player, apples)}");
    }

    private static void GiveCustomItems(Player player, ExampleCustomItemSet items)
    {
        var sword = CustomItemApi.CreateBuilder(items.Sword)
            .SetLore("Degats: 8", "Durabilite: 850", "Brillance et tenue en main")
            .SetTag("toolbox.custom_item", true)
            .Build();
        var apples = CustomItemApi.CreateBuilder(items.Apple, 4)
            .SetLore("Restaure 6 points de faim", "Cooldown: 1 seconde")
            .Build();
        var helmet = CustomItemApi.CreateBuilder(items.Helmet)
            .SetLore("Protection: 4", "Durabilite: 600", "Slot: tete")
            .Build();
        InventoryApi.GiveItem(player, sword);
        InventoryApi.GiveItem(player, apples);
        InventoryApi.GiveItem(player, helmet);
        PlayerApi.SendMessage(player, "Epee, pommes et casque custom ajoutes.");
    }

    private static void GiveCustomBlocks(Player player, ExampleCustomBlockSet blocks)
    {
        InventoryApi.GiveItem(player, CustomBlockApi.CreateBuilder(blocks.Ruby, 16).SetLore("Bloc solide", "Hardness: 5").Build());
        InventoryApi.GiveItem(player, CustomBlockApi.CreateBuilder(blocks.Lamp, 16).SetLore("Lumiere: 15", "Transparent").Build());
        InventoryApi.GiveItem(player, CustomBlockApi.CreateBuilder(blocks.Pedestal, 16).SetLore("Collision demi-hauteur").Build());
        PlayerApi.SendMessage(player, "Blocs custom ajoutes. Tu peux les placer et les casser normalement.");
    }

    public static void ShowUi(Player player)
    {
        var title = TitleApi.CreateTitleWithText("Toolbox");
        title = TitleApi.SetSubtitle(title, "TitleApi");
        title = TitleApi.SetActionText(title, "Action text");
        title = TitleApi.SetDurationTicks(title, 60);
        TitleApi.SendTitle(player, title);
        ToastApi.SendToast(player, "Toolbox", "ToastApi fonctionne.");

        var scoreboard = ScoreboardApi.CreateScoreboard("Toolbox")
            .SetLine(0, $"Player: {PlayerApi.GetName(player)}")
            .SetLine(1, $"Health: {PlayerApi.GetHealth(player):0.0}")
            .SetLine(2, $"XP: {PlayerApi.GetExperience(player)}")
            .SetLine(3, $"Food: {PlayerApi.GetFood(player)}")
            .AddLine("AddLine fonctionne");
        ScoreboardApi.SendScoreboard(player, scoreboard);

        PlayerControlApi.HideHud(player, PlayerHudElement.ItemText);
        var hidden = PlayerControlApi.IsHudHidden(player, PlayerHudElement.ItemText);
        PlayerControlApi.ShowHud(player, PlayerHudElement.ItemText);
        PlayerControlApi.LockInput(player, PlayerInputLock.Jump);
        var locked = PlayerControlApi.IsInputLocked(player, PlayerInputLock.Jump);
        PlayerControlApi.UnlockInput(player, PlayerInputLock.Jump);
        PlayerControlApi.ShowCoordinates(player);
        PlayerControlApi.SendSleepingIndicator(player, 1, 10);
        PlayerControlApi.RemoveBossBar(player);

        PlayerApi.SendMessage(player, $"UI test: itemTextHidden={hidden} jumpLocked={locked}");
    }

    private static void World(CommandContext ctx, Player player)
    {
        var tx = ctx.GetTransaction();
        if (tx is null)
        {
            ctx.SendError("Cette commande a besoin d'une transaction world.");
            return;
        }

        var world = WorldApi.GetWorld(tx);
        var position = PlayerApi.GetBlockPosition(player);
        var target = PositionApi.AddToBlockPosition(position, 1, 0, 0);
        var oldBlock = BlockApi.GetBlock(tx, target);
        var newBlock = BlockFactory.RequireBlockByName("minecraft:gold_block");

        BlockApi.SetBlock(tx, target, newBlock, BlockApi.CreateSetOptions(disableRedstoneUpdates: true));
        SoundApi.PlaySound(tx, PlayerApi.GetPosition(player), SoundApi.CreateBlockPlaceSound(newBlock));

        var nearbyEntities = EntityApi.GetNearbyEntities(tx, PlayerApi.GetPosition(player), 8).Count();
        var nearbyGold = BlockApi.FindNearbyBlocks(tx, target, 4, newBlock).Count();
        var overworld = DimensionApi.GetPresetDimension(DimensionPreset.Overworld);
        var custom = DimensionApi.CreateNetherLikeDimension();

        ctx.SendMessage($"World={WorldApi.GetName(world)} Tick={WorldApi.GetCurrentTick(tx)} Range={WorldApi.GetRange(tx)}");
        ctx.SendMessage($"Block remplace a {target}: {oldBlock.GetType().Name} -> {newBlock.GetType().Name}, goldNearby={nearbyGold}");
        ctx.SendMessage($"Entities radius 8={nearbyEntities}, overworldRange={DimensionApi.GetRange(overworld)}, netherWaterEvaporates={DimensionApi.DoesWaterEvaporate(custom)}");
    }

    private static void Server(CommandContext ctx, Player player, ToolboxPlugin plugin)
    {
        var server = plugin.Server();
        var tx = ctx.GetTransaction();
        var online = ServerApi.GetOnlinePlayers(server, tx).Select(PlayerApi.GetName).ToArray();
        var byName = ServerApi.GetPlayerByName(server, PlayerApi.GetName(player));
        var byUuid = ServerApi.GetPlayer(server, PlayerApi.GetUniqueId(player));
        var byXuid = ServerApi.GetPlayerByXuid(server, PlayerApi.GetXuid(player));

        ctx.SendMessage($"Server players={ServerApi.GetOnlineCount(server)}/{ServerApi.GetMaxPlayers(server)} online=[{string.Join(", ", online)}]");
        ctx.SendMessage($"Worlds: overworld={WorldApi.GetName(ServerApi.GetOverworld(server))}, nether={WorldApi.GetName(ServerApi.GetNether(server))}, end={WorldApi.GetName(ServerApi.GetEnd(server))}");
        ctx.SendMessage($"Lookup self: name={byName.Ok} uuid={byUuid.Ok} xuid={byXuid.Ok}");
    }

    private static void Combat(Player player)
    {
        var before = PlayerApi.GetHealth(player);
        var damage = PlayerApi.Damage(player, 1);
        var healed = PlayerApi.Heal(player, 1);
        PlayerApi.SetAbsorption(player, 2);
        PlayerApi.KnockBack(player, PlayerApi.GetPosition(player), 0.25, 0.25);
        SoundApi.PlaySound(player, SoundApi.CreateAttackSound(damage.Vulnerable));
        PlayerApi.SendMessage(player, $"Combat test: health {before:0.0}->{PlayerApi.GetHealth(player):0.0}, damage={damage.Damage:0.0}, healed={healed:0.0}, vulnerable={damage.Vulnerable}");
    }

    private static void Interact(Player player)
    {
        InventoryApi.GiveItem(player, ItemFactory.CreateBuilder(new Item.Apple(), 4).SetCustomName("Toolbox consume test").Build());
        InventoryApi.GiveItem(player, ItemFactory.CreateBuilder(new Item.Bow()).SetCustomName("Toolbox release test").Build());
        InventoryApi.GiveItem(player, ItemFactory.CreateBuilder(new Item.FlintAndSteel()).SetCustomName("Toolbox use-on-block test").Build());
        InventoryApi.GiveItem(player, ItemFactory.CreateBuilder(new Item.Pickaxe(Item.ToolTierDiamond)).SetCustomName("Toolbox break test").Build());
        InventoryApi.GiveItem(player, ItemFactory.CreateBuilder(new Item.Sword(Item.ToolTierDiamond)).SetCustomName("Toolbox attack test").Build());
        PlayerApi.AddExperience(player, 1);
        PlayerApi.SendMessage(player, "Interaction test: mange la pomme, charge l'arc, pose/use le briquet, casse un block, drop/pickup un item, change de slot.");
    }

    private static void Blocks(CommandContext ctx, Player player)
    {
        var tx = ctx.GetTransaction();
        if (tx is null)
        {
            ctx.SendError("Cette commande a besoin d'une transaction world.");
            return;
        }

        var pos = PlayerApi.GetBlockPosition(player);
        var block = BlockApi.GetBlock(tx, pos);
        var loaded = BlockApi.GetLoadedBlock(tx, pos);
        var liquid = BlockApi.GetLiquid(tx, pos);
        var range = BlockApi.GetRange(tx);
        var biome = BlockApi.GetBiome(tx, pos);
        var highest = BlockApi.GetHighestBlock(tx, pos.X(), pos.Z());
        var light = BlockApi.GetLight(tx, pos);
        var skyLight = BlockApi.GetSkyLight(tx, pos);
        var raining = BlockApi.IsRainingAt(tx, pos);
        var snowing = BlockApi.IsSnowingAt(tx, pos);
        var thundering = BlockApi.IsThunderingAt(tx, pos);
        var temperature = BlockApi.GetTemperature(tx, pos);

        ctx.SendMessage($"Block pos={pos} type={block.GetType().Name} loaded={loaded.Ok} outOfBounds={BlockApi.IsOutOfBounds(tx, pos)} range={range}");
        ctx.SendMessage($"World scan: highest={highest} light={light}/{skyLight} biome={biome.GetType().Name} temp={temperature:0.00}");
        ctx.SendMessage($"Weather at pos: raining={raining} snowing={snowing} thundering={thundering} liquid={liquid.Ok}");
    }

    private static void Entities(CommandContext ctx, Player player)
    {
        var tx = ctx.GetTransaction();
        if (tx is null)
        {
            ctx.SendError("Cette commande a besoin d'une transaction world.");
            return;
        }

        var entities = EntityApi.GetNearbyEntities(tx, PlayerApi.GetPosition(player), 16).ToArray();
        var handle = EntityApi.GetHandle(player);
        var resolved = EntityApi.GetEntity(handle, tx);

        EntityApi.ViewNameTag(player, player, "Toolbox self nametag");
        EntityApi.ViewScoreTag(player, player, $"entities={entities.Length}");
        EntityApi.ViewVisibility(player, player, EntityVisibility.ForceVisible);
        EntityApi.RemoveViewLayer(player, player);
        EntityApi.ViewPublicNameTag(player, player);
        EntityApi.ViewPublicScoreTag(player, player);

        ctx.SendMessage($"Entities nearby={entities.Length}, selfHandleClosed={EntityApi.IsClosed(handle)}, selfUuid={EntityApi.GetUniqueId(handle)}, resolvedSelf={resolved.Ok}");
    }

    private static void WorldState(CommandContext ctx, Player player)
    {
        var tx = ctx.GetTransaction();
        if (tx is null)
        {
            ctx.SendError("Cette commande a besoin d'une transaction world.");
            return;
        }

        var world = WorldApi.GetWorld(tx);
        var spawn = WorldApi.GetSpawn(world);
        var playerSpawn = WorldApi.GetPlayerSpawn(world, player);
        var difficulty = WorldApi.GetDifficulty(world);
        var difficultyId = WorldApi.GetDifficultyId(difficulty);
        var time = WorldApi.GetTime(world);
        var dimension = WorldApi.GetDimension(world);
        var custom = DimensionApi.CreateCustomDimension(-32, 128, timeCycle: false);

        WorldApi.SetPlayerSpawn(world, player, PlayerApi.GetBlockPosition(player));
        WorldApi.SetTime(world, 6000);
        WorldApi.SetDifficulty(world, difficulty);

        ctx.SendMessage($"World state: spawn={spawn}, playerSpawnBefore={playerSpawn}, time={time}->6000, difficultyId={difficultyId.Id}/{difficultyId.Ok}");
        ctx.SendMessage($"Dimension: range={DimensionApi.GetRange(dimension)}, waterEvaporates={DimensionApi.DoesWaterEvaporate(dimension)}, customRange={custom.BuildRange}, customTimeCycle={custom.HasTimeCycle}");
    }

    private static void Tasks(CommandContext ctx, Player player, ExampleState state)
    {
        var tx = ctx.GetTransaction();
        if (tx is null)
        {
            ctx.SendError("Cette commande a besoin d'une transaction world.");
            return;
        }

        var world = WorldApi.GetWorld(tx);
        TaskApi.RunTask(world, taskTx => WorldApi.BroadcastMessage(taskTx, "Task immediate via Toolbox."));
        TaskApi.RunTaskLaterTicks(world, 40, taskTx => WorldApi.BroadcastMessage(taskTx, "Task delayed 40 ticks."));

        if (state.HasRepeatingTask())
        {
            ExampleForms.OpenConfirmReset(player, state);
            return;
        }

        state.SetRepeatingTask(TaskApi.RunTaskTimerTicks(world, 20, 20, taskTx =>
        {
            var run = state.IncrementRepeatingTaskRuns();
            WorldApi.BroadcastMessage(taskTx, $"Repeating task #{run}");
            if (run >= 5)
            {
                state.StopRepeatingTask();
            }
        }));

        PlayerApi.SendMessage(player, $"Task convert: 40 ticks = {TimeApi.ConvertGameTicksToDuration(40).TotalSeconds:0.0}s");
    }

    private static void TogglePacketLog(Player player, ExampleState state)
    {
        state.SetPacketLogEnabled(!state.PacketLogEnabled);
        PlayerApi.SendMessage(player, $"Packet log={state.PacketLogEnabled}. Dernier packet: {state.LastPacket}");
    }

    private static void ToggleEventDiagnostics(Player player, ExampleState state)
    {
        state.SetEventDiagnosticsEnabled(!state.EventDiagnosticsEnabled);
        PlayerApi.SendMessage(player, $"Event diagnostics={state.EventDiagnosticsEnabled}. Dernier event: {state.LastEvent}");
    }
}

public enum ToolboxDemoEffect
{
    Speed,
    JumpBoost,
    NightVision,
}

public sealed class ToolboxKitCommand : PlayerToolboxCommand
{
    internal static ExampleState? State { get; set; }

    [Cmd.Tag("count")]
    public int Count;

    [Cmd.Tag("name")]
    public Cmd.Optional<Cmd.Varargs> Name;

    protected override void Execute(CommandContext ctx, Player player)
    {
        var count = System.Math.Clamp(Count, 1, 64);
        var label = Name.LoadOr(new Cmd.Varargs("Toolbox kit")).ToString();
        var item = ItemFactory.CreateBuilder(new Item.GoldenApple(), count)
            .SetCustomName(label)
            .SetTag("toolbox.command", "tbxkit")
            .Build();

        InventoryApi.GiveItem(player, item);
        ToastApi.SendToast(player, "Toolbox kit", $"{count} golden apples");
        State?.RecordPacket($"Commande tbxkit executee par {PlayerApi.GetName(player)}");
    }
}

public sealed class ToolboxEffectCommand : PlayerToolboxCommand
{
    internal static ExampleState? State { get; set; }

    [Cmd.Tag("effect")]
    public ToolboxDemoEffect Effect;

    [Cmd.Tag("seconds")]
    public Cmd.Optional<int> Seconds;

    protected override void Execute(CommandContext ctx, Player player)
    {
        var seconds = System.Math.Clamp(Seconds.LoadOr(10), 1, 120);
        var type = Effect switch
        {
            ToolboxDemoEffect.JumpBoost => Dragonfly.Effect.JumpBoost,
            ToolboxDemoEffect.NightVision => Dragonfly.Effect.NightVision,
            _ => Dragonfly.Effect.Speed,
        };

        EffectApi.AddEffect(player, EffectApi.Create(type, 1, seconds * 20));
        PlayerApi.SendMessage(player, $"Effect {Effect} applique pendant {seconds}s.");
        State?.RecordPacket($"Commande tbxeffect {Effect} {seconds}s");
    }
}

public sealed class ToolboxEchoCommand : ToolboxCommand
{
    [Cmd.Tag("message")]
    public Cmd.Varargs Message;

    protected override void Execute(CommandContext ctx)
    {
        ctx.SendMessage($"Echo: {Message}");
    }
}
