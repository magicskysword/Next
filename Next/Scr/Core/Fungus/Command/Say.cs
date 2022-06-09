using Fungus;
using HarmonyLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SkySwordKill.Next.FungusTools.FakerCommand
{
    [FCommandBinder(typeof(Fungus.Say))]
    public class Say : FCommand
    {
        public int AvatarIDInt;
        public string AvatarBindKey;
        public string StoryText;
        public string Description;
        [JsonConverter(typeof(StringEnumConverter))]
        public StartFight.MonstarType AvatarIDSetType;
        
        public override void ReadCommand(Command command)
        {
            base.ReadCommand(command);
            var cmdSay = (Fungus.Say)command;

            AvatarIDInt = (int)AccessTools.Field(command.GetType(), "AvatarIntID").GetValue(command);
            AvatarBindKey = ((IntegerVariable)AccessTools.Field(command.GetType(), "AvatarID")
                .GetValue(command))?.Key ?? "";
            StoryText = (string)AccessTools.Field(command.GetType(), "storyText").GetValue(command);
            Description = (string)AccessTools.Field(command.GetType(), "description")?.GetValue(command);
            AvatarIDSetType = cmdSay._AvatarIDSetType;
        }
    }
}