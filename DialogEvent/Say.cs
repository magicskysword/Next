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
            string text = command.say;
            // 处理对话角色ID
            if (!command.bindEventData.character.TryGetValue(command.charID, out charNum))
            {
                if (!DialogAnalysis.tmpCharacter.TryGetValue(command.charID, out charNum))
                {
                    charNum = 0;
                }
            }

            DialogAnalysis.SetCharacter(charNum);
            DialogAnalysis.Say(text.ToString(), () =>
            {
                callback?.Invoke();
            });
        }
    }
}