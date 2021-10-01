using System;

namespace SkySwordKill.Next.DialogEvent
{
    [DialogEvent("SetChar")]
    public class SetChar : IDialogEvent
    {
        public void Execute(DialogCommand command,DialogEnvironment env,Action callback)
        {
            string name = command.GetStr(0);
            int id = command.GetInt(1);
            DialogAnalysis.TryAddTmpChar(name,id);
            callback?.Invoke();
        }
    }
}