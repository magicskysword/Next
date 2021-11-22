using System;

namespace SkySwordKill.Next.DialogEvent
{
    [DialogEvent("ChangeMentality")]
    public class ChangeMentality : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            int num = command.GetInt(0);
            Tools.instance.getPlayer().xinjin += num;
            callback?.Invoke();
        }
    }
}