using System.Collections.Generic;
using Newtonsoft.Json;

namespace SkySwordKill.NextModEditor.Mod.Data
{
    [ModDataInit]
    public class ModItemData : ModFolderFileData<ModItemData>
    {
        public static void Init()
        {
            FolderName = "ItemJsonData";
        }

        [JsonProperty(PropertyName = "id", Order = 0)]
        public override int Id { get; set; }

        [JsonProperty(PropertyName = "ItemIcon", Order = 1)]
        public int ItemIcon { get; set; }

        [JsonProperty(PropertyName = "maxNum", Order = 2)]
        public int MaxStack { get; set; }

        [JsonProperty(PropertyName = "name", Order = 3)]
        public string Name { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "FaBaoType", Order = 4)]
        public string ArtifactType { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "Affix", Order = 5)]
        public List<int> AffixList { get; set; } = new List<int>();

        [JsonProperty(PropertyName = "TuJianType", Order = 6)]
        public int GuideType { get; set; }

        [JsonProperty(PropertyName = "ShopType", Order = 7)]
        public int ShopType { get; set; }

        [JsonProperty(PropertyName = "ItemFlag", Order = 8)]
        public List<int> ItemFlagList { get; set; } = new List<int>();

        [JsonProperty(PropertyName = "WuWeiType", Order = 9)]
        public int MaterialType { get; set; }

        [JsonProperty(PropertyName = "ShuXingType", Order = 10)]
        public int PropertyType { get; set; }

        [JsonProperty(PropertyName = "type", Order = 11)]
        public int ItemType { get; set; }

        [JsonProperty(PropertyName = "quality", Order = 12)]
        public int Quality { get; set; }

        [JsonProperty(PropertyName = "typePinJie", Order = 13)]
        public int SkillQuality { get; set; }

        [JsonProperty(PropertyName = "StuTime", Order = 14)]
        public int StudyCostTime { get; set; }

        [JsonProperty(PropertyName = "seid", Order = 15)]
        public List<int> SeidList { get; set; } = new List<int>();

        [JsonProperty(PropertyName = "vagueType", Order = 16)]
        public int UseType { get; set; }

        [JsonProperty(PropertyName = "price", Order = 17)]
        public int Price { get; set; }

        [JsonProperty(PropertyName = "desc", Order = 18)]
        public string Desc { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "desc2", Order = 19)]
        public string Info { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "CanSale", Order = 20)]
        public int CanSale { get; set; }

        [JsonProperty(PropertyName = "DanDu", Order = 20)]
        public int DrugPoison { get; set; }

        [JsonProperty(PropertyName = "CanUse", Order = 20)]
        public int CanUse { get; set; }

        [JsonProperty(PropertyName = "NPCCanUse", Order = 20)]
        public int NpcCanUse { get; set; }

        [JsonProperty(PropertyName = "yaoZhi1", Order = 20)]
        public int MedicinalGuiding { get; set; }

        [JsonProperty(PropertyName = "yaoZhi2", Order = 20)]
        public int MedicinalMain { get; set; }

        [JsonProperty(PropertyName = "yaoZhi3", Order = 20)]
        public int MedicinalSub { get; set; }

        [JsonProperty(PropertyName = "wuDao", Order = 20)]
        public List<int> StudyRequirement { get; set; } = new List<int>();
    }
}