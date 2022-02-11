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
            if (string.IsNullOrWhiteSpace(condition) || condition.Equals("true",StringComparison.OrdinalIgnoreCase))
            {
                DialogAnalysis.StartDialogEvent(id);
                return;
            }
            callback?.Invoke();
        }
    }
}