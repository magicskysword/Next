using System;

namespace SkySwordKill.Next.DialogEvent
{
    public interface IDialogEvent
    {
        void Excute(DialogCommand command,DialogEnvironment env,Action callback);
    }
}