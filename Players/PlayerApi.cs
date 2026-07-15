using Dragonfly;
using Toolbox.Forms;

namespace Toolbox.Players;

public static class PlayerApi
{

    public static string GetName(Player player)
    {
        return player.Name();
    }

    public static Guid GetUniqueId(Player player)
    {
        return player.UUID();
    }

    public static string GetXuid(Player player)
    {
        return player.XUID();
    }

    public static Vector3 GetPosition(Player player)
    {
        return player.Position();
    }

    public static Rotation GetRotation(Player player)
    {
        return player.Rotation();
    }

    public static Cube.Pos GetBlockPosition(Player player)
    {
        return Cube.PosFromVec3(player.Position());
    }

    public static TimeSpan GetPing(Player player)
    {
        return player.Latency();
    }

    public static void SendMessage(Player player, params object?[] message)
    {
        player.Message(message);
    }

    public static void SendPopup(Player player, params object?[] message)
    {
        player.SendPopup(message);
    }

    public static void SendTip(Player player, params object?[] message)
    {
        player.SendTip(message);
    }

    public static void SendJukeboxPopup(Player player, params object?[] message)
    {
        player.SendJukeboxPopup(message);
    }

    public static void Kick(Player player, params object?[] reason)
    {
        player.Disconnect(reason);
    }

    public static void SetNameTag(Player player, string nameTag)
    {
        player.SetNameTag(nameTag);
    }

    public static void Teleport(Player player, Vector3 position)
    {
        player.Teleport(position);
    }

    public static void Move(Player player, Vector3 delta, double yaw = 0, double pitch = 0)
    {
        player.Move(delta, yaw, pitch);
    }

    public static void Displace(Player player, Vector3 delta)
    {
        player.Displace(delta);
    }

    public static Vector3 GetVelocity(Player player)
    {
        return player.Velocity();
    }

    public static void SetVelocity(Player player, Vector3 velocity)
    {
        player.SetVelocity(velocity);
    }

    public static void ChangeWorld(Player player, World world, Vector3 position)
    {
        player.ChangeWorld(world, position);
    }

    public static int GetFood(Player player)
    {
        return player.Food();
    }

    public static void SetFood(Player player, int food)
    {
        player.SetFood(food);
    }

    public static void Feed(Player player, int food = 20)
    {
        player.SetFood(food);
    }

    public static double GetHealth(Player player)
    {
        return player.Health();
    }

    public static double GetMaxHealth(Player player)
    {
        return player.MaxHealth();
    }

    public static void SetMaxHealth(Player player, double maxHealth)
    {
        player.SetMaxHealth(maxHealth);
    }

    public static int GetExperienceLevel(Player player)
    {
        return player.ExperienceLevel();
    }

    public static void SetExperienceLevel(Player player, int level)
    {
        player.SetExperienceLevel(level);
    }

    public static void AddExperienceLevels(Player player, int levels)
    {
        player.SetExperienceLevel(System.Math.Max(0, player.ExperienceLevel() + levels));
    }

    public static double GetExperienceProgress(Player player)
    {
        return player.ExperienceProgress();
    }

    public static void SetExperienceProgress(Player player, double progress)
    {
        player.SetExperienceProgress(progress);
    }

    public static double GetScale(Player player)
    {
        return player.Scale();
    }

    public static void SetScale(Player player, double scale)
    {
        player.SetScale(scale);
    }

    public static double Heal(Player player, double amount, World.HealingSource? source = null)
    {
        return player.Heal(amount, source ?? new Entity.FoodHealingSource());
    }

    public static double HealFull(Player player, World.HealingSource? source = null)
    {
        var amount = System.Math.Max(0, player.MaxHealth() - player.Health());
        return amount == 0 ? 0 : player.Heal(amount, source ?? new Entity.FoodHealingSource());
    }

    public static (double Damage, bool Vulnerable) Damage(Player player, double amount, World.DamageSource? source = null)
    {
        return player.Hurt(amount, source ?? new Entity.AttackDamageSource());
    }

    public static void SetInvisible(Player player, bool invisible = true)
    {
        if (invisible)
        {
            player.SetInvisible();
        }
        else
        {
            player.SetVisible();
        }
    }

    public static bool IsInvisible(Player player)
    {
        return player.Invisible();
    }

    public static void SetImmobile(Player player, bool immobile = true)
    {
        if (immobile)
        {
            player.SetImmobile();
        }
        else
        {
            player.SetMobile();
        }
    }

    public static bool IsImmobile(Player player)
    {
        return player.Immobile();
    }

    public static World.GameMode GetGameMode(Player player)
    {
        return player.GameMode();
    }

    public static void SetGameMode(Player player, World.GameMode gameMode)
    {
        player.SetGameMode(gameMode);
    }

    public static void AddEffect(Player player, Effect.Value effect)
    {
        player.AddEffect(effect);
    }

    public static void RemoveEffect(Player player, Effect.Type effect)
    {
        player.RemoveEffect(effect);
    }

    public static (Effect.Value Effect, bool Ok) GetEffect(Player player, Effect.Type effect)
    {
        return player.Effect(effect);
    }

    public static bool HasEffect(Player player, Effect.Type effect)
    {
        return player.Effect(effect).Ok;
    }

    public static IReadOnlyList<Effect.Value> GetEffects(Player player)
    {
        return player.Effects();
    }

    public static Inventory.Value GetInventory(Player player)
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

    public static void SetInventoryItem(Player player, int slot, Item.Stack item)
    {
        player.Inventory().SetItem(slot, item);
    }

    public static Item.Stack GetInventoryItem(Player player, int slot)
    {
        return player.Inventory().Item(slot);
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

    public static void SetArmor(Player player, Item.Stack helmet, Item.Stack chestplate, Item.Stack leggings, Item.Stack boots)
    {
        player.Armour().Set(helmet, chestplate, leggings, boots);
    }

    public static void SendForm(Player player, Form.Value form)
    {
        FormFactory.Send(player, form);
    }

    public static void CloseForm(Player player)
    {
        player.CloseForm();
    }
}
