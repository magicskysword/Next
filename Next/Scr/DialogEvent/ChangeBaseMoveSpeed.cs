using System;

namespace SkySwordKill.Next.DialogEvent
{
    [DialogEvent("ChangeBaseMoveSpeed")]
    public class ChangeBaseMoveSpeed : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            int num = command.GetInt(0);
            var player = Tools.instance.getPlayer();
            player._dunSu += num;
            callback?.Invoke();
        }
    }
}