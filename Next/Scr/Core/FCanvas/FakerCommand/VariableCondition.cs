using Fungus;

namespace SkySwordKill.Next.FCanvas.FakerCommand;

public class VariableCondition : FCommand
{
    public string Condition { get; set; }
        
    public override void ReadCommand(Command command)
    {
        base.ReadCommand(command);
        var cmdCondition = (Fungus.VariableCondition)command;
        Condition = cmdCondition.GetSummary();
    }
}
    
[FCommandBinder(typeof(Fungus.If))]
public class If : VariableCondition
{
    public override string GetSummary()
    {
        return $"if ({Condition})";
    }
}
    
[FCommandBinder(typeof(Fungus.ElseIf))]
public class ElseIf : VariableCondition
{
    public override string GetSummary()
    {
        return $"else if ({Condition})";
    }
}
    
[FCommandBinder(typeof(Fungus.Else))]
public class Else : FCommand
{
    public override string GetSummary()
    {
        return "else";
    }
}

[FCommandBinder(typeof(Fungus.End))]
public class End : FCommand
{
    public bool IsLoop { get; set; }
    
    public override void ReadCommand(Command command)
    {
        base.ReadCommand(command);
        var cmdEnd = (Fungus.End)command;
        IsLoop = cmdEnd.Loop;
    }

    public override string GetSummary()
    {
        return "end";
    }
}

[FCommandBinder(typeof(Fungus.While))]
public class While : If
{
    public override string GetSummary()
    {
        return $"while ({Condition})";
    }
}