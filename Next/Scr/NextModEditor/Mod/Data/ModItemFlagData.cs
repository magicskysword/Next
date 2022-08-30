using Newtonsoft.Json;

namespace SkySwordKill.NextModEditor.Mod.Data
{
    [ModDataInit]
    public class ModItemFlagData : ModSingleFileData<ModItemFlagData>
    {
        public static void Init()
        {
            FileName = "ItemFlagData.json";
        }
        
        [JsonProperty("id", Order = 0)]
        public override int Id { get; set; }
        
        [JsonProperty("name", Order = 1)]
        public string Name { get; set; }
    }
}