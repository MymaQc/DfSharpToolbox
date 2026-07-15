using System.Text.Json;

namespace Toolbox.Forms.Elements;

internal abstract class CustomFormElement(string? name)
{
    protected string? Name { get; } = name;

    public abstract void Write(Utf8JsonWriter writer);

    public virtual void Read(JsonElement value, IDictionary<string, object?> values, IDictionary<string, string[]> options) { }
}
