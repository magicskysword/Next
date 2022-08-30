using Newtonsoft.Json;

namespace SkySwordKill.NextModEditor.Mod.Data
{
    [ModDataInit]
    public class ModAlchemyElementData : ModSingleFileData<ModAlchemyElementData>
    {
        public static void Init()
        {
            FileName = "LianDanItemLeiXin.json";
        }
        
        [JsonProperty("id", Order = 0)]
        public override int Id { get; set; }
        
        [JsonProperty("name", Order = 1)]
        public string Name { get; set; }
        
        [JsonProperty("desc", Order = 2)]
        public string Desc { get; set; }
    }
}