using System;

namespace SkySwordKill.Next.DialogEvent
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple = true)]
    public class DialogEventAttribute : Attribute
    {
        public string registerCommand;

        public DialogEventAttribute(string command)
        {
            registerCommand = command;
        }
    }
}