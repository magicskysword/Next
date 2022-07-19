using Fungus;
using SkySwordKill.Next.DialogSystem;
using UnityEngine.Events;

namespace SkySwordKill.Next.FCanvas.PatchCommand
{
    [PCommandBinder("NextMenu")]
    public class NextMenu : PCommand
    {
        public string menuName;

        public string tagEvent;
        
        public string condition;

        public override void OnEnter()
        {
            var roleID = GetFlowchart().GetVariable<IntegerVariable>("npcid")?.Value ?? 0;
            DialogEnvironment env;
            if (roleID != 0)
            {
                var npc = new UINPCData(roleID);
                env = new DialogEnvironment()
                {
                    bindNpc = npc,
                    roleBindID = npc.ZhongYaoNPCID,
                    roleID = npc.ID,
                    roleName = npc.Name
                };
            }
            else
            {
                env = new DialogEnvironment();
            }

            env.flowchart = GetFlowchart();
            
            
            bool showOption = DialogAnalysis.CheckCondition(condition, env);
            MenuDialog menuDialog = MenuDialog.GetMenuDialog();
            if (menuDialog != null && showOption)
            {
                menuDialog.SetActive(true);
                string text = DialogAnalysis.AnalysisInlineScript(menuName, env);
                Main.LogDebug($"载入额外选项: {text} => {tagEvent}");
                DialogAnalysis.AddMenu(text, () =>
                {
                    if(!string.IsNullOrEmpty(tagEvent))
                        DialogAnalysis.StartDialogEvent(tagEvent, env);
                    else
                        DialogAnalysis.CompleteEvent();
                });
            }
            Continue();
        }

        public override void OnInit(FPatchCommand fPatchCommand)
        {
            menuName = fPatchCommand.GetParamString(0);
            tagEvent = fPatchCommand.GetParamString(1);
        }

    }
}