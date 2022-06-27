using System;

namespace SkySwordKill.Next.DialogEvent
{
    [DialogEvent("HideCG")]
    public class HideCG : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            CGManager.Instance.Enable = false;
            callback?.Invoke();
        }
    }
}