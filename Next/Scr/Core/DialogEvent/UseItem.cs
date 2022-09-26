using System;
using Bag;
using SkySwordKill.Next.DialogSystem;

namespace SkySwordKill.Next.DialogEvent;

[DialogEvent("UseItem")]
public class UseItem : IDialogEvent
{
    public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
    {
        int id = command.GetInt(0);
        bool expendBagItem = command.GetBool(1);

        if (!expendBagItem && env.player.getItemNum(id) > 0)
        {
            env.player.addItem(id, 1, null, false);
        }
        var item = BaseItem.Create(id, 1, Tools.getUUID(), Tools.CreateItemSeid(id));
        item.Use();
        MessageMag.Instance.Send(MessageName.MSG_PLAYER_USE_ITEM, null);
            
        callback?.Invoke();
    }
        
}