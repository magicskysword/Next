using System;

namespace SkySwordKill.Next.DialogEvent
{
    [DialogEvent("Nothing")]
    public class Nothing : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            callback?.Invoke();
        }
    }
}