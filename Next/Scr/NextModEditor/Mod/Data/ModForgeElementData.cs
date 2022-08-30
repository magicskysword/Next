using Newtonsoft.Json;

namespace SkySwordKill.NextModEditor.Mod.Data
{
    [ModDataInit]
    public class ModForgeElementData : ModSingleFileData<ModForgeElementData>
    {
        public static void Init()
        {
            FileName = "LianQiWuWeiBiao.json";
        }

        [JsonProperty("id")]
        public override int Id { get; set; }
        
        [JsonProperty("desc")]
        public string Desc { get; set; }
        
        [JsonProperty("value1")]
        public int Value1 { get; set; }
        
        [JsonProperty("value2")]
        public int Value2 { get; set; }
        
        [JsonProperty("value3")]
        public int Value3 { get; set; }
        
        [JsonProperty("value4")]
        public int Value4 { get; set; }
        
        [JsonProperty("value5")]
        public int Value5 { get; set; }
    }
}