using System;
using SkySwordKill.Next.DialogSystem;

namespace SkySwordKill.Next.DialogEvent
{
    [DialogEvent("RunFungusBlock")]
    public class RunFungusBlock : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            var tagBlock = command.GetStr(0);
            DialogAnalysis.CancelEvent();
            if (env.flowchart == null)
            {
                Main.LogError("FungusEvent : 对应flowchart不存在");
            }
            else
            {
                Main.LogInfo("FungusEvent : 跳转FungusBlock " + tagBlock);
                env.flowchart.ExecuteBlock(tagBlock);
            }
        }
    }
}