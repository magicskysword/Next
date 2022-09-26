using System;
using SkySwordKill.Next.DialogSystem;

namespace SkySwordKill.Next.DialogEvent;

[DialogEvent("BindNpc")]
public class BindNpc : IDialogEvent
{
    public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
    {
        var npc = command.GetInt(0);
        if (npc != 0)
        {
            DialogAnalysis.BindNpc(env ,npc);
        }
        else
        {
            Main.LogError("无法绑定Npc，NpcId不能为0");
        }
        callback?.Invoke();
    }
}