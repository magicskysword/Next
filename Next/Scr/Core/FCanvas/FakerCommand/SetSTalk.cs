using Fungus;
using HarmonyLib;
using SkySwordKill.Next.Utils;

namespace SkySwordKill.Next.FCanvas.FakerCommand;


[FCommandBinder(typeof(Fungus.SetSTalk))]
public class SetSTalk : FCommand
{
    /// <summary>
    /// 后续对话的ID
    /// </summary>
    public int TalkID;
    
    public override void ReadCommand(Command command)
    {
        base.ReadCommand(command);
        var cmdSetSTalk = (Fungus.SetSTalk)command;
        TalkID = cmdSetSTalk.GetFieldValue<int>("TalkID");
    }

    public override string GetSummary()
    {
        return $"设置后续对话ID : {TalkID}";;
    }
}