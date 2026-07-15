using System.Text.Json;

namespace Toolbox.Forms.Elements;

internal sealed class OptionsElement(string name, string type, string optionsField, string text, string[] formOptions, int defaultIndex, string tooltip) : CustomFormElement(name)
{
    public override void Write(Utf8JsonWriter writer)
    {
        writer.WriteStartObject();
        writer.WriteString("type", type);
        writer.WriteString("text", text);
        writer.WriteNumber("default", defaultIndex);
        writer.WritePropertyName(optionsField);
        writer.WriteStartArray();
        foreach (var option in formOptions)
        {
            writer.WriteStringValue(option);
        }

        writer.WriteEndArray();
        FormJson.WriteTooltip(writer, tooltip);
        writer.WriteEndObject();
    }

    public override void Read(JsonElement value, IDictionary<string, object?> values, IDictionary<string, string[]> options)
    {
        if (Name is null || value.ValueKind != JsonValueKind.Number || !value.TryGetInt32(out var result))
        {
            return;
        }

        values[Name] = result;
        options[Name] = formOptions;
    }
}
