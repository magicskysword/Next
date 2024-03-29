﻿using Fungus;
using SkySwordKill.Next.DialogSystem;

namespace SkySwordKill.Next.FCanvas.PatchCommand;

[PCommandBinder("NextMenu")]
public class NextMenu : PCommand
{
    public string menuName;

    public string tagEvent;
        
    public string condition;

    public override void OnEnter()
    {
        DialogEnvironment env = new DialogEnvironment();
        var roleID = GetFlowchart().GetVariable<IntegerVariable>("npcid")?.Value ?? 0;
        if (roleID != 0)
        {
            DialogAnalysis.BindNpc(env, roleID);
        }

        env.flowchart = GetFlowchart();
            
            
        bool showOption = DialogAnalysis.CheckCondition(condition, env);
        MenuDialog menuDialog = MenuDialog.GetMenuDialog();
        if (menuDialog != null && showOption)
        {
            menuDialog.SetActive(true);
            string text = DialogAnalysis.AnalysisInlineScript(menuName, env);
            Main.LogDebug($"Fungus 满足条件{condition} 载入额外选项: {text} => {tagEvent}");
            DialogAnalysis.AddMenu(text, () =>
            {
                if(!string.IsNullOrEmpty(tagEvent))
                    DialogAnalysis.StartDialogEvent(tagEvent, env);
                else
                    DialogAnalysis.CompleteEvent();
            });
        }
        else
        {
            Main.LogDebug($"Fungus 不满足条件{condition} 额外选项不显示");
        }
        Continue();
    }

    public override void OnInit(FPatchCommand fPatchCommand)
    {
        menuName = fPatchCommand.GetParamString(0);
        tagEvent = fPatchCommand.GetParamString(1);
        condition = fPatchCommand.GetParamString(2);
    }

}