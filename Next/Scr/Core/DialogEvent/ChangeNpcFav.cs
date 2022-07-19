using System;
using SkySwordKill.Next.DialogSystem;

namespace SkySwordKill.Next.DialogEvent
{
    /// <summary>
    /// 修改NPC好感度
    /// Change Npc Favorability
    /// </summary>
    [DialogEvent("ChangeNpcFav")]
    public class ChangeNpcFav : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            int npcId = command.GetInt(0);
            int favValue = command.GetInt(1);
            bool addHumanFeeling = command.GetBool(2);
            bool showTip = command.GetBool(3);
            NPCEx.AddFavor(npcId, favValue, addHumanFeeling, showTip);
            callback?.Invoke();
        }
    }
}