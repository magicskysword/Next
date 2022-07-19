using System;
using SkySwordKill.Next.DialogSystem;
using UnityEngine;

namespace SkySwordKill.Next.DialogEvent
{
    [DialogEvent("ChangeAge")]
    public class ChangeAge : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            int num = command.GetInt(0);
            env.player.age = (uint)Mathf.Clamp((int)env.player.age + num, 0, (int)env.player.shouYuan);
            callback?.Invoke();
        }
    }
}