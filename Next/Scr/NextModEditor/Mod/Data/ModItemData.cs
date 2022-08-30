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

        /// <summary>
        /// 最大堆叠数量
        /// </summary>
        [JsonProperty(PropertyName = "maxNum", Order = 2)]
        public int MaxStack { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [JsonProperty(PropertyName = "name", Order = 3)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 法宝类型
        /// </summary>
        [JsonProperty(PropertyName = "FaBaoType", Order = 4)]
        public string ArtifactType { get; set; } = string.Empty;

        /// <summary>
        /// 特性列表
        /// </summary>
        [JsonProperty(PropertyName = "Affix", Order = 5)]
        public List<int> AffixList { get; set; } = new List<int>();

        /// <summary>
        /// 图鉴类型
        /// </summary>
        [JsonProperty(PropertyName = "TuJianType", Order = 6)]
        public int GuideType { get; set; }

        /// <summary>
        /// 商店投放类型
        /// </summary>
        [JsonProperty(PropertyName = "ShopType", Order = 7)]
        public int ShopType { get; set; } = 99;

        /// <summary>
        /// 物品标签
        /// </summary>
        [JsonProperty(PropertyName = "ItemFlag", Order = 8)]
        public List<int> ItemFlagList { get; set; } = new List<int>();

        /// <summary>
        /// 五维类别
        /// </summary>
        [JsonProperty(PropertyName = "WuWeiType", Order = 9)]
        public int ForgeElementType { get; set; }

        /// <summary>
        /// 属性类别
        /// </summary>
        [JsonProperty(PropertyName = "ShuXingType", Order = 10)]
        public int ForgePropertyType { get; set; }

        /// <summary>
        /// 物品类型（0武器 1衣服 2饰品 3技能书 4功法书 5丹药
        /// 6药材 7任务 8矿石 9丹炉 10丹方 11药渣 12书籍 13书籍（需要时间） 14灵舟 15秘药 16其它）
        /// </summary>
        [JsonProperty(PropertyName = "type", Order = 11)]
        public int ItemType { get; set; }

        /// <summary>
        /// 品阶
        /// </summary>
        [JsonProperty(PropertyName = "quality", Order = 12)]
        public int Quality { get; set; }

        /// <summary>
        /// 功法技能品阶
        /// </summary>
        [JsonProperty(PropertyName = "typePinJie", Order = 13)]
        public int Phase { get; set; }

        /// <summary>
        /// 领悟时间
        /// </summary>
        [JsonProperty(PropertyName = "StuTime", Order = 14)]
        public int StudyCostTime { get; set; }

        /// <summary>
        /// 物品拥有特性
        /// </summary>
        [JsonProperty(PropertyName = "seid", Order = 15)]
        public List<int> SeidList { get; set; } = new List<int>();

        /// <summary>
        /// 大类型 - 当前为指定能否使用
        /// <br/>0 - 不可使用
        /// <br/>1 - 可使用
        /// </summary>
        [JsonProperty(PropertyName = "vagueType", Order = 16)]
        public int SpecialType { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        [JsonProperty(PropertyName = "price", Order = 17)]
        public int Price { get; set; } = 0;

        /// <summary>
        /// 功能说明
        /// </summary>
        [JsonProperty(PropertyName = "desc", Order = 18)]
        public string Info { get; set; } = string.Empty;

        /// <summary>
        /// 物品描述
        /// </summary>
        [JsonProperty(PropertyName = "desc2", Order = 19)]
        public string Desc { get; set; } = string.Empty;

        /// <summary>
        /// 是否不可出售
        /// <br/> 0 - 可出售
        /// <br/> 1 - 不可出售
        /// </summary>
        [JsonProperty(PropertyName = "CanSale", Order = 20)]
        public int CanNotSale { get; set; } = 0;

        /// <summary>
        /// 丹毒
        /// </summary>
        [JsonProperty(PropertyName = "DanDu", Order = 20)]
        public int DrugPoison { get; set; }

        /// <summary>
        /// 丹药使用次数
        /// </summary>
        [JsonProperty(PropertyName = "CanUse", Order = 20)]
        public int CanUseCount { get; set; }

        /// <summary>
        /// NPC能否使用
        /// <br/> 0 - 不可使用
        /// <br/> 1 - 可使用
        /// </summary>
        [JsonProperty(PropertyName = "NPCCanUse", Order = 20)]
        public int NpcCanUse { get; set; } = 0;

        /// <summary>
        /// 药引
        /// </summary>
        [JsonProperty(PropertyName = "yaoZhi1", Order = 20)]
        public int AlchemyGuiding { get; set; }

        /// <summary>
        /// 主药
        /// </summary>
        [JsonProperty(PropertyName = "yaoZhi2", Order = 20)]
        public int AlchemyMain { get; set; }

        /// <summary>
        /// 辅药
        /// </summary>
        [JsonProperty(PropertyName = "yaoZhi3", Order = 20)]
        public int AlchemySub { get; set; }

        /// <summary>
        /// 领悟前置条件 - 两个一组，[道类型,道等级]
        /// </summary>
        [JsonProperty(PropertyName = "wuDao", Order = 20)]
        public List<int> StudyRequirement { get; set; } = new List<int>();
    }
}