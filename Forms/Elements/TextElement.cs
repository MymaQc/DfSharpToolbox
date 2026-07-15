using System.Text.Json;

namespace Toolbox.Forms.Elements;

internal sealed class TextElement(string? name, string type, string text) : CustomFormElement(name)
{
    public override void Write(Utf8JsonWriter writer)
    {
        writer.WriteStartObject();
        writer.WriteString("type", type);
        writer.WriteString("text", text);
        writer.WriteEndObject();
    }
}
