using System;
using JSONClass;
using SkySwordKill.Next.DialogSystem;

namespace SkySwordKill.Next.DialogEvent;

[DialogEvent("RemoveItem")]
public class RemoveItem : IDialogEvent
{
    public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
    {
        int id = command.GetInt(0);
        int count = command.GetInt(1);
        bool showText = command.GetInt(2) != 0;
            
        Tools.instance.getPlayer().removeItem(id,count);
        if(showText)
        {
            _ItemJsonData itemJsonData = _ItemJsonData.DataDict[id];
            UIPopTip.Inst.Pop($"失去物品{itemJsonData.name}x{count}",
                PopTipIconType.包裹);
        }
        MessageMag.Instance.Send(MessageName.MSG_PLAYER_USE_ITEM, null);
        callback?.Invoke();
    }
}