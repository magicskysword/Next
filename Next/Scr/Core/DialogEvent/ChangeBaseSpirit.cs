using System;
using SkySwordKill.Next.DialogSystem;

namespace SkySwordKill.Next.DialogEvent;

[DialogEvent("ChangeBaseSpirit")]
public class ChangeBaseSpirit : IDialogEvent
{
    public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
    {
        int num = command.GetInt(0);
        env.player.addShenShi(num);
        callback?.Invoke();
    }
}