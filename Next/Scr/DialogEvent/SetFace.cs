using System;

namespace SkySwordKill.Next.DialogEvent
{
    [DialogEvent("SetFace")]
    public class SetFace : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            int id = NPCEx.NPCIDToNew(command.GetInt(0));
            int faceId = command.GetInt(1);

            if (id == 1)
            {
                // 主角
                PlayerEx.Player.Face = faceId;
                if (UIHeadPanel.Inst != null)
                {
                    UIHeadPanel.Inst.Face.setFace();
                }
            }
            else
            {
                // NPC
                jsonData.instance.AvatarJsonData[id.ToString()].SetField("face", faceId);
                NpcJieSuanManager.inst.isUpDateNpcList = true;
                UINPCJiaoHu.Inst.JiaoHuPop.RefreshUI();
            }
            
            callback?.Invoke();
        }
    }
}