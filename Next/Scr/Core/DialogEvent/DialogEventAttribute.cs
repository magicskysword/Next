using System;

namespace SkySwordKill.Next.DialogEvent;

[AttributeUsage(AttributeTargets.Class,AllowMultiple = true)]
public class DialogEventAttribute : Attribute
{
    public string RegisterCommand { get; set; }

    public DialogEventAttribute(string command)
    {
        RegisterCommand = command;
    }
}