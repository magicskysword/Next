using System;
using SkySwordKill.Next.DialogSystem;

namespace SkySwordKill.Next.DialogEvent;

[DialogEvent("AddComprehensionExp")]
public class AddComprehensionExp : IDialogEvent
{
    public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
    {
        int type = command.GetInt(0);
        int num = command.GetInt(1);
        Tools.instance.getPlayer().wuDaoMag.addWuDaoEx(type,num);
        callback?.Invoke();
    }
}