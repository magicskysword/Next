using Fungus;
using SkySwordKill.Next.Utils;

namespace SkySwordKill.Next.FCanvas.FakerCommand;

[FCommandBinder(typeof(Fungus.Call))]
public class Call : FCommand
{
    public string targetFlowchartName { get; set; }
    public string targetBlockID { get; set; }
    public string startLabel { get; set; }
    public int startIndex { get; set; }
    public CallMode callMode { get; set; }
        
    public override void ReadCommand(Command command)
    {
        base.ReadCommand(command);
        var cmdCall = (Fungus.Call)command;
            
        targetFlowchartName = cmdCall.GetFieldValue<Flowchart>("targetFlowchart")?.GetParentName();
        var block = cmdCall.GetFieldValue<Block>("targetBlock");
        if(block != null)
        {
            targetBlockID = $"{block.ItemId}({block.BlockName})";
        }
        startLabel = cmdCall.GetFieldValue<StringData>("startLabel").Value;
        startIndex = cmdCall.GetFieldValue<int>("startIndex");
        callMode = cmdCall.GetFieldValue<CallMode>("callMode");
    }

    public override string GetSummary()
    {
        return $"跳转 {targetFlowchartName ?? "this"}:{targetBlockID}";
    }
}