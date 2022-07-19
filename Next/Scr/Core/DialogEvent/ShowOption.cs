using System;
using SkySwordKill.Next.DialogSystem;

namespace SkySwordKill.Next.DialogEvent
{
    [DialogEvent("ShowOption")]
    [DialogEvent("SayOption")]
    public class ShowOption : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            var haveOption = false;
            
            var isSay = false;
            var optionStartIndex = 0;
            var optionCount = command.ParamList.Length;
            if (command.Command == "SayOption")
            {
                isSay = true;
                optionStartIndex += 2;
            }
            
            DialogAnalysis.ClearMenu();
            for (int i = optionStartIndex; i < optionCount; i++)
            {
                var optionArray = command.GetStr(i, "0::").Split(':');
                var optionID = int.Parse(optionArray[0]);
                var optionContent = optionArray[1];
                string condition;
                if (optionArray.Length > 2)
                    condition = optionArray[2];
                else
                    condition = string.Empty;

                if (DialogAnalysis.StringIsNullOrTrue(condition))
                {
                    haveOption = true;
                    DialogAnalysis.AddMenu(optionContent, () =>
                    {
                        env.optionID = optionID;
                        callback?.Invoke();
                    });
                }
            }

            if (isSay)
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
                if(haveOption)
                    DialogAnalysis.Say(text, () => { });
                else
                    DialogAnalysis.Say(text, () => { callback?.Invoke(); });
            }
            else
            {
                if(!haveOption)
                    callback?.Invoke();
            }
        }
    }
}