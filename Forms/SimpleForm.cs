using System.Text.Json;
using Dragonfly;
using Toolbox.Forms.Elements;

namespace Toolbox.Forms;

public sealed class SimpleForm(string title) : Form.Value
{
    private readonly List<SimpleFormButton> _buttons = [];
    private Action<Form.Submitter, World.Tx>? _onClose;

    private string Title { get; set; } = title;

    private string Content { get; set; } = string.Empty;

    public byte[] MarshalJSON()
    {
        using var stream = new MemoryStream();
        using (var writer = new Utf8JsonWriter(stream))
        {
            writer.WriteStartObject();
            writer.WriteString("type", "form");
            writer.WriteString("title", Title);
            writer.WriteString("content", Content);
            writer.WritePropertyName("elements");
            writer.WriteStartArray();
            foreach (var button in _buttons)
            {
                button.Write(writer);
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
            if (!document.RootElement.TryGetInt32(out var index))
            {
                return;
            }

            if ((uint)index < (uint)_buttons.Count)
            {
                FormCallbackRunner.Run(_buttons[index].OnClick, submitter, tx);
            }
        }
        catch (Exception exception)
        {
            FormCallbackRunner.Report(exception);
        }
    }

    public SimpleForm SetTitle(string title)
    {
        Title = title;
        return this;
    }

    public SimpleForm SetContent(string content)
    {
        Content = content;
        return this;
    }

    public SimpleForm AddButton(string text, Action<Form.Submitter, World.Tx> onClick, string image = "")
    {
        _buttons.Add(new SimpleFormButton(text, image, onClick));
        return this;
    }

    public SimpleForm OnClose(Action<Form.Submitter, World.Tx> onClose)
    {
        _onClose = onClose;
        return this;
    }
}
