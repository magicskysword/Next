using Newtonsoft.Json;

namespace SkySwordKill.NextModEditor.Mod.Data;

[ModDataInit]
public class ModComprehensionPhaseData : ModSingleFileData<ModComprehensionPhaseData>
{
    public static void Init()
    {
        FileName = "WuDaoJinJieJson.json";
    }
        
    [JsonProperty("id", Order = 0)]
    public override int Id { get; set; }
        
    [JsonProperty("LV", Order = 1)]
    public int Level { get; set; }
        
    [JsonProperty("Text", Order = 2)]
    public string Name { get; set; }
        
    [JsonProperty("Max", Order = 3)]
    public int Max { get; set; }
        
    [JsonProperty("JiaCheng", Order = 4)]
    public float Addition { get; set; }
        
    [JsonProperty("LianDan", Order = 5)]
    public int AlchemyCompressionMax { get; set; }
        
    [JsonProperty("LianQi", Order = 6)]
    public int ForgeCompressionMax { get; set; }
}