using System.Text.Json;

namespace Toolbox.Forms.Elements;

internal static class FormJson
{
    public static void WriteTooltip(Utf8JsonWriter writer, string tooltip)
    {
        if (!string.IsNullOrEmpty(tooltip))
        {
            writer.WriteString("tooltip", tooltip);
        }
    }
}
