using System.Collections.Generic;
using Newtonsoft.Json;

namespace SkySwordKill.NextModEditor.Mod.Data;

[ModDataInit]
public class ModBuffData : ModFolderFileData<ModBuffData>
{
    public static void Init()
    {
        FolderName = "BuffJsonData";
    }

    [JsonProperty(PropertyName = "buffid", Order = 0)]
    public override int Id { get; set; }

    [JsonProperty(PropertyName = "BuffIcon", Order = 1)]
    public int Icon { get; set; }

    [JsonProperty(PropertyName = "skillEffect", Order = 2)]
    public string SkillEffect { get; set; } = "fx_Summoner_o";

    [JsonProperty(PropertyName = "name", Order = 3)]
    public string Name { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "Affix", Order = 4)]
    public List<int> AffixList { get; set; } = new List<int>();

    [JsonProperty(PropertyName = "bufftype", Order = 5)]
    public int BuffType { get; set; }

    [JsonProperty(PropertyName = "seid", Order = 6)]
    public List<int> SeidList { get; set; } = new List<int>();

    [JsonProperty(PropertyName = "descr", Order = 7)]
    public string Desc { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "trigger", Order = 8)]
    public int Trigger { get; set; }

    [JsonProperty(PropertyName = "removeTrigger", Order = 9)]
    public int RemoverTrigger { get; set; }

    /// <summary>
    /// 忽略项
    /// </summary>
    [JsonProperty(PropertyName = "script", Order = 10)]
    public string Script { get; set; } = "Buff";

    /// <summary>
    /// 忽略项
    /// </summary>
    [JsonProperty(PropertyName = "looptime", Order = 11)]
    public int LoopTime { get; set; } = 1;

    /// <summary>
    /// 忽略项
    /// </summary>
    [JsonProperty(PropertyName = "totaltime", Order = 12)]
    public int TotalTime { get; set; } = 1;

    [JsonProperty(PropertyName = "BuffType", Order = 13)]
    public int BuffOverlayType { get; set; }

    [JsonProperty(PropertyName = "isHide", Order = 14)]
    public int IsHide { get; set; }

    [JsonProperty(PropertyName = "ShowOnlyOne", Order = 15)]
    public int ShowOnlyOne { get; set; }

    public static ModBuffDataBuffType GetBuffType(int typeId)
    {
        return ModEditorManager.I.BuffDataBuffTypes.Find(data => data.TypeID == typeId);
    }
}