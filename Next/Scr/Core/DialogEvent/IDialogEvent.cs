using System;
using SkySwordKill.Next.DialogSystem;

namespace SkySwordKill.Next.DialogEvent
{
    public interface IDialogEvent
    {
        void Execute(DialogCommand command,DialogEnvironment env,Action callback);
    }
}