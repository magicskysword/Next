using Newtonsoft.Json;

namespace SkySwordKill.NextModEditor.Mod.Data
{
    [ModDataInit]
    public class ModComprehensionData : ModSingleFileData<ModComprehensionData>
    {
        public static void Init()
        {
            FileName = "WuDaoAllTypeJson.json";
        }
        
        [JsonProperty("id", Order = 0)]
        public override int Id { get; set; }
        
        [JsonProperty("name", Order = 1)]
        public string Name { get; set; }
        
        [JsonProperty("name1", Order = 1)]
        public string Desc { get; set; }
    }
}