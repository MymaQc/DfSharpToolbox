using Dragonfly;
using Toolbox.Diagnostics;

namespace Toolbox.Forms;

internal static class FormCallbackRunner
{
    public static void Run(Action<Form.Submitter, World.Tx>? callback, Form.Submitter submitter, World.Tx tx)
    {
        if (callback is null)
        {
            return;
        }

        try
        {
            callback(submitter, tx);
        }
        catch (Exception exception)
        {
            Report(exception);
        }
    }

    public static void Run<T>(Action<T, Form.Submitter, World.Tx>? callback, T value, Form.Submitter submitter, World.Tx tx)
    {
        if (callback is null)
        {
            return;
        }

        try
        {
            callback(value, submitter, tx);
        }
        catch (Exception exception)
        {
            Report(exception);
        }
    }

    public static void Report(Exception exception)
    {
        ToolboxLogger.Error(exception, "Form callback");
    }
}
