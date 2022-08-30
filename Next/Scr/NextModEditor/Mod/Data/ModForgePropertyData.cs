using Newtonsoft.Json;

namespace SkySwordKill.NextModEditor.Mod.Data
{
    [ModDataInit]
    public class ModForgePropertyData : ModSingleFileData<ModForgePropertyData>
    {
        public static void Init()
        {
            FileName = "LianQiShuXinLeiBie.json";
        }

        [JsonProperty("id")]
        public override int Id { get; set; }
        
        [JsonProperty("desc")]
        public string Desc { get; set; }
        
        [JsonProperty("AttackType")]
        public int AttackType { get; set; }
    }
}