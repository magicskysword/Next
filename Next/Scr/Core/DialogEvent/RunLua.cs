using System;
using SkySwordKill.Next.DialogSystem;

namespace SkySwordKill.Next.DialogEvent;

[DialogEvent("RunLua")]
public class RunLua : IDialogEvent
{
    public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
    {
        var scr = command.GetStr(0);
        var funcName = command.GetStr(1);
        Main.Lua.RunFunc("libs/dialog", "runEvent",
            new object[] { scr, funcName, command, env, callback });
    }
}