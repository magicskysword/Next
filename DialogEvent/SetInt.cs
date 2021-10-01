using System;

namespace SkySwordKill.Next.DialogEvent
{
    public class SetInt : IDialogEvent
    {
        #region 字段



        #endregion

        #region 属性



        #endregion

        #region 回调方法

        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            string key = command.paramList[0];
            string expression = command.paramList[1];
            int value = DialogAnalysis.GetEvaluate(env).Evaluate<int>(expression);
            DialogAnalysis.SetInt(key,value);
            callback?.Invoke();
        }

        #endregion

        #region 公共方法



        #endregion

        #region 私有方法



        #endregion
    }
}