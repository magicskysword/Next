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
    public List<ModItemDataGuideType> ItemDataGuideTypes { get; set; }
    public List<ModItemDataShopType> ItemDataShopTypes { get; set; }
    public List<ModItemDataQualityType> ItemDataQualityTypes { get; set; }
    public List<ModItemDataPhaseType> ItemDataPhaseTypes { get; set; }
    public List<ModItemDataType> ItemDataTypes { get; set; }
    public Dictionary<int, ModSeidMeta> ItemEquipSeidMetas { get; set; }
    public Dictionary<int, ModSeidMeta> ItemUseSeidMetas { get; set; }
    public List<ModItemDataUseType> ItemDataUseTypes { get; set; }
    public Dictionary<int, ModSeidMeta> SkillSeidMetas { get; set; }
    public List<ModAttackType> AttackTypes { get; set; }
    public List<ModElementType> ElementTypes { get; set; }
    public List<ModComparisonOperatorType> ComparisonOperatorTypes { get; set; }
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
            LevelTypes = JArray
                .Parse(ModUtils.LoadConfig("Meta/LevelType.json"))
                .ToObject<List<ModLevelType>>();
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
            ItemDataShopTypes = JArray
                .Parse(ModUtils.LoadConfig("Meta/ItemShopType.json"))
                .ToObject<List<ModItemDataShopType>>();
            ItemDataQualityTypes = JArray
                .Parse(ModUtils.LoadConfig("Meta/ItemQualityType.json"))
                .ToObject<List<ModItemDataQualityType>>();
            ItemDataPhaseTypes = JArray
                .Parse(ModUtils.LoadConfig("Meta/ItemPhaseType.json"))
                .ToObject<List<ModItemDataPhaseType>>();
            ItemDataTypes = JArray
                .Parse(ModUtils.LoadConfig("Meta/ItemType.json"))
                .ToObject<List<ModItemDataType>>();
            ItemEquipSeidMetas = JObject
                .Parse(ModUtils.LoadConfig("Meta/ItemEquipSeidMeta.json"))
                .ToObject<Dictionary<int, ModSeidMeta>>();
            ItemUseSeidMetas = JObject
                .Parse(ModUtils.LoadConfig("Meta/ItemUseSeidMeta.json"))
                .ToObject<Dictionary<int, ModSeidMeta>>();
            ItemDataUseTypes = JArray
                .Parse(ModUtils.LoadConfig("Meta/ItemUseType.json"))
                .ToObject<List<ModItemDataUseType>>();
            SkillSeidMetas = JObject
                .Parse(ModUtils.LoadConfig("Meta/SkillSeidMeta.json"))
                .ToObject<Dictionary<int, ModSeidMeta>>();
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
            ItemDataArtifactTypeGroup = JObject
                .Parse(ModUtils.LoadConfig("Meta/ArtifactTypeGroup.json"))
                .ToObject<ModItemDataArtifactTypeGroup>();
            SkillDataQuality = JArray
                .Parse(ModUtils.LoadConfig("Meta/SkillQuality.json"))
                .ToObject<List<ModSkillDataQuality>>();
            SkillDataPhase = JArray
                .Parse(ModUtils.LoadConfig("Meta/SkillPhase.json"))
                .ToObject<List<ModSkillDataPhase>>();
            SkillDataConsultTypes = JArray
                .Parse(ModUtils.LoadConfig("Meta/SkillConsultType.json"))
                .ToObject<List<ModSkillDataConsultType>>();
            SkillDataAttackScriptTypes = JArray
                .Parse(ModUtils.LoadConfig("Meta/SkillAttackScriptType.json"))
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
        if (File.Exists($"{path}/Mod.bin"))
            modInfo = ModUtils.ReadConfig(path);
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
}