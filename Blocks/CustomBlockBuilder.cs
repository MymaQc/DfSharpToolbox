using Dragonfly;

namespace Toolbox.Blocks;

public sealed class CustomBlockBuilder
{
    private readonly string _identifier;
    private readonly string _displayName;
    private readonly byte[] _texture;
    private readonly Block.CustomBlockData _data = new();

    private Item.CustomItemCategory _category = Item.CustomItemCategory.Construction;
    private string _group = "";
    private int _maxStackSize = 64;
    private byte[]? _geometry;
    private Action<CustomBlockInteractionContext>? _interact;
    private Action<CustomBlockPlacementContext>? _placing;

    internal CustomBlockBuilder(string identifier, string displayName, byte[] texture)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(identifier);
        ArgumentException.ThrowIfNullOrWhiteSpace(displayName);
        ArgumentNullException.ThrowIfNull(texture);
        _identifier = identifier;
        _displayName = displayName;
        _texture = texture.ToArray();
    }

    public CustomBlockBuilder Category(Item.CustomItemCategory category, string creativeGroup = "")
    {
        ArgumentNullException.ThrowIfNull(creativeGroup);
        _category = category;
        _group = creativeGroup;
        return this;
    }

    public CustomBlockBuilder MaxStackSize(int value)
    {
        if (value is < 1 or > 64) throw new ArgumentOutOfRangeException(nameof(value));
        _maxStackSize = value;
        return this;
    }

    public CustomBlockBuilder RenderMethod(Block.CustomBlockRenderMethod method) =>
        Native("render_method", (int)method);

    public CustomBlockBuilder Material(
        Block.CustomBlockRenderMethod method,
        bool faceDimming = true,
        bool? ambientOcclusion = null)
    {
        Native("render_method", (int)method);
        Native("face_dimming", faceDimming);
        if (ambientOcclusion is not null) Native("ambient_occlusion", ambientOcclusion.Value);
        return this;
    }

    public CustomBlockBuilder TextureFaces(params CustomBlockTextureFace[] faces)
    {
        ArgumentNullException.ThrowIfNull(faces);
        if (faces.Length == 0) throw new ArgumentException("at least one texture face is required", nameof(faces));
        return Native("texture_targets", faces.Distinct().Select(TextureFaceName).ToArray());
    }

    public CustomBlockBuilder MapColor(string hexColor) =>
        Native("map_color", Required(hexColor, nameof(hexColor)));

    public CustomBlockBuilder Hardness(float value, float? blastResistance = null)
    {
        NonNegative(value, nameof(value));
        Native("hardness", value);
        return blastResistance is null ? this : BlastResistance(blastResistance.Value);
    }

    public CustomBlockBuilder BlastResistance(float value)
    {
        NonNegative(value, nameof(value));
        return Native("blast_resistance", value);
    }

    public CustomBlockBuilder Friction(float value)
    {
        if (!float.IsFinite(value) || value is < 0 or > 1) throw new ArgumentOutOfRangeException(nameof(value));
        return Native("friction", value);
    }

    public CustomBlockBuilder Flammable(int encouragement, int flammability, bool catchesFromLava = true)
    {
        if (encouragement is < 0 or > 100) throw new ArgumentOutOfRangeException(nameof(encouragement));
        if (flammability is < 0 or > 100) throw new ArgumentOutOfRangeException(nameof(flammability));
        Native("fire_encouragement", encouragement);
        Native("flammability", flammability);
        return Native("lava_flammable", catchesFromLava);
    }

    public CustomBlockBuilder LightEmission(int level) => Light("light_emission", level);
    public CustomBlockBuilder LightDampening(int level) => Light("light_dampening", level);

    public CustomBlockBuilder CollisionBox(CustomBlockBox box) =>
        Native("collision_box", SerializeBox(box));

    public CustomBlockBuilder SelectionBox(CustomBlockBox box) =>
        Native("selection_box", SerializeBox(box));

    public CustomBlockBuilder NoCollision() => CollisionBox(default);
    public CustomBlockBuilder NoSelection() => SelectionBox(default);
    public CustomBlockBuilder SolidFaces(bool value = true) => Native("solid_faces", value);
    public CustomBlockBuilder Replaceable(bool value = true) => Native("replaceable", value);

    public CustomBlockBuilder PlacementFilter(IEnumerable<Cube.Face> allowedFaces, params string[] blockFilter)
    {
        ArgumentNullException.ThrowIfNull(allowedFaces);
        ArgumentNullException.ThrowIfNull(blockFilter);
        Native("allowed_faces", allowedFaces.Distinct().Select(face => (int)face).ToArray());
        return Native("placement_filter", blockFilter.Select(value => Required(value, nameof(blockFilter))).ToArray());
    }

    public CustomBlockBuilder LiquidDisplacement(bool water = true, bool lava = false)
    {
        Native("displace_water", water);
        return Native("displace_lava", lava);
    }

    public CustomBlockBuilder Scale(float x, float y, float z) =>
        Native("scale", SerializeVector(new CustomBlockVector(x, y, z)));

    public CustomBlockBuilder Translation(float x, float y, float z) =>
        Native("translation", SerializeVector(new CustomBlockVector(x, y, z)));

    public CustomBlockBuilder Rotation(int xQuarterTurns, int yQuarterTurns, int zQuarterTurns) =>
        Native("rotation", SerializeRotation(xQuarterTurns, yQuarterTurns, zQuarterTurns));

    public CustomBlockBuilder Geometry(string identifier, byte[] geometryJson)
    {
        ArgumentNullException.ThrowIfNull(geometryJson);
        if (geometryJson.Length == 0) throw new ArgumentException("geometry JSON cannot be empty", nameof(geometryJson));
        _geometry = geometryJson.ToArray();
        return Native("geometry_identifier", Required(identifier, nameof(identifier)));
    }

    public CustomBlockBuilder Geometry(string identifier, string geometryJson) =>
        Geometry(identifier, System.Text.Encoding.UTF8.GetBytes(Required(geometryJson, nameof(geometryJson))));

    public CustomBlockBuilder State(string name, params object[] values)
    {
        _data.AddState(name, values);
        return this;
    }

    public CustomBlockBuilder Permutation(string condition, Action<CustomBlockPermutationBuilder> configure)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(condition);
        ArgumentNullException.ThrowIfNull(configure);
        var permutation = new CustomBlockPermutationBuilder();
        configure(permutation);
        _data.AddPermutation(condition, permutation.Build());
        return this;
    }

    public CustomBlockBuilder OnInteract(Action<CustomBlockInteractionContext> callback)
    {
        ArgumentNullException.ThrowIfNull(callback);
        _interact += callback;
        return this;
    }

    public CustomBlockBuilder OnPlayerPlacing(Action<CustomBlockPlacementContext> callback)
    {
        ArgumentNullException.ThrowIfNull(callback);
        _placing += callback;
        return this;
    }

    public Block.Custom Register()
    {
        var block = Block.RegisterCustom(
            _identifier,
            _displayName,
            _texture,
            _category,
            _maxStackSize,
            _group,
            _geometry,
            _data);
        CustomBlockApi.RegisterCallbacks(block, _interact, _placing);
        return block;
    }

    private CustomBlockBuilder Native(string name, object value)
    {
        _data.SetNativeProperty(name, value);
        return this;
    }

    private CustomBlockBuilder Light(string name, int level)
    {
        if (level is < 0 or > 15) throw new ArgumentOutOfRangeException(nameof(level));
        return Native(name, level);
    }

    internal static Dictionary<string, object> SerializeBox(CustomBlockBox box)
    {
        var values = new[] { box.MinX, box.MinY, box.MinZ, box.MaxX, box.MaxY, box.MaxZ };
        if (values.Any(value => !float.IsFinite(value) || value is < 0 or > 1) ||
            box.MinX > box.MaxX || box.MinY > box.MaxY || box.MinZ > box.MaxZ)
            throw new ArgumentOutOfRangeException(nameof(box));
        return new Dictionary<string, object>
        {
            ["min_x"] = box.MinX, ["min_y"] = box.MinY, ["min_z"] = box.MinZ,
            ["max_x"] = box.MaxX, ["max_y"] = box.MaxY, ["max_z"] = box.MaxZ,
        };
    }

    internal static Dictionary<string, object> SerializeVector(CustomBlockVector vector)
    {
        if (!float.IsFinite(vector.X) || !float.IsFinite(vector.Y) || !float.IsFinite(vector.Z))
            throw new ArgumentOutOfRangeException(nameof(vector));
        return new Dictionary<string, object> { ["x"] = vector.X, ["y"] = vector.Y, ["z"] = vector.Z };
    }

    internal static Dictionary<string, object> SerializeRotation(int x, int y, int z) =>
        new() { ["x"] = x, ["y"] = y, ["z"] = z };

    internal static string TextureFaceName(CustomBlockTextureFace face) => face switch
    {
        CustomBlockTextureFace.All => "*",
        _ => face.ToString().ToLowerInvariant(),
    };

    internal static string Required(string value, string parameter)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value, parameter);
        return value;
    }

    internal static void NonNegative(float value, string parameter)
    {
        if (!float.IsFinite(value) || value < 0) throw new ArgumentOutOfRangeException(parameter);
    }
}

public sealed class CustomBlockPermutationBuilder
{
    private readonly Dictionary<string, object> _properties = new(StringComparer.Ordinal);

    public CustomBlockPermutationBuilder RenderMethod(Block.CustomBlockRenderMethod method) => Set("render_method", (int)method);
    public CustomBlockPermutationBuilder MapColor(string color) => Set("map_color", CustomBlockBuilder.Required(color, nameof(color)));
    public CustomBlockPermutationBuilder CollisionBox(CustomBlockBox box) => Set("collision_box", CustomBlockBuilder.SerializeBox(box));
    public CustomBlockPermutationBuilder SelectionBox(CustomBlockBox box) => Set("selection_box", CustomBlockBuilder.SerializeBox(box));
    public CustomBlockPermutationBuilder Scale(float x, float y, float z) => Set("scale", CustomBlockBuilder.SerializeVector(new(x, y, z)));
    public CustomBlockPermutationBuilder Translation(float x, float y, float z) => Set("translation", CustomBlockBuilder.SerializeVector(new(x, y, z)));
    public CustomBlockPermutationBuilder Rotation(int x, int y, int z) => Set("rotation", CustomBlockBuilder.SerializeRotation(x, y, z));
    public CustomBlockPermutationBuilder Geometry(string identifier) => Set("geometry_identifier", CustomBlockBuilder.Required(identifier, nameof(identifier)));

    internal IReadOnlyDictionary<string, object> Build() => _properties;

    private CustomBlockPermutationBuilder Set(string name, object value)
    {
        _properties[name] = value;
        return this;
    }
}
