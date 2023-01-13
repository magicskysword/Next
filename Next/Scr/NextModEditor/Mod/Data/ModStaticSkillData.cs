using Newtonsoft.Json;

namespace SkySwordKill.NextModEditor.Mod.Data;

public class ModStaticSkillData : ModFolderFileData<ModStaticSkillData>
{
    public static void Init()
    {
        FolderName = "skillJsonData";
    }
    
    [JsonProperty("id", Order = 1)]
    public override int Id { get; set; }
}