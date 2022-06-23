﻿using System;

namespace SkySwordKill.Next.DialogEvent
{
    [DialogEvent("SetCustomFace")]
    public class SetCustomFace : IDialogEvent
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
                DialogAnalysis.SetNpcFaceData(id, faceId);
                NpcJieSuanManager.inst.isUpDateNpcList = true;
                UINPCJiaoHu.Inst.JiaoHuPop.RefreshUI();
            }
            
            callback?.Invoke();
        }
    }
}