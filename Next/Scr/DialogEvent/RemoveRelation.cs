using System;
using System.Collections.Generic;

namespace SkySwordKill.Next.DialogEvent
{
    /// <summary>
    /// 移除关系
    /// </summary>
    [DialogEvent("RemoveRelation")]
    public class RemoveRelation : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            var npcId = NPCEx.NPCIDToNew(command.GetInt(0));
            var relationType = command.GetInt(1);

            List<int> list = null;
            switch (relationType)
            {
                // 0 师傅
                case 0 when PlayerEx.IsTheather(npcId):
                    list = PlayerEx.Player.TeatherId.ToList();
                    list.Remove(npcId);
                    PlayerEx.Player.TeatherId = new JSONObject(JSONObject.Type.ARRAY);
                    foreach (int val in list)
                        PlayerEx.Player.TeatherId.Add(val);
                    break;
                // 1 徒弟
                case 1 when PlayerEx.IsTuDi(npcId):
                    list = PlayerEx.Player.TuDiId.ToList();
                    list.Remove(npcId);
                    PlayerEx.Player.TuDiId = new JSONObject(JSONObject.Type.ARRAY);
                    foreach (int val in list)
                        PlayerEx.Player.TuDiId.Add(val);
                    break;
                // 2 道侣
                case 2 when PlayerEx.IsDaoLv(npcId):
                    list = PlayerEx.Player.DaoLvId.ToList();
                    list.Remove(npcId);
                    PlayerEx.Player.DaoLvId = new JSONObject(JSONObject.Type.ARRAY);
                    foreach (int val in list)
                        PlayerEx.Player.DaoLvId.Add(val);
                    break;
                // 3 兄弟姐妹
                case 3 when PlayerEx.IsBrother(npcId):
                    list = PlayerEx.Player.Brother.ToList();
                    list.Remove(npcId);
                    PlayerEx.Player.Brother = new JSONObject(JSONObject.Type.ARRAY);
                    foreach (int val in list)
                        PlayerEx.Player.Brother.Add(val);
                    break;
            }
            
            callback?.Invoke();
        }
    }
}