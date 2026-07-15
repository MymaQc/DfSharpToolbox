using Dragonfly;

namespace Toolbox.Forms;

public static class FormFactory
{
    public static SimpleForm CreateSimpleForm(string title)
    {
        return new SimpleForm(title);
    }

    public static ModalForm CreateModalForm(string title, string content = "")
    {
        return new ModalForm(title, content);
    }

    public static CustomForm CreateCustomForm(string title)
    {
        return new CustomForm(title);
    }

    public static void SendForm(Player player, Form.Value form)
    {
        player.SendForm(form);
    }

    public static void CloseForm(Player player)
    {
        player.CloseForm();
    }
}
