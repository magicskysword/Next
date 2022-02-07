using System;
using System.Text;
using System.Text.RegularExpressions;

namespace SkySwordKill.Next.DialogEvent
{
    [DialogEvent("")]
    [DialogEvent("Say")]
    public class Say : IDialogEvent
    {
        public void Execute(DialogCommand command,DialogEnvironment env,Action callback)
        {
            int charNum;
            // 处理对话角色ID
            if (!command.bindEventData.character.TryGetValue(command.charID, out charNum))
            {
                if (!DialogAnalysis.TmpCharacter.TryGetValue(command.charID, out charNum))
                {
                    charNum = 0;
                }
            }
            string text = DialogAnalysis.DealSayText(command.say,charNum);

            DialogAnalysis.SetCharacter(charNum);
            DialogAnalysis.Say(text,() =>
            {
                callback?.Invoke();
            });
        }
    }
}