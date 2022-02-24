using System;

namespace SkySwordKill.Next.DialogEvent
{
    /// <summary>
    /// 修改NPC的神识
    /// </summary>
    [DialogEvent("ChangeNpcSprite")]
    public class ChangeNpcSprite: IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            int npcId = NPCEx.NPCIDToNew(command.GetInt(0));
            int addSprite = command.GetInt(1);

            var npcSprite = DialogAnalysis.GetNpcSprite(npcId);
            NPCEx.SetJSON(npcId, "shengShi", npcSprite + addSprite);
            
            callback?.Invoke();
        }
    }
}