using System;
using SkySwordKill.Next.DialogSystem;

namespace SkySwordKill.Next.DialogEvent;

[DialogEvent("SetFace")]
public class SetFace : IDialogEvent
{
    public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
    {
        int id = NPCEx.NPCIDToNew(command.GetInt(0));
        int faceId = command.GetInt(1);
        string workshopID = command.GetStr(2, "");

        if (id == 1)
        {
            // 主角
            PlayerEx.Player.Face = faceId;
            PlayerEx.Player.FaceWorkshop = workshopID;
            if (UIHeadPanel.Inst != null)
            {
                UIHeadPanel.Inst.Face.setFace();
            }
        }
        else
        {
            // NPC
            jsonData.instance.AvatarJsonData[id.ToString()].SetField("face", faceId);
            jsonData.instance.AvatarJsonData[id.ToString()].SetField("workshoplihui", workshopID);
            NpcJieSuanManager.inst.isUpDateNpcList = true;
            if (UINPCJiaoHu.Inst != null && UINPCJiaoHu.Inst.NowJiaoHuNPC != null)
            {
                UINPCJiaoHu.Inst.JiaoHuPop.RefreshUI();
            }
        }
            
        callback?.Invoke();
    }
}