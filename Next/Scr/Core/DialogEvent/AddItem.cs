﻿using System;
using SkySwordKill.Next.DialogSystem;

namespace SkySwordKill.Next.DialogEvent;

[DialogEvent("AddItem")]
public class AddItem : IDialogEvent
{
    public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
    {
        int id = command.GetInt(0);
        int count = command.GetInt(1);
        bool showText = command.GetInt(2) != 0;
            
        Tools.instance.getPlayer().addItem(id,count,null,showText);
        MessageMag.Instance.Send(MessageName.MSG_PLAYER_USE_ITEM, null);
        callback?.Invoke();
    }
}