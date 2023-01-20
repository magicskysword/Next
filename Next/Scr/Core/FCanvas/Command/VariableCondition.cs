using Fungus;

namespace SkySwordKill.Next.FCanvas.FakerCommand;

public class VariableCondition : FCommand
{
    public string Condition;
        
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
        
}
    
[FCommandBinder(typeof(Fungus.ElseIf))]
public class ElseIf : VariableCondition
{
        
}
    
[FCommandBinder(typeof(Fungus.Else))]
public class Else : FCommand
{
        
}