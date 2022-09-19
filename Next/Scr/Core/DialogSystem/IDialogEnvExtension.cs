using System;

namespace SkySwordKill.Next.DialogSystem
{
    public class DialogEnvExtensionAttribute : Attribute
    {
        public DialogEnvExtensionAttribute(string method)
        {
            RegisterMethod = method;
        }

        public string RegisterMethod { get; set; }
    }
    
    public interface IDialogEnvExtension
    {
        object Execute(DialogEnvironment env, string[] args);
    }
}