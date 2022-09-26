using System;
using SkySwordKill.Next.DialogSystem;

namespace SkySwordKill.Next.DialogEvent;

[DialogEvent("SetStr")]
public class SetStr : IDialogEvent
{
    public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
    {
        string key = command.GetStr(0);
        string value = command.GetStr(1);
        DialogAnalysis.SetStr(key,value);
        callback?.Invoke();
    }
}