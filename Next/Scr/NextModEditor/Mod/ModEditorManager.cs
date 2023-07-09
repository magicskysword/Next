using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;
using script.Steam;
using SkySwordKill.Next;
using SkySwordKill.NextModEditor.Mod.Data;
using SkySwordKill.Next.Extension;
using SkySwordKill.Next.FCanvas;
using SkySwordKill.Next.Mod;
using Steamworks;

namespace SkySwordKill.NextModEditor.Mod;

public class ModEditorManager
{
    public static ModEditorManager I { get; }

    // Meta
    public List<ModAffixDataProjectType> AffixDataProjectTypes { get; set; }
    public List<ModAffixDataAffixType> AffixDataAffixTypes { get; set; }
    public Dictionary<int, ModSeidMeta> CreateAvatarSeidMetas { get; set; }
    public List<ModCreateAvatarDataTalentType> CreateAvatarDataTalentTypes { get; set; }
    public List<ModLevelType> LevelTypes { get; set; }
    public Dictionary<int, ModSeidMeta> BuffSeidMetas { get; set; }
    public List<ModBuffDataBuffType> BuffDataBuffTypes { get; set; }
    public List<ModBuffDataTriggerType> BuffDataTriggerTypes { get; set; }
    public List<ModBuffDataRemoveTriggerType> BuffDataRemoveTriggerTypes { get; set; }
    public List<ModBuffDataOverlayType> BuffDataOverlayTypes { get; set; }
    public List<ModGuideType> GuideTypes { get; set; }
    public List<ModItemDataShopType> ItemDataShopTypes { get; set; }
    public List<ModItemDataQualityType> ItemDataQualityTypes { get; set; }
    public List<ModItemDataPhaseType> ItemDataPhaseTypes { get; set; }
    public List<ModItemDataType> ItemDataTypes { get; set; }
    public Dictionary<int, ModSeidMeta> ItemEquipSeidMetas { get; set; }
    public Dictionary<int, ModSeidMeta> ItemUseSeidMetas { get; set; }
    public List<ModItemDataUseType> ItemDataUseTypes { get; set; }
    public Dictionary<int, ModSeidMeta> SkillSeidMetas { get; set; }
    public Dictionary<int, ModSeidMeta> StaticSkillSeidMetas { get; set; }
    public List<ModAttackType> AttackTypes { get; set; }
    public List<ModStaticSkillType> StaticSkillTypes { get; set; }
    public List<ModElementType> ElementTypes { get; set; }
    public List<ModComparisonOperatorType> ComparisonOperatorTypes { get; set; }
    public List<ModArithmeticOperatorType> ArithmeticOperatorTypes { get; set; }
    public List<ModTargetType> TargetTypes { get; set; }
    /// <summary>
    /// 法宝类型组
    /// </summary>
    public ModItemDataArtifactTypeGroup ItemDataArtifactTypeGroup { get; set; }
    public List<ModSkillDataQuality> SkillDataQuality { get; set; }
    public List<ModSkillDataPhase> SkillDataPhase { get; set; }
    public List<ModSkillDataConsultType> SkillDataConsultTypes { get; set; }
    public List<ModSkillDataAttackScriptType> SkillDataAttackScriptTypes { get; set; }

    
    // Data
    public ModProject ReferenceProject { get; set; }
    public Dictionary<string, FFlowchart> DefaultFFlowchart { get; set; }
    public bool IsInit { get; private set; }

    static ModEditorManager()
    {
        I = new ModEditorManager();
    }

    public void Init()
    {
        if (!IsInit)
        {
            foreach (var type in ModUtils.GetTypesWithAttribute(Assembly.GetAssembly(typeof(ModDataInitAttribute)),
                         typeof(ModDataInitAttribute)))
            {
                var initMethod = type.GetMethod("Init", BindingFlags.Static | BindingFlags.Public);
                initMethod.Invoke(null, Array.Empty<object>());
            }

            AffixDataProjectTypes = JArray
                .Parse(ModUtils.LoadEditorConfig("Meta/AffixProjectType.json"))
                .ToObject<List<ModAffixDataProjectType>>();
            AffixDataAffixTypes = JArray
                .Parse(ModUtils.LoadEditorConfig("Meta/AffixType.json"))
                .ToObject<List<ModAffixDataAffixType>>();
            CreateAvatarSeidMetas = JObject
                .Parse(ModUtils.LoadEditorConfig("Meta/CreateAvatarSeidMeta.json"))
                .ToObject<Dictionary<int, ModSeidMeta>>();
            CreateAvatarDataTalentTypes = JArray
                .Parse(ModUtils.LoadEditorConfig("Meta/CreateAvatarTalentType.json"))
                .ToObject<List<ModCreateAvatarDataTalentType>>();
            LevelTypes = JArray
                .Parse(ModUtils.LoadEditorConfig("Meta/LevelType.json"))
                .ToObject<List<ModLevelType>>();
            BuffSeidMetas = JObject
                .Parse(ModUtils.LoadEditorConfig("Meta/BuffSeidMeta.json"))
                .ToObject<Dictionary<int, ModSeidMeta>>();
            BuffDataBuffTypes = JArray
                .Parse(ModUtils.LoadEditorConfig("Meta/BuffType.json"))
                .ToObject<List<ModBuffDataBuffType>>();
            BuffDataTriggerTypes = JArray
                .Parse(ModUtils.LoadEditorConfig("Meta/BuffTriggerType.json"))
                .ToObject<List<ModBuffDataTriggerType>>();
            BuffDataRemoveTriggerTypes = JArray
                .Parse(ModUtils.LoadEditorConfig("Meta/BuffRemoveTriggerType.json"))
                .ToObject<List<ModBuffDataRemoveTriggerType>>();
            BuffDataOverlayTypes = JArray
                .Parse(ModUtils.LoadEditorConfig("Meta/BuffOverlayType.json"))
                .ToObject<List<ModBuffDataOverlayType>>();
            GuideTypes = JArray
                .Parse(ModUtils.LoadEditorConfig("Meta/GuideType.json"))
                .ToObject<List<ModGuideType>>();
            ItemDataShopTypes = JArray
                .Parse(ModUtils.LoadEditorConfig("Meta/ItemShopType.json"))
                .ToObject<List<ModItemDataShopType>>();
            ItemDataQualityTypes = JArray
                .Parse(ModUtils.LoadEditorConfig("Meta/ItemQualityType.json"))
                .ToObject<List<ModItemDataQualityType>>();
            ItemDataPhaseTypes = JArray
                .Parse(ModUtils.LoadEditorConfig("Meta/ItemPhaseType.json"))
                .ToObject<List<ModItemDataPhaseType>>();
            ItemDataTypes = JArray
                .Parse(ModUtils.LoadEditorConfig("Meta/ItemType.json"))
                .ToObject<List<ModItemDataType>>();
            ItemEquipSeidMetas = JObject
                .Parse(ModUtils.LoadEditorConfig("Meta/ItemEquipSeidMeta.json"))
                .ToObject<Dictionary<int, ModSeidMeta>>();
            ItemUseSeidMetas = JObject
                .Parse(ModUtils.LoadEditorConfig("Meta/ItemUseSeidMeta.json"))
                .ToObject<Dictionary<int, ModSeidMeta>>();
            ItemDataUseTypes = JArray
                .Parse(ModUtils.LoadEditorConfig("Meta/ItemUseType.json"))
                .ToObject<List<ModItemDataUseType>>();
            SkillSeidMetas = JObject
                .Parse(ModUtils.LoadEditorConfig("Meta/SkillSeidMeta.json"))
                .ToObject<Dictionary<int, ModSeidMeta>>();
            StaticSkillSeidMetas = JObject
                .Parse(ModUtils.LoadEditorConfig("Meta/StaticSkillSeidMeta.json"))
                .ToObject<Dictionary<int, ModSeidMeta>>();
            AttackTypes = JArray
                .Parse(ModUtils.LoadEditorConfig("Meta/AttackType.json"))
                .ToObject<List<ModAttackType>>();
            StaticSkillTypes = JArray
                .Parse(ModUtils.LoadEditorConfig("Meta/StaticSkillType.json"))
                .ToObject<List<ModStaticSkillType>>();
            ElementTypes = JArray
                .Parse(ModUtils.LoadEditorConfig("Meta/ElementType.json"))
                .ToObject<List<ModElementType>>();
            ComparisonOperatorTypes = JArray
                .Parse(ModUtils.LoadEditorConfig("Meta/ComparisonOperatorType.json"))
                .ToObject<List<ModComparisonOperatorType>>();
            ArithmeticOperatorTypes = JArray
                .Parse(ModUtils.LoadEditorConfig("Meta/ArithmeticOperatorType.json"))
                .ToObject<List<ModArithmeticOperatorType>>();
            TargetTypes = JArray
                .Parse(ModUtils.LoadEditorConfig("Meta/TargetType.json"))
                .ToObject<List<ModTargetType>>();
            ItemDataArtifactTypeGroup = JObject
                .Parse(ModUtils.LoadEditorConfig("Meta/ArtifactTypeGroup.json"))
                .ToObject<ModItemDataArtifactTypeGroup>();
            SkillDataQuality = JArray
                .Parse(ModUtils.LoadEditorConfig("Meta/SkillQuality.json"))
                .ToObject<List<ModSkillDataQuality>>();
            SkillDataPhase = JArray
                .Parse(ModUtils.LoadEditorConfig("Meta/SkillPhase.json"))
                .ToObject<List<ModSkillDataPhase>>();
            SkillDataConsultTypes = JArray
                .Parse(ModUtils.LoadEditorConfig("Meta/SkillConsultType.json"))
                .ToObject<List<ModSkillDataConsultType>>();
            SkillDataAttackScriptTypes = JArray
                .Parse(ModUtils.LoadEditorConfig("Meta/SkillAttackScriptType.json"))
                .ToObject<List<ModSkillDataAttackScriptType>>();
        }

        IsInit = true;
    }

    public void LoadDefaultData()
    {
        if (!Directory.Exists(Main.PathExportOutputDir.Value))
        {
            return;
        }
        ReferenceProject = CreateReferencedProject();
        DefaultFFlowchart = FFlowchartTools.ImportAllFFlowchart(ModUtils.GetFungusDataPath());
    }

    public ModProject CreateProject(string path)
    {
        var project = ModProject.Create(path);
        return project;
    }
    public ModProject CreateReferencedProject()
    {
        var project = ModProject.Load(ModUtils.GetBasePath() ,ModUtils.GetBasePath(), true);
        project.ProjectName = "参考数据".I18NTodo();
        return project;
    }

    public ModProject OpenProject(string path)
    {
        var project = ModProject.Load(path);
        return project;
    }

    public void SaveProject(ModProject modProject, string path = null)
    {
        if (path == null)
            path = modProject.ProjectPath;
        ModProject.Save(path, modProject);
    }

    public ModWorkshop CreateWorkshop(string path, string modName = null)
    {
        var workShopItem = new WorkShopItem();
        workShopItem.Title = modName ?? string.Empty;
        workShopItem.SteamID = SteamUser.GetSteamID().m_SteamID;
        workShopItem.ModPath = path;

        var mod = new ModWorkshop(workShopItem, path);
        return mod;
    }

    public ModWorkshop LoadWorkshop(string path)
    {
        WorkShopItem modInfo = null;
        modInfo = ModManager.ReadConfig(path);
        var mod = new ModWorkshop(modInfo, path);
        return mod;
    }
        
    public void SaveWorkshop(ModWorkshop mod, string path = null)
    {
        if (path == null)
            path = mod.Path;
        var saveMod = new ModWorkshop(mod.ModInfo, path);
        saveMod.Projects.AddRange(mod.Projects);
        saveMod.Save();
    }

    public string GetCreateAvatarTalentTypeDesc(int relationNum)
    {
        return CreateAvatarDataTalentTypes.FirstOrDefault(type => type.TypeID == relationNum)?.Desc ??
               "Main.Unknown".I18N();
    }
    
    
    private HashSet<int> _itemGuideTypes = new() { 0, 1, 2, 3, 4};
    /// <summary>
    /// 获取物品使用的图鉴类型
    /// </summary>
    /// <returns></returns>
    public List<ModGuideType> GetItemGuideTypes()
    {
        return GuideTypes.Where(type => _itemGuideTypes.Contains(type.Id) ).ToList();
    }
    
    private HashSet<int> _skillGuideTypes = new() { 0, 6, 8, 9 };
    /// <summary>
    /// 获取神通使用的图鉴类型
    /// </summary>
    /// <returns></returns>
    public List<ModGuideType> GetSkillGuideTypes()
    {
        return GuideTypes.Where(type => _skillGuideTypes.Contains(type.Id) ).ToList();
    }
    
    private HashSet<int> _staticSkillGuideTypes = new() { 0, 7};
    /// <summary>
    /// 获取功法使用的图鉴类型
    /// </summary>
    /// <returns></returns>
    public List<ModGuideType> GetStaticSkillGuideTypes()
    {
        return GuideTypes.Where(type => _staticSkillGuideTypes.Contains(type.Id) ).ToList();
    }
}