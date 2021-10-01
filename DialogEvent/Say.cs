using System;
using System.Text;
using System.Text.RegularExpressions;

namespace SkySwordKill.Next.DialogEvent
{
    public class Say : IDialogEvent
    {
        #region 字段



        #endregion

        #region 属性



        #endregion

        #region 回调方法

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
            // 处理对话文本
            StringBuilder finallyText = new StringBuilder(text);
            Regex regex = new Regex(@"\[&(?<expression>[\s\S]*?)&]");
            
            var matches = regex.Matches(text);
            var evaluate = DialogAnalysis.GetEvaluate(env);
            foreach(Match match in matches)
            {
                var expression = match.Groups["expression"].Value;
                var getValue = evaluate.Evaluate(expression).ToString();
                finallyText.Replace(match.Value, getValue);
            }
            
            DialogAnalysis.SetCharacter(charNum);
            DialogAnalysis.Say(finallyText.ToString(), () =>
            {
                callback?.Invoke();
            });
        }

        #endregion

        #region 公共方法



        #endregion

        #region 私有方法



        #endregion


        
    }
}