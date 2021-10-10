using System;

namespace SkySwordKill.Next.DialogEvent
{
    [DialogEvent("ChangeExp")]
    public class ChangeExp : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            int num = command.GetInt(0);
            Tools.instance.getPlayer().AddHp(num);
            callback?.Invoke();
        }
    }
}