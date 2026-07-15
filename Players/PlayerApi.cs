using Dragonfly;
using Toolbox.Forms;
using DPacket = Dragonfly.Packet.Packet;

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

    public static void PlaySound(Player player, World.Sound sound)
    {
        player.PlaySound(sound);
    }

    public static void SendPacket(Player player, DPacket packet)
    {
        ArgumentNullException.ThrowIfNull(player);
        ArgumentNullException.ThrowIfNull(packet);
        player.WritePacket(packet);
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

    public static PlayerSpeedSettings GetSpeedSettings(Player player)
    {
        return new PlayerSpeedSettings(player.Speed(), player.FlightSpeed(), player.VerticalFlightSpeed());
    }

    public static double GetSpeed(Player player)
    {
        return player.Speed();
    }

    public static void SetSpeed(Player player, double speed)
    {
        player.SetSpeed(speed);
    }

    public static double GetFlightSpeed(Player player)
    {
        return player.FlightSpeed();
    }

    public static void SetFlightSpeed(Player player, double speed)
    {
        player.SetFlightSpeed(speed);
    }

    public static double GetVerticalFlightSpeed(Player player)
    {
        return player.VerticalFlightSpeed();
    }

    public static void SetVerticalFlightSpeed(Player player, double speed)
    {
        player.SetVerticalFlightSpeed(speed);
    }

    public static void SetSpeedSettings(Player player, PlayerSpeedSettings settings)
    {
        player.SetSpeed(settings.Speed);
        player.SetFlightSpeed(settings.FlightSpeed);
        player.SetVerticalFlightSpeed(settings.VerticalFlightSpeed);
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

    public static PlayerPhysicalState GetPhysicalState(Player player)
    {
        return new PlayerPhysicalState(
            player.FallDistance(),
            player.Absorption(),
            player.Dead(),
            player.OnGround(),
            player.EyeHeight(),
            player.TorsoHeight(),
            player.Breathing());
    }

    public static double GetFallDistance(Player player)
    {
        return player.FallDistance();
    }

    public static void ResetFallDistance(Player player)
    {
        player.ResetFallDistance();
    }

    public static double GetAbsorption(Player player)
    {
        return player.Absorption();
    }

    public static void SetAbsorption(Player player, double health)
    {
        player.SetAbsorption(health);
    }

    public static bool IsDead(Player player)
    {
        return player.Dead();
    }

    public static bool IsOnGround(Player player)
    {
        return player.OnGround();
    }

    public static double GetEyeHeight(Player player)
    {
        return player.EyeHeight();
    }

    public static double GetTorsoHeight(Player player)
    {
        return player.TorsoHeight();
    }

    public static bool IsBreathing(Player player)
    {
        return player.Breathing();
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

    public static PlayerMovementState GetMovementState(Player player)
    {
        return new PlayerMovementState(
            player.Sprinting(),
            player.Sneaking(),
            player.Swimming(),
            player.Crawling(),
            player.Gliding(),
            player.Flying());
    }

    public static void SetMovementState(Player player, PlayerMovementState state)
    {
        SetSprinting(player, state.Sprinting);
        SetSneaking(player, state.Sneaking);
        SetSwimming(player, state.Swimming);
        SetCrawling(player, state.Crawling);
        SetGliding(player, state.Gliding);
        SetFlying(player, state.Flying);
    }

    public static bool IsSprinting(Player player)
    {
        return player.Sprinting();
    }

    public static void SetSprinting(Player player, bool sprinting = true)
    {
        if (sprinting)
        {
            player.StartSprinting();
        }
        else
        {
            player.StopSprinting();
        }
    }

    public static bool IsSneaking(Player player)
    {
        return player.Sneaking();
    }

    public static void SetSneaking(Player player, bool sneaking = true)
    {
        if (sneaking)
        {
            player.StartSneaking();
        }
        else
        {
            player.StopSneaking();
        }
    }

    public static bool IsSwimming(Player player)
    {
        return player.Swimming();
    }

    public static void SetSwimming(Player player, bool swimming = true)
    {
        if (swimming)
        {
            player.StartSwimming();
        }
        else
        {
            player.StopSwimming();
        }
    }

    public static bool IsCrawling(Player player)
    {
        return player.Crawling();
    }

    public static void SetCrawling(Player player, bool crawling = true)
    {
        if (crawling)
        {
            player.StartCrawling();
        }
        else
        {
            player.StopCrawling();
        }
    }

    public static bool IsGliding(Player player)
    {
        return player.Gliding();
    }

    public static void SetGliding(Player player, bool gliding = true)
    {
        if (gliding)
        {
            player.StartGliding();
        }
        else
        {
            player.StopGliding();
        }
    }

    public static bool IsFlying(Player player)
    {
        return player.Flying();
    }

    public static void SetFlying(Player player, bool flying = true)
    {
        if (flying)
        {
            player.StartFlying();
        }
        else
        {
            player.StopFlying();
        }
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
