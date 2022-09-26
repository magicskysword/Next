using System;
using SkySwordKill.Next.DialogSystem;

namespace SkySwordKill.Next.DialogEvent;

[DialogEvent("ChangeMoney")]
public class ChangeMoney : IDialogEvent
{
    public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
    {
        int num = command.GetInt(0);
        Tools.instance.getPlayer().AddMoney(num);
        callback?.Invoke();
    }
}