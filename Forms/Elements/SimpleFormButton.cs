using System.Text.Json;
using Dragonfly;

namespace Toolbox.Forms.Elements;

internal sealed class SimpleFormButton(string text, string image, Action<Form.Submitter, World.Tx> onClick)
{
    private string Text { get; } = text;

    private string Image { get; } = image;

    public Action<Form.Submitter, World.Tx> OnClick { get; } = onClick;

    public void Write(Utf8JsonWriter writer)
    {
        writer.WriteStartObject();
        writer.WriteString("type", "button");
        writer.WriteString("text", Text);
        if (!string.IsNullOrEmpty(Image))
        {
            writer.WritePropertyName("image");
            writer.WriteStartObject();
            writer.WriteString("type", Image.StartsWith("http:", StringComparison.Ordinal) || Image.StartsWith("https:", StringComparison.Ordinal) ? "url" : "path");
            writer.WriteString("data", Image);
            writer.WriteEndObject();
        }

        writer.WriteEndObject();
    }
}
