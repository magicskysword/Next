using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SkySwordKill.NextModEditor.Mod.Data;

public class ModAvatarJsonData : ModSingleFileData<ModAvatarJsonData>
{
    /// <summary>
    ///  角色id
    /// </summary>
    [JsonProperty(PropertyName = "id", Order = 0)]
    public override int Id { get; set; }

    /// <summary>
    ///  称号
    /// </summary>
    [JsonProperty(PropertyName = "Title", Order = 1)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    ///  姓氏
    /// </summary>
    [JsonProperty(PropertyName = "FirstName", Order = 2)]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    ///  名字
    /// </summary>
    [JsonProperty(PropertyName = "Name", Order = 3)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///  立绘
    /// </summary>
    [JsonProperty(PropertyName = "face", Order = 4)]
    public int Avatar { get; set; }

    /// <summary>
    ///  战斗立绘
    /// </summary>
    [JsonProperty(PropertyName = "fightFace", Order = 5)]
    public int FightAvatar { get; set; }

    /// <summary>
    ///  性别
    /// </summary>
    [JsonProperty(PropertyName = "SexType", Order = 6)]
    public int SexType { get; set; } = 1;

    /// <summary>
    ///  种族
    /// </summary>
    [JsonProperty(PropertyName = "AvatarType", Order = 7)]
    public int Race { get; set; } = 1;

    /// <summary>
    ///  境界
    /// </summary>
    [JsonProperty(PropertyName = "Level", Order = 8)]
    public int Level { get; set; } = 1;

    /// <summary>
    ///  气血
    /// </summary>
    [JsonProperty(PropertyName = "HP", Order = 9)]
    public int HP { get; set; } = 100;

    /// <summary>
    ///  遁速
    /// </summary>
    [JsonProperty(PropertyName = "dunSu", Order = 10)]
    public int MoveSpeed { get; set; } = 4;

    /// <summary>
    ///  资质
    /// </summary>
    [JsonProperty(PropertyName = "ziZhi", Order = 11)]
    [Obsolete("实例NPC从NPCImportantDate获取")]
    public int Talent { get; set; }

    /// <summary>
    ///  悟性
    /// </summary>
    [JsonProperty(PropertyName = "wuXin", Order = 12)]
    [Obsolete("实例NPC从NPCImportantDate获取")]
    public int Ability { get; set; }

    /// <summary>
    ///  神识
    ///  实例NPC从NPCImportantDate获取
    /// </summary>
    [JsonProperty(PropertyName = "shengShi", Order = 13)]
    public int Spirit { get; set; } = 4;

    /// <summary>
    ///  煞气
    /// </summary>
    [JsonProperty(PropertyName = "shaQi", Order = 14)]
    [Obsolete("无用数据")]
    public int shaQi { get; set; }

    /// <summary>
    ///  寿元
    /// </summary>
    [Obsolete("实例NPC从NPCImportantDate获取")]
    [JsonProperty(PropertyName = "shouYuan", Order = 15)]
    public int shouYuan { get; set; }

    /// <summary>
    ///  出场年龄
    /// </summary>
    [JsonProperty(PropertyName = "age", Order = 16)]
    [Obsolete("实例NPC从NPCImportantDate获取")]
    public int age { get; set; }

    /// <summary>
    ///  门派
    /// </summary>
    [JsonProperty(PropertyName = "menPai", Order = 17)]
    [Obsolete("空字符串")]
    public string menPai { get; set; } = string.Empty;

    /// <summary>
    ///  武器
    /// </summary>
    [JsonProperty(PropertyName = "equipWeapon", Order = 18)]
    public int equipWeapon { get; set; }

    /// <summary>
    ///  防具
    /// </summary>
    [JsonProperty(PropertyName = "equipClothing", Order = 19)]
    public int equipClothing { get; set; }

    /// <summary>
    ///  饰品
    /// </summary>
    [JsonProperty(PropertyName = "equipRing", Order = 20)]
    public int equipRing { get; set; }

    /// <summary>
    /// 灵根权重
    /// 接受5-6个参数，分别为金木水火土魔的对应权重数值，非使用魔气的npc可以不写第六个
    /// </summary>
    [JsonProperty(PropertyName = "LingGen", Order = 21)]
    public List<int> LingGen { get; set; }

    /// <summary>
    ///  神通
    /// </summary>
    [JsonProperty(PropertyName = "skills", Order = 22)]
    public List<int> skills { get; set; }

    /// <summary>
    ///  功法
    /// </summary>
    [JsonProperty(PropertyName = "staticSkills", Order = 23)]
    public List<int> staticSkills { get; set; }

    /// <summary>
    ///  元婴功法
    /// </summary>
    [JsonProperty(PropertyName = "yuanying", Order = 24)]
    public int yuanying { get; set; }

    /// <summary>
    ///  化神领域
    /// </summary>
    [JsonProperty(PropertyName = "HuaShenLingYu", Order = 25)]
    public int HuaShenLingYu { get; set; }

    /// <summary>
    ///  富有度
    /// </summary>
    [JsonProperty(PropertyName = "MoneyType", Order = 26)]
    public int MoneyType { get; set; }

    /// <summary>
    ///  死亡是否刷新
    /// </summary>
    [JsonProperty(PropertyName = "IsRefresh", Order = 27)]
    public int IsRefresh { get; set; }

    /// <summary>
    ///  战场掉落方式
    /// </summary>
    [JsonProperty(PropertyName = "dropType", Order = 28)]
    public int dropType { get; set; }

    /// <summary>
    ///  是否参加拍卖
    /// </summary>
    [JsonProperty(PropertyName = "canjiaPaiMai", Order = 29)]
    public int canjiaPaiMai { get; set; }

    /// <summary>
    ///  拍卖分组
    /// </summary>
    [JsonProperty(PropertyName = "paimaifenzu", Order = 30)]
    public List<int> paimaifenzu { get; set; }

    /// <summary>
    ///  悟道类型
    /// </summary>
    [JsonProperty(PropertyName = "wudaoType", Order = 31)]
    public int wudaoType { get; set; }

    /// <summary>
    ///  感兴趣的物品大类型
    /// </summary>
    [JsonProperty(PropertyName = "XinQuType", Order = 32)]
    public int XinQuType { get; set; }

    /// <summary>
    ///  是否固定价格
    /// </summary>
    [JsonProperty(PropertyName = "gudingjiage", Order = 33)]
    public int gudingjiage { get; set; }

    /// <summary>
    ///  出售物品固定系数
    /// </summary>
    [JsonProperty(PropertyName = "sellPercent", Order = 34)]
    public int sellPercent { get; set; }
}