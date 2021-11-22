using System;

namespace SkySwordKill.Next.DialogEvent
{
    [DialogEvent("ChangeInspiration")]
    public class ChangeInspiration : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            int num = command.GetInt(0);
            if (num >= 0)
            {
                Tools.instance.getPlayer().AddLingGan(num);
            }
            else
            {
                Tools.instance.getPlayer().ReduceLingGan(-num);
            }
            callback?.Invoke();
        }
    }
}