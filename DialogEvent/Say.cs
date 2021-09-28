using System;

namespace SkySwordKill.Next.DialogEvent
{
    public class Say : IDialogEvent
    {
        #region 字段



        #endregion

        #region 属性



        #endregion

        #region 回调方法

        public void Excute(DialogCommand command,DialogEnvironment env,Action callback)
        {
            int charNum;
            if (!command.bindEventData.character.TryGetValue(command.charID, out charNum))
            {
                if (!DialogAnalysis.tmpCharacter.TryGetValue(command.charID, out charNum))
                {
                    charNum = 0;
                }
            }
            DialogAnalysis.SetCharacter(charNum);
            DialogAnalysis.Say(command.say, () =>
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