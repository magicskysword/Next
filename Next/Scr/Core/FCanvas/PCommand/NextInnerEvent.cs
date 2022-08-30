using Fungus;
using SkySwordKill.Next.DialogSystem;

namespace SkySwordKill.Next.FCanvas.PatchCommand
{
    [PCommandBinder("NextInnerEvent")]
    public class NextInnerEvent : PCommand
    {
        public string nextEvent;
        public string condition;
        
        public override void OnEnter()
        { 
            Main.LogInfo($"Fungus 跳转NextEvent: {nextEvent}");
            
            DialogEnvironment env = new DialogEnvironment();
            var roleID = GetFlowchart().GetVariable<IntegerVariable>("npcid")?.Value ?? 0;
            if (roleID != 0)
            {
                DialogAnalysis.BindNpc(env, roleID);
            }
            var canExecute = DialogAnalysis.CheckCondition(condition, env);
            if (canExecute)
            {
                DialogAnalysis.OnDialogComplete += Continue;
                DialogAnalysis.StartDialogEvent(nextEvent, env);
            }
            else
            {
                Continue();
            }
        }

        public override void OnInit(FPatchCommand fPatchCommand)
        {
            nextEvent = fPatchCommand.GetParamString(0);
            condition = fPatchCommand.GetParamString(1);
        }
    }
}