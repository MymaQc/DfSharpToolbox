using System.Text.Json;

namespace Toolbox.Forms.Elements;

internal sealed class InputElement(string name, string text, string defaultValue, string placeholder, string tooltip) : CustomFormElement(name)
{
    public override void Write(Utf8JsonWriter writer)
    {
        writer.WriteStartObject();
        writer.WriteString("type", "input");
        writer.WriteString("text", text);
        writer.WriteString("default", defaultValue);
        writer.WriteString("placeholder", placeholder);
        FormJson.WriteTooltip(writer, tooltip);
        writer.WriteEndObject();
    }

    public override void Read(JsonElement value, IDictionary<string, object?> values, IDictionary<string, string[]> options)
    {
        if (Name is not null && value.ValueKind == JsonValueKind.String)
        {
            values[Name] = value.GetString() ?? string.Empty;
        }
    }
}
