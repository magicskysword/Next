using System;

namespace SkySwordKill.Next.DialogEvent
{
    [DialogEvent("ChangeTalent")]
    public class ChangeTalent : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            int num = command.GetInt(0);
            var player = Tools.instance.getPlayer();
            player.addZiZhi(num);
            callback?.Invoke();
        }
    }
}