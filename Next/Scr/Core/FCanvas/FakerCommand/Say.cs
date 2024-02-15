using Fungus;
using HarmonyLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SkySwordKill.Next.FCanvas.FakerCommand;

[FCommandBinder(typeof(Fungus.Say))]
public class Say : FCommand
{
    public int AvatarIDInt { get; set; }
    public string AvatarBindKey { get; set; }
    public string StoryText { get; set; }
    public string Description { get; set; }
    public int AvatarIDSetType { get; set; }
    
    public override void ReadCommand(Command command)
    {
        base.ReadCommand(command);
        var cmdSay = (Fungus.Say)command;

        AvatarIDInt = (int)AccessTools.Field(command.GetType(), "AvatarIntID").GetValue(command);
        AvatarBindKey = ((IntegerVariable)AccessTools.Field(command.GetType(), "AvatarID")
            .GetValue(command))?.Key ?? "";
        StoryText = (string)AccessTools.Field(command.GetType(), "storyText").GetValue(command);
        Description = (string)AccessTools.Field(command.GetType(), "description")?.GetValue(command);
        AvatarIDSetType = (int)cmdSay._AvatarIDSetType;
    }

    public override string GetSummary()
    {
        var id = AvatarIDSetType == 0
            ? AvatarIDInt.ToString()
            : AvatarBindKey;
        return  $"{id} : {StoryText}";
    }
}
