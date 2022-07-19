using System;
using SkySwordKill.Next.DialogSystem;

namespace SkySwordKill.Next.DialogEvent
{
    [DialogEvent("ChangeExp")]
    public class ChangeExp : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            int num = command.GetInt(0);
            Tools.instance.getPlayer().addEXP(num);
            MessageMag.Instance.Send(MessageName.MSG_PLAYER_USE_ITEM, null);
            callback?.Invoke();
        }
    }
}