using System.Collections.Generic;
using Newtonsoft.Json;

namespace SkySwordKill.NextModEditor.Mod.Data
{
    [ModDataInit]
    public class ModSkillData : ModFolderFileData<ModSkillData>
    {
        public static void Init()
        {
            FolderName = "skillJsonData";
        }

        [JsonProperty("id", Order = 1)]
        public override int Id { get; set; }
        
        [JsonProperty("Skill_ID", Order = 2)]
        public int SkillId { get; set; }

        [JsonProperty("Skill_Lv", Order = 3)]
        public int SkillLv { get; set; } = 1;
        
        [JsonProperty("skillEffect", Order = 4)]
        public string Effect { get; set; } = "1";
        
        [JsonProperty("Skill_Type", Order = 5)]
        public int SkillPriority { get; set; }
        
        [JsonProperty("name", Order = 6)]
        public string Name { get; set; }
        
        /// <summary>
        /// 请教类型
        /// </summary>
        [JsonProperty("qingjiaotype", Order = 7)]
        public int ConsultType { get; set; }
        
        [JsonProperty("seid", Order = 8)]
        public List<int> SeidList { get; set; } = new List<int>();
        
        [JsonProperty("Affix", Order = 9)]
        public List<int> OldAffixList { get; set; } = new List<int>();
        
        [JsonProperty("Affix2", Order = 10)]
        public List<int> AffixList { get; set; } = new List<int>();
        
        [JsonProperty("descr", Order = 11)]
        public string Desc { get; set; }
        
        [JsonProperty("TuJiandescr", Order = 12)]
        public string GuideDesc { get; set; }
        
        [JsonProperty("AttackType", Order = 13)]
        public List<int> AttackTypeList { get; set; } = new List<int>();

        /// <summary>
        /// 攻击脚本
        /// SkillAttack（对敌方）
        /// SkillSelf（对自己）
        /// </summary>
        [JsonProperty("script", Order = 14)]
        public string Script { get; set; } = "SkillAttack";
        
        [JsonProperty("HP", Order = 15)]
        public int BaseDamage { get; set; }

        /// <summary>
        /// 必须配0
        /// </summary>
        [JsonProperty("speed", Order = 16)]
        public int Speed { get; set; } = 0;

        [JsonProperty("icon", Order = 17)]
        public int Icon { get; set; } = 0;
        
        /// <summary>
        /// 必须配0
        /// </summary>
        [JsonProperty("Skill_DisplayType", Order = 18)]
        public int SkillDisplayType { get; set; } = 0;
        
        /// <summary>
        /// 同系灵气消耗
        /// </summary>
        [JsonProperty("skill_SameCastNum", Order = 19)]
        public List<int> SkillSameCastNumList { get; set; } = new List<int>();
        
        /// <summary>
        /// 灵气消耗类型
        /// </summary>
        [JsonProperty("skill_CastType", Order = 20)]
        public List<int> SkillCastTypeList { get; set; } = new List<int>();
        
        /// <summary>
        /// 灵气消耗
        /// </summary>
        [JsonProperty("skill_Cast", Order = 21)]
        public List<int> SkillCastList { get; set; } = new List<int>();

        /// <summary>
        /// 品阶
        /// </summary>
        [JsonProperty(PropertyName = "Skill_LV", Order = 12)]
        public int Quality { get; set; } = 1;

        /// <summary>
        /// 功法技能品阶
        /// </summary>
        [JsonProperty(PropertyName = "typePinJie", Order = 13)]
        public int Phase { get; set; } = 1;
        
        /// <summary>
        /// 斗法
        /// </summary>
        [JsonProperty("DF", Order = 22)]
        public int Battle { get; set; }
        
        /// <summary>
        /// 图鉴类型
        /// </summary>
        [JsonProperty("TuJianType", Order = 23)]
        public int GuideType { get; set; }
        
        /// <summary>
        /// 学习境界
        /// </summary>
        [JsonProperty("Skill_Open", Order = 24)]
        public int LearnLevel { get; set; }
        
        /// <summary>
        /// 参悟月数
        /// </summary>
        [JsonProperty("Skill_castTime", Order = 25)]
        public int LearnCostMonth { get; set; }

        /// <summary>
        /// 攻击距离 - 废弃
        /// </summary>
        [JsonProperty("canUseDistMax", Order = 26)]
        public int Range { get; set; } = 30;

        /// <summary>
        /// 技能CD - 废弃
        /// </summary>
        [JsonProperty("CD", Order = 27)]
        public int Cd { get; set; } = 10000;
    }
}