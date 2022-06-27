using System;

namespace SkySwordKill.Next.DialogEvent
{
    [DialogEvent("ChangeDrugsPoison")]
    public class ChangeDrugsPoison : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            int num = command.GetInt(0);
            if (num >= 0)
            {
                Tools.instance.getPlayer().AddDandu(num);
            }
            else
            {
                Tools.instance.getPlayer().ReduceDandu(-num);
            }
            callback?.Invoke();
        }
    }
}