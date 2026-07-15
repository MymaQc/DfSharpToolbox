using System.Text.Json;

namespace Toolbox.Forms.Elements;

internal sealed class SliderElement(string name, string text, double min, double max, double step, double defaultValue, string tooltip) : CustomFormElement(name)
{
    public override void Write(Utf8JsonWriter writer)
    {
        writer.WriteStartObject();
        writer.WriteString("type", "slider");
        writer.WriteString("text", text);
        writer.WriteNumber("min", min);
        writer.WriteNumber("max", max);
        writer.WriteNumber("step", step);
        writer.WriteNumber("default", defaultValue);
        FormJson.WriteTooltip(writer, tooltip);
        writer.WriteEndObject();
    }

    public override void Read(JsonElement value, IDictionary<string, object?> values, IDictionary<string, string[]> options)
    {
        if (Name is not null && value.ValueKind == JsonValueKind.Number && value.TryGetDouble(out var result))
        {
            values[Name] = result;
        }
    }
}
