using System;
using SkySwordKill.Next.DialogSystem;

namespace SkySwordKill.Next.DialogEvent;

/// <summary>
/// 修改NPC的寿元
/// </summary>
[DialogEvent("ChangeNpcLife")]
public class ChangeNpcLife : IDialogEvent
{

    public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
    {
        int npcId = NPCEx.NPCIDToNew(command.GetInt(0));
        int addLife = command.GetInt(1);

        var npcLife = DialogAnalysis.GetNpcLife(npcId);
        NPCEx.SetJSON(npcId, "shouYuan", npcLife + addLife);
            
        callback?.Invoke();
    }
}