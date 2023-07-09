using System;

namespace SkySwordKill.Next.DialogSystem;

[AttributeUsage(AttributeTargets.Class,AllowMultiple = true)]
public class DialogEnvQueryAttribute : Attribute
{
    public DialogEnvQueryAttribute(string method)
    {
        RegisterMethod = method;
    }

    public string RegisterMethod { get; set; }
}
    
public interface IDialogEnvQuery
{
    object Execute(DialogEnvQueryContext context);
}

public class DialogEnvQueryContext
{
    public DialogEnvQueryContext(DialogEnvironment env, object[] args)
    {
        Env = env;
        Args = args;
    }
        
    public DialogEnvironment Env { get; }
    public object[] Args { get; }
        
    public object GetArg(int index, object defaultValue = null)
    {
        if(index < 0 || index >= Args.Length)
            return defaultValue;
            
        return Args[index];
    }
        
    public T GetArg<T>(int index, T defaultValue = default(T))
    {
        if(index < 0 || index >= Args.Length)
            return defaultValue;
            
        if(Args[index] is T)
            return (T)Args[index];
            
        return defaultValue;
    }
}