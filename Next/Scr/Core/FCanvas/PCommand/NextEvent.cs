using System.Collections;
using Fungus;
using SkySwordKill.Next.DialogSystem;

namespace SkySwordKill.Next.FCanvas.PatchCommand
{
    [PCommandBinder("NextEvent")]
    public class NextEvent : PCommand
    {
        public string nextEvent;
        public string condition;
        
        public override void OnEnter()
        { 
            Main.LogInfo($"Fungus 跳转NextEvent: {nextEvent}");
            
            var env = new DialogEnvironment()
            {
                flowchart = GetFlowchart()
            };
            var canExecute = DialogAnalysis.CheckCondition(condition, env);
            if (canExecute)
            {
                DialogAnalysis.StartDialogEvent(nextEvent, env);
                StopParentBlock();
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