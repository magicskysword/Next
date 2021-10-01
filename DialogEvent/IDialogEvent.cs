using System;

namespace SkySwordKill.Next.DialogEvent
{
    public interface IDialogEvent
    {
        void Execute(DialogCommand command,DialogEnvironment env,Action callback);
    }
}