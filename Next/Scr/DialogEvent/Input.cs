using System;

namespace SkySwordKill.Next.DialogEvent
{
    [DialogEvent("Input")]
    public class Input : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            var inputTip = command.GetStr(0);
            UInputBox.Show(inputTip, str =>
            {
                env.input = str;
                callback?.Invoke();
            });
        }
    }
}