using Dragonfly;
using Toolbox.Blocks;

namespace ToolboxExample;

internal static class ExampleCustomBlocks
{
    private const string RubyTexture =
        "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mNk+A8AAQUBAScY42YAAAAASUVORK5CYII=";

    internal static ExampleCustomBlockSet Register()
    {
        var texture = Convert.FromBase64String(RubyTexture);
        var ruby = CustomBlockApi.Create("toolbox:ruby_block", "Bloc de rubis", texture)
            .Hardness(5, blastResistance: 30)
            .MapColor("#d92848")
            .Register();

        var lamp = CustomBlockApi.Create("toolbox:ruby_lamp", "Lampe de rubis", texture)
            .Hardness(1.5f)
            .LightEmission(15)
            .LightDampening(0)
            .RenderMethod(Block.CustomBlockRenderMethod.Blend)
            .MapColor("#ff405f")
            .Register();

        var pedestal = CustomBlockApi.Create("toolbox:ruby_pedestal", "Socle de rubis", texture)
            .Hardness(2)
            .CollisionBox(new CustomBlockBox(.125f, 0, .125f, .875f, .5f, .875f))
            .SelectionBox(new CustomBlockBox(.125f, 0, .125f, .875f, .5f, .875f))
            .Scale(.75f, .5f, .75f)
            .Translation(0, -.25f, 0)
            .SolidFaces(false)
            .Register();

        return new ExampleCustomBlockSet(ruby, lamp, pedestal);
    }
}

internal readonly record struct ExampleCustomBlockSet(Block.Custom Ruby, Block.Custom Lamp, Block.Custom Pedestal);
