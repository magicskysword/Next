using System;
using System.Linq;

namespace SkySwordKill.Next.DialogEvent
{
    [DialogEvent("RemoveCongenitalBuff")]
    public class RemoveCongenitalBuff : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            var player = env.player;
            int buffId = command.GetInt(0);
            var seid16 = 16.ToString();
            if (player.TianFuID.HasField(seid16))
            {
                var list = player.TianFuID[seid16].ToList();
                list.Remove(buffId);
                player.TianFuID[seid16] = new JSONObject(list.Select(buff => new JSONObject(buff)).ToArray());
            }

            callback?.Invoke();
        }
    }
}