using System;
using SkySwordKill.Next.DialogSystem;

namespace SkySwordKill.Next.DialogEvent
{
    [DialogEvent("Death")]
    public class Death : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            var deathType = (DeathType)command.GetInt(0, 1);
            UIDeath.Inst.Show(deathType);
            DialogAnalysis.CancelEvent();
        }
    }
}