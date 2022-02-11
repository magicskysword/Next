using System;

namespace SkySwordKill.Next.DialogEvent
{
    [DialogEvent("ShowTip")]
    public class ShowTip : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            var text = command.GetStr(0);
            var popType = (PopTipIconType)command.GetInt(1);
            
            UIPopTip.Inst.Pop(text, popType);
            
            callback?.Invoke();
        }
        
    }
}