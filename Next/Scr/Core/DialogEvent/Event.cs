using System;

namespace SkySwordKill.Next.DialogEvent
{
    [DialogEvent("Event")]
    public class Event : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            var id = command.GetStr(0);
            var condition = command.GetStr(1);
            if (DialogAnalysis.StringIsNullOrTrue(condition))
            {
                DialogAnalysis.SwitchDialogEvent(id);
                return;
            }
            callback?.Invoke();
        }
    }
}