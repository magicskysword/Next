using System;
using SkySwordKill.Next.DialogSystem;

namespace SkySwordKill.Next.DialogEvent;

[DialogEvent("AddCongenitalBuff")]
public class AddCongenitalBuff : IDialogEvent
{
    public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
    {
        var player = env.player;
        int buffId = command.GetInt(0);
        var seid16 = 16.ToString();
        if (player.TianFuID.HasField(seid16))
        {
            player.TianFuID[seid16].Add(buffId);
        }
        else
        {
            player.TianFuID.SetField(seid16, new JSONObject(JSONObject.Type.ARRAY));
            player.TianFuID[seid16].Add(buffId);
        }
            
        callback?.Invoke();
    }
}