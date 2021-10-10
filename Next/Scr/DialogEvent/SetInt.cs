using System;

namespace SkySwordKill.Next.DialogEvent
{
    [DialogEvent("SetInt")]
    public class SetInt : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            string key = command.GetStr(0);
            int value = command.GetInt(1);
            DialogAnalysis.SetInt(key,value);
            callback?.Invoke();
        }
    }
}