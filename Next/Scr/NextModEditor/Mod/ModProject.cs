using System.Collections.Generic;
using System.IO;
using System.Linq;
using SkySwordKill.Next.Mod;
using SkySwordKill.NextModEditor.Mod.Data;

namespace SkySwordKill.NextModEditor.Mod;

public class ModProject
{
    private string _path;

    public string ProjectPath
    {
        get => _path;
        set
        {
            _path = value;
            ProjectDirectory = Path.GetFileName(_path);
        }
    }

    private string _projectName;
        
    public string ProjectDirectory { get; set; }
    public string ProjectName
    {
        get => _projectName ?? ProjectDirectory;
        set
        {
            if (string.IsNullOrEmpty(value))
                _projectName = null;
            _projectName = value;
        }
    }
    public ModConfig Config { get; set; }

    public List<ModCreateAvatarData> CreateAvatarData { get; set; }
    public ModCreateAvatarSeidDataGroup CreateAvatarSeidDataGroup { get; set; }
    public List<ModBuffData> BuffData { get; set; }
    public List<ModItemData> ItemData { get; set; }
    public List<ModItemFlagData> ItemFlagData { get; set; }
    public ModItemEquipSeidDataGroup ItemEquipSeidDataGroup { get; set; }
    public ModBuffSeidDataGroup BuffSeidDataGroup { get; set; }
    public List<ModAffixData> AffixData { get; set; }
    public List<ModForgePropertyData> ForgeProperty { get; set; }
    public List<ModForgeElementData> ForgeElement { get; set; }
    public List<ModAlchemyElementData> AlchemyElement { get; set; }
    public List<ModComprehensionData> Comprehension { get; set; }
    public List<ModComprehensionPhaseData> ComprehensionPhase { get; set; }
    public ModItemUseSeidDataGroup ItemUseSeidDataGroup { get; set; }
    /// <summary>
    /// 神通数据
    /// </summary>
    public List<ModSkillData> SkillData { get; set; }
    /// <summary>
    /// 功法数据
    /// </summary>
    public List<ModStaticSkillData> StaticSkillData { get; set; }
    /// <summary>
    /// 技能Seid数据
    /// </summary>
    public ModSkillSeidDataGroup SkillSeidDataGroup { get; set; }
    /// <summary>
    /// 神通Seid信息
    /// </summary>
    public ModStaticSkillSeidDataGroup StaticSkillSeidDataGroup { get; set; }

    private ModProject()
    {
            
    }

    public ModAffixData FindAffix(int affixID)
    {
        var affixData = AffixData.Find(data => data.Id == affixID);

        return affixData;
    }
        
    public ModItemData FindItem(int id)
    {
        var itemData = ItemData.Find(data => data.Id == id);

        return itemData;
    }
        
    public ModItemFlagData FindItemFlag(int id)
    {
        var flagData = ItemFlagData.Find(data => data.Id == id);

        return flagData;
    }

    /// <summary>
    /// 根据技能Id返回技能
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public ModSkillData FindSkill(int id)
    {
        var skillData = SkillData.Find(data => data.Id == id);

        return skillData;
    }
        
    /// <summary>
    /// 根据技能Id返回技能，默认返回等级最高技能
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public ModSkillData FindSkillBySkillPkId(int id)
    {
        var list = SkillData.FindAll(data => data.SkillPkId == id);
        return list.OrderByDescending(data => data.SkillLv).FirstOrDefault();
    }
    
    /// <summary>
    /// 根据神通Id返回神通
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public ModStaticSkillData FindStaticSkill(int id)
    {
        var staticSkillData = StaticSkillData.Find(data => data.Id == id);

        return staticSkillData;
    }

    /// <summary>
    /// 根据神通Id返回神通，默认返回等级最高神通
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public ModStaticSkillData FindStaticSkillBySkillPkId(int id)
    {
        var list = StaticSkillData.FindAll(data => data.SkillPkId == id);
        return list.OrderByDescending(data => data.SkillLv).FirstOrDefault();
    }
        
    public ModBuffData FindBuff(int buffID)
    {
        var buffData = BuffData.Find(data => data.Id == buffID);
        return buffData;
    }
        
    public ModAlchemyElementData FindAlchemyElement(int id)
    {
        var findData = AlchemyElement.Find(data => data.Id == id);
        return findData;
    }
        
    public ModComprehensionData FindComprehension(int id)
    {
        var findData = Comprehension.Find(data => data.Id == id);
        return findData;
    }
        
    public ModComprehensionPhaseData FindComprehensionPhase(int id)
    {
        var findData = ComprehensionPhase.Find(data => data.Id == id);
        return findData;
    }
        
    public bool TryGetFilePath(string path, out string filePath)
    {
        filePath = Path.Combine(Config.GetAssetDir(), path);
        return File.Exists(filePath);
    }

    public static ModProject Create(string dir)
    {
        ModProject project = new ModProject
        {
            ProjectPath = dir,
            Config = new ModConfig(),
            CreateAvatarData = new List<ModCreateAvatarData>(),
            CreateAvatarSeidDataGroup = ModCreateAvatarSeidDataGroup.Create(ModEditorManager.I.CreateAvatarSeidMetas),
            BuffData = new List<ModBuffData>(),
            ItemData = new List<ModItemData>(),
            ItemFlagData = new List<ModItemFlagData>(),
            ItemEquipSeidDataGroup = ModItemEquipSeidDataGroup.Create(ModEditorManager.I.ItemEquipSeidMetas),
            ItemUseSeidDataGroup = ModItemUseSeidDataGroup.Create(ModEditorManager.I.ItemUseSeidMetas),
            BuffSeidDataGroup = ModBuffSeidDataGroup.Create(ModEditorManager.I.BuffSeidMetas),
            AffixData = new List<ModAffixData>(),
            ForgeProperty = new List<ModForgePropertyData>(),
            ForgeElement = new List<ModForgeElementData>(),
            AlchemyElement = new List<ModAlchemyElementData>(),
            Comprehension = new List<ModComprehensionData>(),
            ComprehensionPhase = new List<ModComprehensionPhaseData>(),
            SkillData = new List<ModSkillData>(),
            StaticSkillData = new List<ModStaticSkillData>(),
            SkillSeidDataGroup = ModSkillSeidDataGroup.Create(ModEditorManager.I.SkillSeidMetas),
            StaticSkillSeidDataGroup = ModStaticSkillSeidDataGroup.Create(ModEditorManager.I.StaticSkillSeidMetas),
        };
            
        return project;
    }
        
    public static ModProject Load(string dir, string dataDir = null, bool ignoreWarning = false)
    {
        var config = ModConfig.Load(dir, ignoreWarning);
        dataDir = dataDir ?? config.GetDataDir();

        ModProject project = new ModProject
        {
            ProjectPath = dir,
            Config = config,
            CreateAvatarData = ModCreateAvatarData.Load(dataDir).ToList(),
            CreateAvatarSeidDataGroup =
                ModCreateAvatarSeidDataGroup.Load(dataDir, ModEditorManager.I.CreateAvatarSeidMetas),
            BuffData = ModBuffData.Load(dataDir),
            ItemData = ModItemData.Load(dataDir),
            ItemFlagData = ModItemFlagData.Load(dataDir).ToList(),
            ItemEquipSeidDataGroup = ModItemEquipSeidDataGroup.Load(dataDir, ModEditorManager.I.ItemEquipSeidMetas),
            ItemUseSeidDataGroup = ModItemUseSeidDataGroup.Load(dataDir, ModEditorManager.I.ItemUseSeidMetas),
            BuffSeidDataGroup = ModBuffSeidDataGroup.Load(dataDir, ModEditorManager.I.BuffSeidMetas),
            AffixData = ModAffixData.Load(dataDir).ToList(),
            ForgeProperty = ModForgePropertyData.Load(dataDir).ToList(),
            ForgeElement = ModForgeElementData.Load(dataDir).ToList(),
            AlchemyElement = ModAlchemyElementData.Load(dataDir).ToList(),
            Comprehension = ModComprehensionData.Load(dataDir).ToList(),
            ComprehensionPhase = ModComprehensionPhaseData.Load(dataDir).ToList(),
            SkillData = ModSkillData.Load(dataDir),
            StaticSkillData = ModStaticSkillData.Load(dataDir),
            SkillSeidDataGroup = ModSkillSeidDataGroup.Load(dataDir, ModEditorManager.I.SkillSeidMetas),
            StaticSkillSeidDataGroup = ModStaticSkillSeidDataGroup.Load(dataDir, ModEditorManager.I.StaticSkillSeidMetas),
        };

        project.CreateAvatarData.ModSort();
        project.BuffData.ModSort();
        project.ItemData.ModSort();      
        project.AffixData.ModSort();
        project.SkillData.ModSort();
        project.StaticSkillData.ModSort();

        return project;
    }

    public static void Save(string dir, ModProject project)
    {
        var config = project.Config;
        config.Path = dir;

        Directory.CreateDirectory(config.GetConfigDir());
        Directory.CreateDirectory(config.GetDataDir());
        Directory.CreateDirectory(config.GetNDataDir());
            
        ModConfig.Save(config.GetConfigDir(), project.Config);
        ModCreateAvatarData.Save(config.GetDataDir(),
            project.CreateAvatarData);
        ModCreateAvatarSeidDataGroup.Save(config.GetDataDir(), project.CreateAvatarSeidDataGroup);
        ModBuffData.Save(config.GetDataDir(), project.BuffData);
        ModItemData.Save(config.GetDataDir(), project.ItemData);
        ModItemFlagData.Save(config.GetDataDir(), project.ItemFlagData);
        ModItemEquipSeidDataGroup.Save(config.GetDataDir(), project.ItemEquipSeidDataGroup);
        ModItemUseSeidDataGroup.Save(config.GetDataDir(), project.ItemUseSeidDataGroup);
        ModBuffSeidDataGroup.Save(config.GetDataDir(), project.BuffSeidDataGroup);
        ModAffixData.Save(config.GetDataDir(), project.AffixData);
        ModForgePropertyData.Save(config.GetDataDir(), project.ForgeProperty);
        ModForgeElementData.Save(config.GetDataDir(), project.ForgeElement);
        ModAlchemyElementData.Save(config.GetDataDir(), project.AlchemyElement);
        ModComprehensionData.Save(config.GetDataDir(), project.Comprehension);
        ModComprehensionPhaseData.Save(config.GetDataDir(), project.ComprehensionPhase);
        ModSkillData.Save(config.GetDataDir(), project.SkillData);
        ModStaticSkillData.Save(config.GetDataDir(), project.StaticSkillData);
        ModSkillSeidDataGroup.Save(config.GetDataDir(), project.SkillSeidDataGroup);
        ModStaticSkillSeidDataGroup.Save(config.GetDataDir(), project.StaticSkillSeidDataGroup);
    }
}