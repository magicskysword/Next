using System.Collections.Generic;
using Newtonsoft.Json;

namespace SkySwordKill.NextModEditor.Mod.Data
{
    [ModDataInit]
    public class ModCreateAvatarData : ModSingleFileData<ModCreateAvatarData>
    {
        public static void Init()
        {
            FileName = "CreateAvatarJsonData.json";
        }

        [JsonProperty(PropertyName = "id", Order = 0)]
        public override int Id { get; set; }

        [JsonProperty(PropertyName = "Title", Order = 1)]
        public string Name { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "fenZu", Order = 2)]
        public int Group { get; set; }

        [JsonProperty(PropertyName = "feiYong", Order = 3)]
        public int Cost { get; set; }

        [JsonProperty(PropertyName = "fenLei", Order = 4)]
        public string TalentType { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "fenLeiGuanLian", Order = 5)]
        public int TalentTypeRelation { get; set; }

        [JsonProperty(PropertyName = "seid", Order = 6)]
        public List<int> SeidList { get; set; } = new List<int>();

        [JsonProperty(PropertyName = "jiesuo", Order = 7)]
        public int RequireLevel { get; set; }

        [JsonProperty(PropertyName = "Desc", Order = 8)]
        public string Desc { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "Info", Order = 9)]
        public string Info { get; set; } = string.Empty;

        public void SetTalentType(ModCreateAvatarDataTalentType type)
        {
            TalentType = type.TypeName;
            TalentTypeRelation = type.TypeID;
        }
    }
}