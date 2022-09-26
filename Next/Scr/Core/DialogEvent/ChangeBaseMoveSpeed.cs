using System;
using SkySwordKill.Next.DialogSystem;

namespace SkySwordKill.Next.DialogEvent;

[DialogEvent("ChangeBaseMoveSpeed")]
public class ChangeBaseMoveSpeed : IDialogEvent
{
    public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
    {
        int num = command.GetInt(0);
        env.player._dunSu += num;
        callback?.Invoke();
    }
}