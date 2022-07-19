using System;
using SkySwordKill.Next.DialogSystem;

namespace SkySwordKill.Next.DialogEvent
{
    /// <summary>
    /// 添加关系
    /// </summary>
    [DialogEvent("AddRelation")]
    public class AddRelation : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            var npcId = NPCEx.NPCIDToNew(command.GetInt(0));
            var relationType = command.GetInt(1);
            
            switch (relationType)
            {
                // 0 师傅
                case 0 when !PlayerEx.IsTheather(npcId):
                    PlayerEx.Player.TeatherId.Add(npcId);
                    break;
                // 1 徒弟
                case 1 when !PlayerEx.IsTuDi(npcId):
                    PlayerEx.Player.TuDiId.Add(npcId);
                    break;
                // 2 道侣
                case 2 when !PlayerEx.IsDaoLv(npcId):
                    PlayerEx.Player.DaoLvId.Add(npcId);
                    break;
                // 3 兄弟姐妹
                case 3 when !PlayerEx.IsBrother(npcId):
                    PlayerEx.Player.Brother.Add(npcId);
                    break;
            }
            
            callback?.Invoke();
        }
    }
}