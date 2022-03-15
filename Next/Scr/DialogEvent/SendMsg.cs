using System;

namespace SkySwordKill.Next.DialogEvent
{
    [DialogEvent("SendMsg")]
    public class SendMsg : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            var msgID = command.GetInt(0);
            env.player.chuanYingManager.addChuanYingFu(msgID);
            env.player.updateChuanYingFu();
            callback?.Invoke();
        }
    }
}