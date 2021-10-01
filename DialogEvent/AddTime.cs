using System;
using System.Linq;

namespace SkySwordKill.Next.DialogEvent
{
    [DialogEvent("AddTime")]
    public class AddTime : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            int year = command.GetInt(0);
            int month = command.GetInt(1);
            int day = command.GetInt(2);
            // PS: 我本来不想吐槽，可是这里看到这个函数签名真的忍不住了......
            // 实在想不通怎么会写出 AddTime(int addday, int addMonth = 0, int Addyear = 0) 这种大小写全都有的命名......
            Tools.instance.getPlayer().AddTime(year,month,day);
            callback?.Invoke();
        }
    }
}