﻿using System;

namespace SkySwordKill.Next.DialogEvent
{
    [DialogEvent("ChangeHp")]
    public class ChangeHp : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            int num = command.GetInt(0);
            Tools.instance.getPlayer().AddHp(num);
            MessageMag.Instance.Send(MessageName.MSG_PLAYER_USE_ITEM, null);
            callback?.Invoke();
        }
    }
}