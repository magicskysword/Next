using System;
using SkySwordKill.Next.DialogSystem;

namespace SkySwordKill.Next.DialogEvent;

[DialogEvent("ChangeComprehensionPoint")]
public class ChangeComprehensionPoint : IDialogEvent
{
    public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
    {
        int num = command.GetInt(0);
        var player = Tools.instance.getPlayer();
        player.WuDaoDian = Math.Max(0,player.WuDaoDian+num);
        callback?.Invoke();
    }
}