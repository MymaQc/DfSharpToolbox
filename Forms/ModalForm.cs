using System.Text.Json;
using Dragonfly;

namespace Toolbox.Forms;

public sealed class ModalForm(string title, string content = "") : Form.Value
{
    private Action<Form.Submitter, World.Tx>? _onClose;
    private Action<Form.Submitter, World.Tx>? _onNo;
    private Action<Form.Submitter, World.Tx>? _onYes;

    private string Title { get; } = title;

    private string Content { get; set; } = content;

    private string YesButton { get; set; } = "gui.yes";

    private string NoButton { get; set; } = "gui.no";

    public byte[] MarshalJSON()
    {
        using var stream = new MemoryStream();
        using (var writer = new Utf8JsonWriter(stream))
        {
            writer.WriteStartObject();
            writer.WriteString("type", "modal");
            writer.WriteString("title", Title);
            writer.WriteString("content", Content);
            writer.WriteString("button1", YesButton);
            writer.WriteString("button2", NoButton);
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
            switch (document.RootElement.ValueKind)
            {
                case JsonValueKind.True:
                    FormCallbackRunner.Run(_onYes, submitter, tx);
                    break;
                case JsonValueKind.False:
                    FormCallbackRunner.Run(_onNo, submitter, tx);
                    break;
                case JsonValueKind.Undefined:
                case JsonValueKind.Object:
                case JsonValueKind.Array:
                case JsonValueKind.String:
                case JsonValueKind.Number:
                case JsonValueKind.Null:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        catch (Exception exception)
        {
            FormCallbackRunner.Report(exception);
        }
    }

    public ModalForm SetContent(string content)
    {
        Content = content;
        return this;
    }

    public ModalForm SetButton1(string text, Action<Form.Submitter, World.Tx> onSubmit)
    {
        YesButton = text;
        _onYes = onSubmit;
        return this;
    }

    public ModalForm SetButton2(string text, Action<Form.Submitter, World.Tx> onSubmit)
    {
        NoButton = text;
        _onNo = onSubmit;
        return this;
    }

    public ModalForm OnClose(Action<Form.Submitter, World.Tx> onClose)
    {
        _onClose = onClose;
        return this;
    }
}
