using System;

namespace SkySwordKill.Next.DialogEvent
{
    public class SetChar : IDialogEvent
    {
        #region 字段



        #endregion

        #region 属性



        #endregion

        #region 回调方法

        public void Excute(DialogCommand command,DialogEnvironment env,Action callback)
        {
            if (command.paramList.Length >= 2)
            {
                var name = command.paramList[0];
                var expression = command.paramList[1];
                var id = DialogAnalysis.GetEvaluate(env).Evaluate<int>(expression);
                DialogAnalysis.TryAddTmpChar(name,id);
            }
            callback?.Invoke();
        }

        #endregion

        #region 公共方法



        #endregion

        #region 私有方法



        #endregion


        
    }
}