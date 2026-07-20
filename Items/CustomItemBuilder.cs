using Dragonfly;

namespace Toolbox.Items;

public sealed class CustomItemBuilder
{
    private readonly string _identifier;
    private readonly string _displayName;
    private readonly byte[] _texturePng;
    private readonly Item.CustomItemData _data = new();
    private Item.CustomItemCategory _category = Item.CustomItemCategory.Items;
    private int _maxStackSize = 64;
    private string _creativeGroup = "";

    internal CustomItemBuilder(string identifier, string displayName, byte[] texturePng)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(identifier);
        ArgumentException.ThrowIfNullOrWhiteSpace(displayName);
        ArgumentNullException.ThrowIfNull(texturePng);
        _identifier = identifier;
        _displayName = displayName;
        _texturePng = texturePng.ToArray();
    }

    public CustomItemBuilder Category(Item.CustomItemCategory category, string creativeGroup = "")
    {
        _category = category;
        _creativeGroup = creativeGroup ?? throw new ArgumentNullException(nameof(creativeGroup));
        return this;
    }

    public CustomItemBuilder MaxStackSize(int value)
    {
        if (value is < 1 or > 64)
        {
            throw new ArgumentOutOfRangeException(nameof(value));
        }
        _maxStackSize = value;
        return this;
    }

    public CustomItemBuilder Glinted(bool value = true) => Property("foil", value);
    public CustomItemBuilder HandEquipped(bool value = true) => Property("hand_equipped", value);
    public CustomItemBuilder AllowOffHand(bool value = true) => Property("allow_off_hand", value);
    public CustomItemBuilder CanDestroyInCreative(bool value = true) => Property("can_destroy_in_creative", value);
    public CustomItemBuilder LiquidClipped(bool value = true) => Property("liquid_clipped", value);
    public CustomItemBuilder ShouldDespawn(bool value = true) => Property("should_despawn", value);
    public CustomItemBuilder StackedByData(bool value = true) => Property("stacked_by_data", value);

    public CustomItemBuilder AttackDamage(int value)
    {
        return value < 0 ? throw new ArgumentOutOfRangeException(nameof(value)) : Property("damage", value);
    }

    public CustomItemBuilder MiningSpeed(float value)
    {
        RequireFinitePositive(value, nameof(value), allowZero: true);
        return Property("mining_speed", value);
    }

    public CustomItemBuilder UseAnimation(CustomItemUseAnimation animation) =>
        Property("use_animation", (int)animation);

    public CustomItemBuilder UseDuration(TimeSpan duration)
    {
        RequirePositive(duration, nameof(duration));
        return Property("use_duration", checked((int)System.Math.Ceiling(duration.TotalSeconds * 20)));
    }

    public CustomItemBuilder HoverTextColor(string color) => Property("hover_text_color", Required(color, nameof(color)));
    public CustomItemBuilder InteractButton(string text) => Property("interact_button", Required(text, nameof(text)));
    public CustomItemBuilder Rarity(CustomItemRarity rarity) => Property("rarity", rarity.ToString().ToLowerInvariant());

    public CustomItemBuilder Cooldown(TimeSpan duration, string category = "")
    {
        RequirePositive(duration, nameof(duration));
        return Component("minecraft:cooldown",
            ("duration", (float)duration.TotalSeconds),
            ("category", string.IsNullOrWhiteSpace(category) ? IdentifierName() : category));
    }

    public CustomItemBuilder Durability(
        int maximum,
        int damageChanceMin = 0,
        int damageChanceMax = 0,
        int damagePerAttack = 1,
        int damagePerBlock = 1,
        bool persistent = false)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(maximum, 1);
        if (damageChanceMin is < 0 or > 100 || damageChanceMax is < 0 or > 100 || damageChanceMin > damageChanceMax)
        {
            throw new ArgumentOutOfRangeException(nameof(damageChanceMin));
        }
        ArgumentOutOfRangeException.ThrowIfNegative(damagePerAttack);
        ArgumentOutOfRangeException.ThrowIfNegative(damagePerBlock);
        _data.SetServerBehavior("durability_attack_damage", damagePerAttack)
            .SetServerBehavior("durability_break_damage", damagePerBlock)
            .SetServerBehavior("durability_persistent", persistent);
        return Component("minecraft:durability",
            ("max_durability", maximum),
            ("damage_chance", Values(("min", damageChanceMin), ("max", damageChanceMax))));
    }

    public CustomItemBuilder Food(int nutrition, float saturationModifier, bool canAlwaysEat = false)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(nutrition);
        RequireFinitePositive(saturationModifier, nameof(saturationModifier), allowZero: true);
        return Component("minecraft:food",
            ("nutrition", nutrition),
            ("saturation_modifier", saturationModifier),
            ("can_always_eat", canAlwaysEat));
    }

    public CustomItemBuilder Armor(
        int protection,
        CustomItemWearableSlot slot,
        string textureType = "none",
        float toughness = 0,
        float knockbackResistance = 0)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(protection);
        RequireFinitePositive(toughness, nameof(toughness), allowZero: true);
        if (!float.IsFinite(knockbackResistance) || knockbackResistance is < 0 or > 1)
        {
            throw new ArgumentOutOfRangeException(nameof(knockbackResistance));
        }
        _data.SetServerBehavior("armour_toughness", toughness)
            .SetServerBehavior("armour_knockback_resistance", knockbackResistance);
        Component("minecraft:armor",
            ("protection", protection),
            ("texture_type", textureType));
        return Wearable(slot, protection);
    }

    public CustomItemBuilder Wearable(CustomItemWearableSlot slot, int protection = 0)
    {
        return protection < 0 ? throw new ArgumentOutOfRangeException(nameof(protection)) : Component("minecraft:wearable", ("slot", WearableSlot(slot)), ("protection", protection));
    }

    public CustomItemBuilder Enchantable(CustomItemEnchantSlot slot, int value)
    {
        return value < 0 ? throw new ArgumentOutOfRangeException(nameof(value)) : Component("minecraft:enchantable", ("slot", EnchantSlot(slot)), ("value", value));
    }

    public CustomItemBuilder Fuel(TimeSpan duration)
    {
        RequirePositive(duration, nameof(duration));
        return Component("minecraft:fuel", ("duration", (float)duration.TotalSeconds));
    }

    public CustomItemBuilder FireResistant(bool value = true) =>
        Component("minecraft:fire_resistant", ("value", value));

    public CustomItemBuilder Compostable(float chance)
    {
        if (!float.IsFinite(chance) || chance is < 0 or > 1)
        {
            throw new ArgumentOutOfRangeException(nameof(chance));
        }
        return Component("minecraft:compostable", ("composting_chance", chance));
    }

    public CustomItemBuilder Projectile(string entityIdentifier = "", float minimumCriticalPower = 1.25f)
    {
        RequireFinitePositive(minimumCriticalPower, nameof(minimumCriticalPower), allowZero: true);
        return Component("minecraft:projectile",
            ("projectile_entity", entityIdentifier ?? throw new ArgumentNullException(nameof(entityIdentifier))),
            ("minimum_critical_power", minimumCriticalPower));
    }

    public CustomItemBuilder Throwable(
        bool swingAnimation = true,
        float launchPowerScale = 1,
        float minimumDrawDuration = 0,
        float maximumDrawDuration = 0,
        float maximumLaunchPower = 1,
        bool scalePowerByDrawDuration = false)
    {
        RequireFinitePositive(launchPowerScale, nameof(launchPowerScale), allowZero: true);
        RequireFinitePositive(minimumDrawDuration, nameof(minimumDrawDuration), allowZero: true);
        RequireFinitePositive(maximumDrawDuration, nameof(maximumDrawDuration), allowZero: true);
        RequireFinitePositive(maximumLaunchPower, nameof(maximumLaunchPower), allowZero: true);
        return Component("minecraft:throwable",
            ("do_swing_animation", swingAnimation),
            ("launch_power_scale", launchPowerScale),
            ("min_draw_duration", minimumDrawDuration),
            ("max_draw_duration", maximumDrawDuration),
            ("max_launch_power", maximumLaunchPower),
            ("scale_power_by_draw_duration", scalePowerByDrawDuration));
    }

    public CustomItemBuilder Tags(params string[] tags)
    {
        ArgumentNullException.ThrowIfNull(tags);
        return tags.Any(string.IsNullOrWhiteSpace) ? throw new ArgumentException("tags cannot contain empty values", nameof(tags)) : Component("minecraft:tags", ("tags", tags));
    }

    public CustomItemBuilder Record(string soundEvent, TimeSpan duration, int comparatorSignal = 1)
    {
        RequirePositive(duration, nameof(duration));
        if (comparatorSignal is < 0 or > 15)
        {
            throw new ArgumentOutOfRangeException(nameof(comparatorSignal));
        }
        return Component("minecraft:record",
            ("sound_event", Required(soundEvent, nameof(soundEvent))),
            ("duration", checked((int)System.Math.Ceiling(duration.TotalSeconds))),
            ("comparator_signal", comparatorSignal));
    }

    public CustomItemBuilder EntityPlacer(string entityIdentifier, string[]? dispenseOn = null, string[]? useOn = null) =>
        Component("minecraft:entity_placer",
            ("entity", Required(entityIdentifier, nameof(entityIdentifier))),
            ("dispense_on", dispenseOn ?? []),
            ("use_on", useOn ?? []));

    public CustomItemBuilder BlockPlacer(string blockIdentifier, bool useBlockAsIcon = false, params string[] useOn) =>
        Component("minecraft:block_placer",
            ("block", Required(blockIdentifier, nameof(blockIdentifier))),
            ("can_use_block_as_icon", useBlockAsIcon),
            ("use_on", useOn));

    public CustomItemBuilder UseModifiers(TimeSpan duration, float movementModifier = 1)
    {
        RequirePositive(duration, nameof(duration));
        RequireFinitePositive(movementModifier, nameof(movementModifier), allowZero: true);
        return Component("minecraft:use_modifiers",
            ("use_duration", (float)duration.TotalSeconds),
            ("movement_modifier", movementModifier));
    }

    public CustomItemBuilder SwingDuration(TimeSpan duration)
    {
        RequirePositive(duration, nameof(duration));
        return Component("minecraft:swing_duration", ("value", (float)duration.TotalSeconds));
    }

    public CustomItemBuilder BundleInteraction(int visibleSlots)
    {
        return visibleSlots is < 1 or > 64 ? throw new ArgumentOutOfRangeException(nameof(visibleSlots)) : Component("minecraft:bundle_interaction", ("num_viewable_slots", visibleSlots));
    }

    public CustomItemBuilder Storage(
        int slots,
        bool allowNestedStorage = true,
        string[]? allowedItems = null,
        string[]? bannedItems = null)
    {
        if (slots is < 0 or > 64)
        {
            throw new ArgumentOutOfRangeException(nameof(slots));
        }
        return Component("minecraft:storage_item",
            ("max_slots", slots),
            ("allow_nested_storage_items", allowNestedStorage),
            ("allowed_items", allowedItems ?? []),
            ("banned_items", bannedItems ?? []));
    }

    public CustomItemBuilder StorageWeightLimit(int maximumWeight)
    {
        return maximumWeight is < 0 or > 64 ? throw new ArgumentOutOfRangeException(nameof(maximumWeight)) : Component("minecraft:storage_item_weight_limit", ("max_weight_limit", maximumWeight));
    }

    public CustomItemBuilder StorageWeight(int weight)
    {
        return weight is < 0 or > 64 ? throw new ArgumentOutOfRangeException(nameof(weight)) : Component("minecraft:storage_item_weight_modifier", ("weight_in_storage_item", weight));
    }

    public CustomItemBuilder Dyeable(string defaultColor = "#ffffff")
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(defaultColor);
        var value = defaultColor.AsSpan();
        if (value.Length is not (4 or 7) || value[0] != '#' || !value[1..].ToString().All(Uri.IsHexDigit))
        {
            throw new ArgumentException("color must use #RGB or #RRGGBB", nameof(defaultColor));
        }
        return Component("minecraft:dyeable", ("default_color", defaultColor));
    }

    public CustomItemBuilder DamageAbsorption(params string[] causes)
    {
        ArgumentNullException.ThrowIfNull(causes);
        return causes.Any(string.IsNullOrWhiteSpace) ? throw new ArgumentException("damage causes cannot be empty", nameof(causes)) : Component("minecraft:damage_absorption", ("absorbable_causes", causes));
    }

    public CustomItemBuilder SwingSounds(string hit, string miss, string criticalHit) =>
        Component("minecraft:swing_sounds",
            ("attack_hit", Required(hit, nameof(hit))),
            ("attack_miss", Required(miss, nameof(miss))),
            ("attack_critical_hit", Required(criticalHit, nameof(criticalHit))));

    public CustomItemBuilder PiercingWeapon(float hitboxMargin = 0, int minimumReach = 0, int maximumReach = 3)
    {
        RequireFinitePositive(hitboxMargin, nameof(hitboxMargin), allowZero: true);
        if (minimumReach < 0 || maximumReach < minimumReach)
        {
            throw new ArgumentOutOfRangeException(nameof(minimumReach));
        }
        return Component("minecraft:piercing_weapon",
            ("hitbox_margin", hitboxMargin),
            ("reach", Values(("min", minimumReach), ("max", maximumReach))));
    }

    public CustomItemBuilder KineticWeapon(
        float hitboxMargin = 0,
        int minimumReach = 0,
        int maximumReach = 3,
        float damageModifier = 0,
        float damageMultiplier = 1,
        int delayTicks = 0)
    {
        RequireFinitePositive(hitboxMargin, nameof(hitboxMargin), allowZero: true);
        RequireFinitePositive(damageModifier, nameof(damageModifier), allowZero: true);
        RequireFinitePositive(damageMultiplier, nameof(damageMultiplier), allowZero: true);
        if (minimumReach < 0 || maximumReach < minimumReach)
        {
            throw new ArgumentOutOfRangeException(nameof(minimumReach));
        }
        ArgumentOutOfRangeException.ThrowIfNegative(delayTicks);
        return Component("minecraft:kinetic_weapon",
            ("hitbox_margin", hitboxMargin),
            ("reach", Values(("min", minimumReach), ("max", maximumReach))),
            ("damage_modifier", damageModifier),
            ("damage_multiplier", damageMultiplier),
            ("delay", delayTicks));
    }

    [Obsolete("minecraft:chargeable is deprecated; prefer UseModifiers for current Bedrock versions.")]
    public CustomItemBuilder Chargeable(float movementModifier, string onComplete = "")
    {
        RequireFinitePositive(movementModifier, nameof(movementModifier), allowZero: true);
        return Component("minecraft:chargeable", ("movement_modifier", movementModifier), ("on_complete", onComplete));
    }

    public CustomItemBuilder SetProperty(string name, object value) => Property(name, value);

    public CustomItemBuilder AddComponent(string name, IReadOnlyDictionary<string, object>? values = null)
    {
        _data.AddComponent(name, values);
        return this;
    }

    public Item.Custom Register() => Item.RegisterCustom(
        _identifier, _displayName, _texturePng, _category, _maxStackSize, _creativeGroup, _data);

    private CustomItemBuilder Property(string name, object value)
    {
        _data.SetProperty(name, value);
        return this;
    }

    private CustomItemBuilder Component(string name, params (string Name, object Value)[] values)
    {
        _data.AddComponent(name, Values(values));
        return this;
    }

    private static Dictionary<string, object> Values(params (string Name, object Value)[] values) =>
        values.ToDictionary(value => value.Name, value => value.Value, StringComparer.Ordinal);

    private string IdentifierName() => _identifier[(_identifier.IndexOf(':') + 1)..];

    private static string WearableSlot(CustomItemWearableSlot slot) => slot switch
    {
        CustomItemWearableSlot.Head => "slot.armor.head",
        CustomItemWearableSlot.Chest => "slot.armor.chest",
        CustomItemWearableSlot.Legs => "slot.armor.legs",
        CustomItemWearableSlot.Feet => "slot.armor.feet",
        CustomItemWearableSlot.MainHand => "slot.weapon.mainhand",
        CustomItemWearableSlot.OffHand => "slot.weapon.offhand",
        _ => throw new ArgumentOutOfRangeException(nameof(slot)),
    };

    private static string EnchantSlot(CustomItemEnchantSlot slot) => slot switch
    {
        CustomItemEnchantSlot.FishingRod => "fishing_rod",
        CustomItemEnchantSlot.FlintAndSteel => "flintsteel",
        CustomItemEnchantSlot.ArmorHead => "armor_head",
        CustomItemEnchantSlot.ArmorTorso => "armor_torso",
        CustomItemEnchantSlot.ArmorLegs => "armor_legs",
        CustomItemEnchantSlot.ArmorFeet => "armor_feet",
        _ => slot.ToString().ToLowerInvariant(),
    };

    private static string Required(string value, string parameter)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value, parameter);
        return value;
    }

    private static void RequirePositive(TimeSpan value, string parameter)
    {
        if (value <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(parameter);
        }
    }

    private static void RequireFinitePositive(float value, string parameter, bool allowZero)
    {
        if (!float.IsFinite(value) || value < 0 || !allowZero && value == 0)
        {
            throw new ArgumentOutOfRangeException(parameter);
        }
    }
}
