using Dragonfly;
using Toolbox.Forms;
using Toolbox.Inventories;
using Toolbox.Items;
using Toolbox.Players;
using Toolbox.Presentation;
using Toolbox.Sounds;

namespace ToolboxExample;

internal static class ExampleForms
{
    public static void OpenMain(Player player, ExampleState state)
    {
        var form = FormFactory.CreateSimpleForm("Toolbox")
            .SetContent("Choisis une fonctionnalite a tester.")
            .AddButton("Items + NBT", (_, _) => ExampleCommands.GiveItems(player))
            .AddButton("UI", (_, _) => ExampleCommands.ShowUi(player))
            .AddButton("Custom form", (_, _) => OpenSettings(player, state))
            .AddButton("Fermer", (submitter, _) =>
            {
                if (submitter is Player submittedPlayer)
                {
                    FormFactory.CloseForm(submittedPlayer);
                }
            })
            .OnClose((submitter, _) =>
            {
                if (submitter is Player submittedPlayer)
                {
                    PlayerApi.SendTip(submittedPlayer, "Form fermee.");
                }
            });

        FormFactory.SendForm(player, form);
    }

    private static void OpenSettings(Player player, ExampleState state)
    {
        var form = FormFactory.CreateCustomForm("Toolbox settings")
            .AddHeader("Etat du plugin")
            .AddLabel($"Derniere connexion: {state.LastConnection}")
            .AddToggle("packet_log", "Logger les packets", state.PacketLogEnabled)
            .AddSlider("speed", "Vitesse", 0.1, 1.0, 0.1, PlayerApi.GetSpeed(player))
            .AddDropdown("sound", "Son de test", ["LevelUp", "Click", "Pop"], 0)
            .OnSubmit((response, submitter, _) =>
            {
                if (submitter is not Player submittedPlayer)
                {
                    return;
                }

                state.SetPacketLogEnabled(response.GetBool("packet_log"));
                PlayerApi.SetSpeed(submittedPlayer, response.GetFloat("speed", PlayerApi.GetSpeed(submittedPlayer)));
                PlaySelectedSound(submittedPlayer, response.GetSelectedOption("sound", "LevelUp"));
                ToastApi.SendToast(submittedPlayer, "Toolbox", $"Packet log: {state.PacketLogEnabled}");
            });

        FormFactory.SendForm(player, form);
    }

    public static void OpenConfirmReset(Player player, ExampleState state)
    {
        var form = FormFactory.CreateModalForm("Toolbox task", "Arreter la task repetitive ?")
            .SetButton1("Oui", (_, _) =>
            {
                state.StopRepeatingTask();
                PlayerApi.SendMessage(player, "Task repetitive arretee.");
            })
            .SetButton2("Non", (_, _) => PlayerApi.SendMessage(player, "Task conservee."));

        FormFactory.SendForm(player, form);
    }

    private static void PlaySelectedSound(Player player, string sound)
    {
        var value = sound switch
        {
            "Click" => ToolboxSound.Click,
            "Pop" => ToolboxSound.Pop,
            _ => ToolboxSound.LevelUp,
        };

        SoundApi.PlaySound(player, value);
    }
}
