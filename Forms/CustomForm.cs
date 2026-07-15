using System.Text.Json;
using Dragonfly;
using Toolbox.Forms.Elements;

namespace Toolbox.Forms;

public sealed class CustomForm(string title) : Form.Value
{
    private readonly List<CustomFormElement> _elements = [];
    private Action<Form.Submitter, World.Tx>? _onClose;
    private Action<CustomFormResponse, Form.Submitter, World.Tx>? _onSubmit;

    private string Title { get; } = title;

    public byte[] MarshalJSON()
    {
        using var stream = new MemoryStream();
        using (var writer = new Utf8JsonWriter(stream))
        {
            writer.WriteStartObject();
            writer.WriteString("type", "custom_form");
            writer.WriteString("title", Title);
            writer.WritePropertyName("content");
            writer.WriteStartArray();
            foreach (var element in _elements)
            {
                element.Write(writer);
            }

            writer.WriteEndArray();
            writer.WriteEndObject();
        }

        return stream.ToArray();
    }

    public void SubmitJSON(byte[]? response, Form.Submitter submitter, World.Tx tx)
    {
        try
        {
            if (response is null)
            {
                FormCallbackRunner.Run(_onClose, submitter, tx);
                return;
            }

            using var document = JsonDocument.Parse(response);
            if (document.RootElement.ValueKind != JsonValueKind.Array)
            {
                return;
            }

            var values = new Dictionary<string, object?>(StringComparer.Ordinal);
            var options = new Dictionary<string, string[]>(StringComparer.Ordinal);

            for (var index = 0; index < _elements.Count && index < document.RootElement.GetArrayLength(); index++)
            {
                _elements[index].Read(document.RootElement[index], values, options);
            }

            FormCallbackRunner.Run(_onSubmit, new CustomFormResponse(values, options), submitter, tx);
        }
        catch (Exception exception)
        {
            FormCallbackRunner.Report(exception);
        }
    }

    public CustomForm AddLabel(string text)
    {
        _elements.Add(new TextElement(null, "label", text));
        return this;
    }

    public CustomForm AddHeader(string text)
    {
        _elements.Add(new TextElement(null, "header", text));
        return this;
    }

    public CustomForm AddDivider()
    {
        _elements.Add(new TextElement(null, "divider", string.Empty));
        return this;
    }

    public CustomForm AddInput(string name, string text, string defaultValue = "", string placeholder = "", string tooltip = "")
    {
        _elements.Add(new InputElement(name, text, defaultValue, placeholder, tooltip));
        return this;
    }

    public CustomForm AddToggle(string name, string text, bool defaultValue = false, string tooltip = "")
    {
        _elements.Add(new ToggleElement(name, text, defaultValue, tooltip));
        return this;
    }

    public CustomForm AddSlider(string name, string text, double min, double max, double step = 1, double defaultValue = 0, string tooltip = "")
    {
        _elements.Add(new SliderElement(name, text, min, max, step, defaultValue, tooltip));
        return this;
    }

    public CustomForm AddDropdown(string name, string text, string[] options, int defaultIndex = 0, string tooltip = "")
    {
        _elements.Add(new OptionsElement(name, "dropdown", "options", text, options, defaultIndex, tooltip));
        return this;
    }

    public CustomForm AddStepSlider(string name, string text, string[] options, int defaultIndex = 0, string tooltip = "")
    {
        _elements.Add(new OptionsElement(name, "step_slider", "steps", text, options, defaultIndex, tooltip));
        return this;
    }

    public CustomForm OnSubmit(Action<CustomFormResponse, Form.Submitter, World.Tx> onSubmit)
    {
        _onSubmit = onSubmit;
        return this;
    }

    public CustomForm OnClose(Action<Form.Submitter, World.Tx> onClose)
    {
        _onClose = onClose;
        return this;
    }
}
