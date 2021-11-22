using System;

namespace SkySwordKill.Next.DialogEvent
{
    [DialogEvent("ChangeLife")]
    public class ChangeLife : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            int num = command.GetInt(0);
            var player = Tools.instance.getPlayer();
            player.addShoYuan(num);
            callback?.Invoke();
        }
    }
}