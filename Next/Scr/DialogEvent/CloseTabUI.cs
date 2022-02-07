using System;
using Tab;

namespace SkySwordKill.Next.DialogEvent
{
    [DialogEvent("CloseTabUI")]
    public class CloseTabUI : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            if(TabUIMag.Instance != null)
                TabUIMag.Instance.TryEscClose();
            callback?.Invoke();
        }
    }
}