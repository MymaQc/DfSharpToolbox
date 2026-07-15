using Dragonfly;

namespace Toolbox.Forms;

public static class FormFactory
{
    public static SimpleForm Simple(string title)
    {
        return new SimpleForm(title);
    }

    public static ModalForm Modal(string title, string content = "")
    {
        return new ModalForm(title, content);
    }

    public static CustomForm Custom(string title)
    {
        return new CustomForm(title);
    }

    public static void Send(Player player, Form.Value form)
    {
        player.SendForm(form);
    }
}
