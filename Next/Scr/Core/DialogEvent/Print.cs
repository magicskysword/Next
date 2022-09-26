using System;
using SkySwordKill.Next.DialogSystem;

namespace SkySwordKill.Next.DialogEvent;

[DialogEvent("Print")]
public class Print : IDialogEvent
{
    public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
    {
        var printStr = string.Join("#", command.ParamList, 1, command.ParamList.Length - 1);
        var printType = command.GetStr(0, "Info");
        if (printType.Equals("Warning", StringComparison.OrdinalIgnoreCase))
        {
            Main.LogWarning(printStr);
        }
        else if (printType.Equals("Error", StringComparison.OrdinalIgnoreCase))
        {
            Main.LogError(printStr);
        }
        else
        {
            Main.LogInfo(printStr);
        }
            
        callback?.Invoke();
    }
}