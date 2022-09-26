using System;
using SkySwordKill.Next.DialogSystem;

namespace SkySwordKill.Next.DialogEvent;

[DialogEvent("")]
[DialogEvent("Say")]
public class Say : IDialogEvent
{
    public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
    {
        var charId = command.GetStr(0);
        var say = command.GetStr(1);

        // 处理对话角色ID
        if (!command.BindEventData.Character.TryGetValue(charId, out var charNum))
        {
            if (!DialogAnalysis.TmpCharacter.TryGetValue(charId, out charNum))
            {
                charNum = 0;
            }
        }

        string text = DialogAnalysis.DealSayText(say, charNum);

        DialogAnalysis.SetCharacter(charNum);
        DialogAnalysis.Say(text, () => { callback?.Invoke(); });
    }
}