using Fungus;
using SkySwordKill.Next.DialogSystem;

namespace SkySwordKill.Next.FCanvas.PatchCommand;

[PCommandBinder("NextEvent")]
public class NextEvent : PCommand
{
    public string nextEvent;
    public string condition;
        
    public override void OnEnter()
    {
        var env = new DialogEnvironment()
        {
            flowchart = GetFlowchart()
        };
        var roleID = GetFlowchart().GetVariable<IntegerVariable>("npcid")?.Value ?? 0;
        if (roleID != 0)
        {
            DialogAnalysis.BindNpc(env, roleID);
        }
            
        var canExecute = DialogAnalysis.CheckCondition(condition, env);
        if (canExecute)
        {
            StopParentBlock();
            Main.LogDebug($"Fungus 满足条件{condition} 跳转NextEvent: {nextEvent}");
            DialogAnalysis.StartDialogEvent(nextEvent, env);
        }
        else
        {
            Main.LogDebug($"Fungus 不满足条件{condition} 继续执行指令");
            Continue();
        }
    }

    public override void OnInit(FPatchCommand fPatchCommand)
    {
        nextEvent = fPatchCommand.GetParamString(0);
        condition = fPatchCommand.GetParamString(1);
    }
}