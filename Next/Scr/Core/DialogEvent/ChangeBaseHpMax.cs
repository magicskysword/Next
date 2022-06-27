using System;

namespace SkySwordKill.Next.DialogEvent
{
    [DialogEvent("ChangeBaseHpMax")]
    public class ChangeBaseHpMax : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            int num = command.GetInt(0);
            Tools.instance.getPlayer()._HP_Max += num;
            callback?.Invoke();
        }
    }
}