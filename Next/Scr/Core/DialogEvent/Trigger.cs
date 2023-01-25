using System;
using SkySwordKill.Next.DialogSystem;

namespace SkySwordKill.Next.DialogEvent;

[DialogEvent("Trigger")]
public class Trigger : IDialogEvent
{
    public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
    {
        var triggerID = command.GetStr(0);
        if (DialogAnalysis.TryTriggerByID(triggerID, env))
        {
            return;
        }
        callback?.Invoke();
    }
}