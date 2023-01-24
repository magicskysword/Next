using System.Collections.Generic;
using Newtonsoft.Json;

namespace SkySwordKill.NextModEditor.Mod.Data;

[ModDataInit]
public class ModStaticSkillData : ModSingleFileData<ModStaticSkillData>
{
    public static void Init()
    {
        FileName = "StaticSkillJsonData.json";
    }
    
    [JsonProperty("id", Order = 1)]
    public override int Id { get; set; }
    
    [JsonProperty("Skill_ID", Order = 1)]
    public int SkillPkId { get; set; }
    
    [JsonProperty("Skill_Lv", Order = 1)]
    public int SkillLv { get; set; }
    
    [JsonProperty("name", Order = 1)]
    public string Name { get; set; } = string.Empty;
    
    [JsonProperty("Affix", Order = 1)]
    public List<int> AffixList { get; set; } = new List<int>();
    
    /// <summary>
    /// 请教类型
    /// </summary>
    [JsonProperty("qingjiaotype", Order = 1)]
    public int ConsultType { get; set; }

    /// <summary>
    /// 特性列表
    /// </summary>
    [JsonProperty("seid", Order = 1)]
    public List<int> SeidList { get; set; } = new List<int>();

    [JsonProperty("TuJiandescr", Order = 1)]
    public string GuideDesc { get; set; } = string.Empty;
    
    [JsonProperty("descr", Order = 1)]
    public string Desc { get; set; } = string.Empty;
    
    [JsonProperty("AttackType", Order = 1)]
    public int AttackType { get; set; }
    
    [JsonProperty("icon", Order = 1)]
    public int Icon { get; set; }
    
    /// <summary>
    /// 品阶
    /// </summary>
    [JsonProperty("Skill_LV", Order = 1)]
    public int Quality { get; set; }
    
    /// <summary>
    /// 功法技能阶段
    /// </summary>
    [JsonProperty("typePinJie", Order = 1)]
    public int Phase { get; set; }
    
    /// <summary>
    /// 参悟月数
    /// </summary>
    [JsonProperty("Skill_castTime", Order = 1)]
    public int LearnCostMonth { get; set; }
    
    /// <summary>
    /// 修炼速度
    /// </summary>
    [JsonProperty("Skill_Speed", Order = 1)]
    public int TrainingSpeed { get; set; }
    
    /// <summary>
    /// 斗法可用
    /// </summary>
    [JsonProperty("DF", Order = 1)]
    public int Battle { get; set; }
    
    /// <summary>
    /// 图鉴类型
    /// </summary>
    [JsonProperty("TuJianType", Order = 23)]
    public int GuideType { get; set; }
}