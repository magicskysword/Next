using System;
using SkySwordKill.Next.DialogSystem;

namespace SkySwordKill.Next.DialogEvent;

[DialogEvent("AddNpcItem")]
public class AddNpcItem : IDialogEvent
{
    public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
    {
        int npcId = NPCEx.NPCIDToNew(command.GetInt(0));
        int itemID = command.GetInt(1);
        int count = command.GetInt(2);

        NpcJieSuanManager.inst.AddItemToNpcBackpack(npcId, itemID, count, null, false);
        callback?.Invoke();
    }
}