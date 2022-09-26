using Newtonsoft.Json;

namespace SkySwordKill.NextModEditor.Mod.Data;

[ModDataInit]
public class ModAffixData : ModSingleFileData<ModAffixData>
{
    public static void Init()
    {
        FileName = "TuJianChunWenBen.json";
    }
    
    [JsonProperty(PropertyName = "id",Order = 0)]
    public override int Id { get; set; }
    [JsonProperty(PropertyName = "name1",Order = 1)]
    public string ProjectTypeName { get; set; }
    [JsonProperty(PropertyName = "typenum",Order = 2)]
    public int ProjectTypeNum { get; set; }
    [JsonProperty(PropertyName = "type",Order = 3)]
    public int AffixType { get; set; }
    [JsonProperty(PropertyName = "name2",Order = 4)]
    public string Name;
    [JsonProperty(PropertyName = "descr",Order = 4)]
    public string Desc;

    public void SetProjectType(ModAffixDataProjectType projectType)
    {
        ProjectTypeName = projectType.TypeName;
        ProjectTypeNum = projectType.TypeNum;
    }
}