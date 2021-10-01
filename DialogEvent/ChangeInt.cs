using System;

namespace SkySwordKill.Next.DialogEvent
{
    [DialogEvent("ChangeInt")]
    public class ChangeInt : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            string key = command.GetStr(0);
            int value = DialogAnalysis.GetInt(key) + command.GetInt(1);
            DialogAnalysis.SetInt(key,value);
            callback?.Invoke();
        }
    }
}