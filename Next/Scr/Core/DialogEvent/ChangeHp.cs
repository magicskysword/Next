using System;
using SkySwordKill.Next.DialogSystem;

namespace SkySwordKill.Next.DialogEvent;

[DialogEvent("ChangeHp")]
public class ChangeHp : IDialogEvent
{
    public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
    {
        int num = command.GetInt(0);
        env.player.HP = Math.Min(env.player.HP + num, env.player.HP_Max);
        MessageMag.Instance.Send(MessageName.MSG_PLAYER_USE_ITEM, null);
        if (env.player.HP <= 0)
        {
            UIDeath.Inst.Show(DeathType.身死道消);
        }
        else
        {
            callback?.Invoke();
        }
    }
}