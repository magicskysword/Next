using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;
using SkySwordKill.NextModEditor.Mod.Data;
using SFB;
using SkySwordKill.Next.Extension;

namespace SkySwordKill.NextEditor.Mod
{
    public class ModMgr
{
    public static ModMgr I { get; }
    
    // Meta
    public List<ModAffixDataProjectType> AffixDataProjectTypes { get; set; }
    public List<ModAffixDataAffixType> AffixDataAffixTypes { get; set; }
    public Dictionary<int,ModSeidMeta> CreateAvatarSeidMetas { get; set; }
    public List<ModCreateAvatarDataTalentType> CreateAvatarDataTalentTypes { get; set; }
    public List<ModCreateAvatarDataLevelType> CreateAvatarDataLevelTypes { get; set; }
    public Dictionary<int,ModSeidMeta> BuffSeidMetas { get; set; }
    public List<ModBuffDataBuffType> BuffDataBuffTypes { get; set; }
    public List<ModBuffDataTriggerType> BuffDataTriggerTypes { get; set; }
    public List<ModBuffDataRemoveTriggerType> BuffDataRemoveTriggerTypes { get; set; }
    public List<ModBuffDataOverlayType> BuffDataOverlayTypes { get; set; }
    public List<ModItemDataGuideType> ItemDataGuideTypes { get; set; }
    public List<ModAttackType> AttackTypes { get; set; }
    public List<ModElementType> ElementTypes { get; set; }
    public List<ModComparisonOperatorType> ComparisonOperatorTypes { get; set; }
    public List<ModTargetType> TargetTypes { get; set; }
    
    // Data
    public Dictionary<int,ModAffixData> DefaultAffixData { get; set; }
    public Dictionary<int,ModBuffData> DefaultBuffData { get; set; }

    static ModMgr()
    {
        foreach (var type in ModUtils.GetTypesWithAttribute(Assembly.GetAssembly(typeof(ModDataInitAttribute)),
                     typeof(ModDataInitAttribute)))
        {
            var initMethod = type.GetMethod("Init", BindingFlags.Static | BindingFlags.Public);
            initMethod.Invoke(null, Array.Empty<object>());
        }
        I = new ModMgr();
        I.Init();
    }

    public void Init()
    {
        AffixDataProjectTypes = JArray
            .Parse(ModUtils.LoadConfig("Meta/AffixProjectType.json"))
            .ToObject<List<ModAffixDataProjectType>>();
        AffixDataAffixTypes = JArray
            .Parse(ModUtils.LoadConfig("Meta/AffixType.json"))
            .ToObject<List<ModAffixDataAffixType>>();
        CreateAvatarSeidMetas = JObject
            .Parse(ModUtils.LoadConfig("Meta/CreateAvatarSeidMeta.json"))
            .ToObject<Dictionary<int, ModSeidMeta>>();
        CreateAvatarDataTalentTypes = JArray
            .Parse(ModUtils.LoadConfig("Meta/CreateAvatarTalentType.json"))
            .ToObject<List<ModCreateAvatarDataTalentType>>();
        CreateAvatarDataLevelTypes = JArray
            .Parse(ModUtils.LoadConfig("Meta/CreateAvatarLevelType.json"))
            .ToObject<List<ModCreateAvatarDataLevelType>>();
        BuffSeidMetas = JObject
            .Parse(ModUtils.LoadConfig("Meta/BuffSeidMeta.json"))
            .ToObject<Dictionary<int, ModSeidMeta>>();
        BuffDataBuffTypes = JArray
            .Parse(ModUtils.LoadConfig("Meta/BuffType.json"))
            .ToObject<List<ModBuffDataBuffType>>();
        BuffDataTriggerTypes = JArray
            .Parse(ModUtils.LoadConfig("Meta/BuffTriggerType.json"))
            .ToObject<List<ModBuffDataTriggerType>>();
        BuffDataRemoveTriggerTypes = JArray
            .Parse(ModUtils.LoadConfig("Meta/BuffRemoveTriggerType.json"))
            .ToObject<List<ModBuffDataRemoveTriggerType>>();
        BuffDataOverlayTypes = JArray
            .Parse(ModUtils.LoadConfig("Meta/BuffOverlayType.json"))
            .ToObject<List<ModBuffDataOverlayType>>();
        ItemDataGuideTypes = JArray
            .Parse(ModUtils.LoadConfig("Meta/ItemGuideType.json"))
            .ToObject<List<ModItemDataGuideType>>();
        AttackTypes = JArray
            .Parse(ModUtils.LoadConfig("Meta/AttackType.json"))
            .ToObject<List<ModAttackType>>();
        ElementTypes = JArray
            .Parse(ModUtils.LoadConfig("Meta/ElementType.json"))
            .ToObject<List<ModElementType>>();
        ComparisonOperatorTypes = JArray
            .Parse(ModUtils.LoadConfig("Meta/ComparisonOperatorType.json"))
            .ToObject<List<ModComparisonOperatorType>>();
        TargetTypes = JArray
            .Parse(ModUtils.LoadConfig("Meta/TargetType.json"))
            .ToObject<List<ModTargetType>>();
        
        DefaultAffixData = ModAffixData.Load(ModUtils.GetConfigPath("Data"))
            .ToDictionary(pair => pair.Value.ID, pair => pair.Value);
        DefaultBuffData = ModBuffData.Load(ModUtils.GetConfigPath("Data"))
            .ToDictionary(item => item.ID);
    }

    public ModProject OpenProject()
    {
        // Open file
        var paths = StandaloneFileBrowser.OpenFolderPanel("打开Mod目录", "", false);

        if (paths.Length != 0)
        {
            var path = paths[0];
            var project = ModProject.Load(path);
            return project;
        }

        return null;
    }

    public void SaveProject(ModProject modProject,string path = null)
    {
        if (path == null)
            path = modProject.ProjectPath;
        ModProject.Save(path, modProject);
    }

    public string GetCreateAvatarTalentTypeDesc(int relationNum)
    {
        return CreateAvatarDataTalentTypes.FirstOrDefault(type => type.TypeID == relationNum)?.Desc ?? "Main.Unknown".I18N();
    }
}
}