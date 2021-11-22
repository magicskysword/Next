using System;

namespace SkySwordKill.Next.DialogEvent
{
    [DialogEvent("ChangeBaseSpirit")]
    public class ChangeBaseSpirit : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            int num = command.GetInt(0);
            var player = Tools.instance.getPlayer();
            player.addShenShi(num);
            callback?.Invoke();
        }
    }
}