using System;
using UnityEngine;

namespace SkySwordKill.Next.DialogEvent
{
    [DialogEvent("ChangeAge")]
    public class ChangeAge : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            int num = command.GetInt(0);
            var player = Tools.instance.getPlayer();
            player.age = (uint)Mathf.Clamp((int)player.age + num, 0, (int)player.shouYuan);
            callback?.Invoke();
        }
    }
}