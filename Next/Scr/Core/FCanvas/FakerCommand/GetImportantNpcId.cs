using Fungus;
using SkySwordKill.Next.Utils;

namespace SkySwordKill.Next.FCanvas.FakerCommand;

[FCommandBinder(typeof(Fungus.GetImportantNpcId))]
public class GetImportantNpcId : FCommand
{
    public string NpcBindId;
    
    public override void ReadCommand(Command command)
    {
        base.ReadCommand(command);
        var cmdGetImportantNpcId = (Fungus.GetImportantNpcId)command;
        NpcBindId = cmdGetImportantNpcId.GetFieldValue<IntegerVariable>("NpcBingDingId")?.Key ?? "";
    }

    public override string GetSummary()
    {
        return $"获取重要NPC ID : {NpcBindId}";
    }
}