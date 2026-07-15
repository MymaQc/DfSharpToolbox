using System.Text.Json;

namespace Toolbox.Forms.Elements;

internal sealed class ToggleElement(string name, string text, bool defaultValue, string tooltip) : CustomFormElement(name)
{
    public override void Write(Utf8JsonWriter writer)
    {
        writer.WriteStartObject();
        writer.WriteString("type", "toggle");
        writer.WriteString("text", text);
        writer.WriteBoolean("default", defaultValue);
        FormJson.WriteTooltip(writer, tooltip);
        writer.WriteEndObject();
    }

    public override void Read(JsonElement value, IDictionary<string, object?> values, IDictionary<string, string[]> options)
    {
        if (Name is not null && value.ValueKind is JsonValueKind.True or JsonValueKind.False)
        {
            values[Name] = value.GetBoolean();
        }
    }
}
