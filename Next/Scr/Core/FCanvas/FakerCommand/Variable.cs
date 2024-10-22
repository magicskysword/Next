using Fungus;
using SkySwordKill.Next.Utils;

namespace SkySwordKill.Next.FCanvas.FakerCommand;

[FCommandBinder(typeof(Fungus.SetVariable))]
public class SetValue : FCommand
{
    public string Operator { get; set; } = string.Empty;

    public override void ReadCommand(Command command)
    {
        base.ReadCommand(command);
        var cmdSerVariable = (Fungus.SetVariable)command;
        Operator = cmdSerVariable.GetSummary();
    }

    public override string GetSummary()
    {
        return $"设置变量[{Operator}]";
    }
}

[FCommandBinder(typeof(Fungus.SetStaticValue))]
public class SetStaticValue : FCommand
{
    public int VariableID { get; set; }
    public int Value { get; set; }

    public override void ReadCommand(Command command)
    {
        base.ReadCommand(command);
        var variable = (Fungus.SetStaticValue)command;
        VariableID = variable.StaticValueID;
        Value = variable.value;
    }

    public override string GetSummary()
    {
        return $"设置全局变量[{VariableID}] = {Value}";;
    }
}

[FCommandBinder(typeof(Fungus.GetStaticValue))]
public class GetStaticValue : FCommand
{
    public int VariableID { get; set; }

    public override void ReadCommand(Command command)
    {
        base.ReadCommand(command);
        var variable = (Fungus.GetStaticValue)command;
        VariableID = variable.StaticValueID;
    }

    public override string GetSummary()
    {
        return $"读取全局变量[{VariableID}] => TempValue";
    }
}