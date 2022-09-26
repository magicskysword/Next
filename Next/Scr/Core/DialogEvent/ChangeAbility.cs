using System;
using SkySwordKill.Next.DialogSystem;

namespace SkySwordKill.Next.DialogEvent;

[DialogEvent("ChangeAbility")]
public class ChangeAbility : IDialogEvent
{
    public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
    {
        int num = command.GetInt(0);
        var player = Tools.instance.getPlayer();
        player.addWuXin(num);
        callback?.Invoke();
    }
}