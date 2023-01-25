using System;
using SkySwordKill.Next.DialogSystem;

namespace SkySwordKill.Next.DialogEvent;

[DialogEvent("TriggerAll")]
public class TriggerAll : IDialogEvent
{
    public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
    {
        var triggerType = command.GetStr(0).Split('|');
        var triggerAll = command.GetInt(1, 1) != 0;
        if (DialogAnalysis.TryTrigger(triggerType, env, triggerAll))
        {
            if (triggerAll)
            {
                DialogAnalysis.CompleteEvent();
            }
            return;
        }
        callback?.Invoke();
    }
}