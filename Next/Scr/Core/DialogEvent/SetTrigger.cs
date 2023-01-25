using System;
using SkySwordKill.Next.DialogSystem;

namespace SkySwordKill.Next.DialogEvent;

[DialogEvent("SetTrigger")]
public class SetTrigger : IDialogEvent
{
    public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
    {
        var triggerID = command.GetStr(0);
        var on = command.GetInt(1, 1) != 0;
        DialogAnalysis.AvatarNextData.SetTrigger(triggerID, on);
        callback?.Invoke();
    }
}