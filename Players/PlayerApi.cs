using Dragonfly;
using Toolbox.Players.States;
using Toolbox.Timing;

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
    
    public static void SendToast(Player player, string title, string message)
    {
        player.SendToast(title, message);
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

    public static void MoveBy(Player player, Vector3 delta, double yaw = 0, double pitch = 0)
    {
        player.Move(delta, yaw, pitch);
    }

    public static void DisplaceBy(Player player, Vector3 delta)
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

    public static void KnockBack(Player player, Vector3 source, double force, double height)
    {
        player.KnockBack(source, force, height);
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

    public static PlayerFoodState GetFoodState(Player player)
    {
        return new PlayerFoodState(player.Food());
    }

    public static void AddFood(Player player, int points)
    {
        player.AddFood(points);
    }

    public static void Saturate(Player player, int food, double saturation)
    {
        player.Saturate(food, saturation);
    }

    public static void Exhaust(Player player, double points)
    {
        player.Exhaust(points);
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

    public static PlayerExperienceState GetExperienceState(Player player)
    {
        return new PlayerExperienceState(
            player.ExperienceLevel(),
            player.ExperienceProgress(),
            player.Experience(),
            player.EnchantmentSeed(),
            player.CanCollectExperience());
    }

    public static int GetExperience(Player player)
    {
        return player.Experience();
    }

    public static int AddExperience(Player player, int amount)
    {
        return player.AddExperience(amount);
    }

    public static void RemoveExperience(Player player, int amount)
    {
        player.RemoveExperience(amount);
    }

    public static long GetEnchantmentSeed(Player player)
    {
        return player.EnchantmentSeed();
    }

    public static void ResetEnchantmentSeed(Player player)
    {
        player.ResetEnchantmentSeed();
    }

    public static bool CanCollectExperience(Player player)
    {
        return player.CanCollectExperience();
    }

    public static bool CollectExperience(Player player, int amount)
    {
        return player.CollectExperience(amount);
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

    public static Skin GetSkin(Player player)
    {
        return player.Skin();
    }

    public static void SetSkin(Player player, Skin skin)
    {
        player.SetSkin(skin);
    }

    public static PlayerIdentitySnapshot GetIdentitySnapshot(Player player)
    {
        return new PlayerIdentitySnapshot(
            player.Name(),
            player.UUID(),
            player.XUID(),
            player.DeviceID(),
            player.DeviceModel(),
            player.SelfSignedID(),
            player.Locale(),
            player.Addr());
    }

    public static string GetDeviceId(Player player)
    {
        return player.DeviceID();
    }

    public static string GetDeviceModel(Player player)
    {
        return player.DeviceModel();
    }

    public static string GetSelfSignedId(Player player)
    {
        return player.SelfSignedID();
    }

    public static Language.Tag GetLocale(Player player)
    {
        return player.Locale();
    }

    public static Net.Addr? GetAddress(Player player)
    {
        return player.Addr();
    }

    public static string GetAddressText(Player player)
    {
        return player.Addr()?.String() ?? string.Empty;
    }

    public static string GetAddressNetwork(Player player)
    {
        return player.Addr()?.Network() ?? string.Empty;
    }

    public static PlayerFireState GetFireState(Player player)
    {
        return new PlayerFireState(player.FireProof(), player.OnFireDuration());
    }

    public static bool IsFireProof(Player player)
    {
        return player.FireProof();
    }

    public static TimeSpan GetOnFireDuration(Player player)
    {
        return player.OnFireDuration();
    }

    public static void SetOnFire(Player player, TimeSpan duration)
    {
        player.SetOnFire(duration);
    }

    public static void SetOnFireTicks(Player player, long ticks)
    {
        player.SetOnFire(Ticks(ticks));
    }

    public static void Extinguish(Player player)
    {
        player.Extinguish();
    }

    public static PlayerAirState GetAirState(Player player)
    {
        return new PlayerAirState(player.AirSupply(), player.MaxAirSupply());
    }

    public static TimeSpan GetAirSupply(Player player)
    {
        return player.AirSupply();
    }

    public static void SetAirSupply(Player player, TimeSpan duration)
    {
        player.SetAirSupply(duration);
    }

    public static void SetAirSupplyTicks(Player player, long ticks)
    {
        SetAirSupply(player, Ticks(ticks));
    }

    public static TimeSpan GetMaxAirSupply(Player player)
    {
        return player.MaxAirSupply();
    }

    private static void SetMaxAirSupply(Player player, TimeSpan duration)
    {
        player.SetMaxAirSupply(duration);
    }

    public static void SetMaxAirSupply(Player player, long ticks)
    {
        SetMaxAirSupply(player, Ticks(ticks));
    }

    public static PlayerStatusState GetStatusState(Player player)
    {
        return new PlayerStatusState(IsUsingItem(player), GetSleepState(player), GetDeathPosition(player));
    }

    public static bool IsUsingItem(Player player)
    {
        return player.UsingItem();
    }

    public static PlayerSleepState GetSleepState(Player player)
    {
        var (position, sleeping) = player.Sleeping();
        return new PlayerSleepState(position, sleeping);
    }

    public static PlayerDeathPosition GetDeathPosition(Player player)
    {
        var (position, dimension, found) = player.DeathPosition();
        return new PlayerDeathPosition(position, dimension, found);
    }

    public static double GetFinalDamage(Player player, double damage, World.DamageSource source)
    {
        return player.FinalDamageFrom(damage, source);
    }

    public static void EnableInstantRespawn(Player player)
    {
        player.EnableInstantRespawn();
    }

    public static void DisableInstantRespawn(Player player)
    {
        player.DisableInstantRespawn();
    }

    public static string GetNameTag(Player player)
    {
        return player.NameTag();
    }

    public static string GetScoreTag(Player player)
    {
        return player.ScoreTag();
    }

    public static void AbortBreaking(Player player)
    {
        player.AbortBreaking();
    }

    public static void FinishBreaking(Player player)
    {
        player.FinishBreaking();
    }

    public static void Jump(Player player)
    {
        player.Jump();
    }

    public static void MoveItemsToInventory(Player player)
    {
        player.MoveItemsToInventory();
    }

    public static void PunchAir(Player player)
    {
        player.PunchAir();
    }

    public static void ReleaseItem(Player player)
    {
        player.ReleaseItem();
    }

    public static void RemoveAllDebugShapes(Player player)
    {
        player.RemoveAllDebugShapes();
    }

    public static void SwingArm(Player player)
    {
        player.SwingArm();
    }

    public static void UseItem(Player player)
    {
        player.UseItem();
    }

    public static void WakeUp(Player player)
    {
        player.Wake();
    }

    public static void BreakBlock(Player player, Cube.Pos position)
    {
        player.BreakBlock(position);
    }

    public static void ContinueBreaking(Player player, Cube.Face face)
    {
        player.ContinueBreaking(face);
    }

    public static void PickBlock(Player player, Cube.Pos position)
    {
        player.PickBlock(position);
    }

    public static void SleepAt(Player player, Cube.Pos position)
    {
        player.Sleep(position);
    }

    public static void StartBreaking(Player player, Cube.Pos position, Cube.Face face)
    {
        player.StartBreaking(position, face);
    }

    public static void UseItemOnBlock(Player player, Cube.Pos position, Cube.Face face, Vector3 clickPosition)
    {
        player.UseItemOnBlock(position, face, clickPosition);
    }

    public static bool UseItemOnEntity(Player player, World.Entity entity)
    {
        return player.UseItemOnEntity(entity);
    }

    public static bool AttackEntity(Player player, World.Entity entity)
    {
        return player.AttackEntity(entity);
    }

    private static TimeSpan Ticks(long ticks)
    {
        return TimeApi.ConvertGameTicksToDuration(ticks);
    }
}
