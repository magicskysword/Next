using System;
using SkySwordKill.Next.DialogSystem;

namespace SkySwordKill.Next.DialogEvent;

[DialogEvent("OpenGUI")]
public class OpenGUI : IDialogEvent
{
    public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
    {
        var guiRes = command.GetStr(0);
        var guiCom = command.GetStr(1);
        var guiScript = command.GetStr(2);
        var isModel = command.GetBool(3, true);
        var closeEvent = command.GetStr(4);

        Helper.OpenGUI(guiRes, guiCom, guiScript, isModel, () =>
        {
            if (!string.IsNullOrEmpty(closeEvent))
            {
                Helper.StartEvent(closeEvent, null);
            }
        });

        callback?.Invoke();
    }
}