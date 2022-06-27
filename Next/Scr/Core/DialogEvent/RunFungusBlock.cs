using System;

namespace SkySwordKill.Next.DialogEvent
{
    [DialogEvent("RunFungusBlock")]
    public class RunFungusBlock : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            var tagBlock = command.GetStr(0);
            if (env.flowchart == null)
            {
                Main.LogError("FungusEvent : 对应flowchart不存在");
            }
            else
            {
                env.flowchart.ExecuteBlock(tagBlock);
            }
            DialogAnalysis.CancelEvent();
        }
    }
}