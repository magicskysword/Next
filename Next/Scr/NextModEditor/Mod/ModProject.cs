using System.Collections.Generic;
using System.IO;
using System.Linq;
using SkySwordKill.Next.Mod;
using SkySwordKill.NextModEditor.Mod.Data;

namespace SkySwordKill.NextEditor.Mod
{
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
        public List<ModSkillData> SkillData { get; set; }

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
        public ModSkillData FindSkillBySkillId(int id)
        {
            var list = SkillData.FindAll(data => data.SkillId == id);
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
                SkillData = new List<ModSkillData>()
            };
            
            return project;
        }
        
        public static ModProject Load(string dir, string dataDir = null)
        {
            var config = ModConfig.Load(dir);
            dataDir = dataDir ?? config.GetDataDir();

            ModProject project = new ModProject
            {
                ProjectPath = dir,
                Config = config,
                CreateAvatarData = ModCreateAvatarData.Load(dataDir)?.Select(pair => pair.Value).ToList(),
                CreateAvatarSeidDataGroup =
                    ModCreateAvatarSeidDataGroup.Load(dataDir, ModEditorManager.I.CreateAvatarSeidMetas),
                BuffData = ModBuffData.Load(dataDir),
                ItemData = ModItemData.Load(dataDir),
                ItemFlagData = ModItemFlagData.Load(dataDir)?.Select(pair => pair.Value).ToList(),
                ItemEquipSeidDataGroup = ModItemEquipSeidDataGroup.Load(dataDir, ModEditorManager.I.ItemEquipSeidMetas),
                ItemUseSeidDataGroup = ModItemUseSeidDataGroup.Load(dataDir, ModEditorManager.I.ItemUseSeidMetas),
                BuffSeidDataGroup = ModBuffSeidDataGroup.Load(dataDir, ModEditorManager.I.BuffSeidMetas),
                AffixData = ModAffixData.Load(dataDir).Select(pair => pair.Value).ToList(),
                ForgeProperty = ModForgePropertyData.Load(dataDir).Select(pair => pair.Value).ToList(),
                ForgeElement = ModForgeElementData.Load(dataDir).Select(pair => pair.Value).ToList(),
                AlchemyElement = ModAlchemyElementData.Load(dataDir).Select(pair => pair.Value).ToList(),
                Comprehension = ModComprehensionData.Load(dataDir).Select(pair => pair.Value).ToList(),
                ComprehensionPhase = ModComprehensionPhaseData.Load(dataDir).Select(pair => pair.Value).ToList(),
                SkillData = ModSkillData.Load(dataDir)
            };

            project.CreateAvatarData.ModSort();
            project.BuffData.ModSort();
            project.ItemData.ModSort();
            project.AffixData.ModSort();
            project.SkillData.ModSort();

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
                project.CreateAvatarData.ToDictionary(item => item.Id.ToString()));
            ModCreateAvatarSeidDataGroup.Save(config.GetDataDir(), project.CreateAvatarSeidDataGroup);
            ModBuffData.Save(config.GetDataDir(), project.BuffData);
            ModItemData.Save(config.GetDataDir(), project.ItemData);
            ModItemFlagData.Save(config.GetDataDir(), project.ItemFlagData.ToDictionary(item => item.Id.ToString()));
            ModItemEquipSeidDataGroup.Save(config.GetDataDir(), project.ItemEquipSeidDataGroup);
            ModItemUseSeidDataGroup.Save(config.GetDataDir(), project.ItemUseSeidDataGroup);
            ModBuffSeidDataGroup.Save(config.GetDataDir(), project.BuffSeidDataGroup);
            ModAffixData.Save(config.GetDataDir(), project.AffixData.ToDictionary(data => data.Id.ToString()));
            ModForgePropertyData.Save(config.GetDataDir(), project.ForgeProperty.ToDictionary(data => data.Id.ToString()));
            ModForgeElementData.Save(config.GetDataDir(), project.ForgeElement.ToDictionary(data => data.Id.ToString()));
            ModAlchemyElementData.Save(config.GetDataDir(), project.AlchemyElement.ToDictionary(data => data.Id.ToString()));
            ModComprehensionData.Save(config.GetDataDir(), project.Comprehension.ToDictionary(data => data.Id.ToString()));
            ModComprehensionPhaseData.Save(config.GetDataDir(), project.ComprehensionPhase.ToDictionary(data => data.Id.ToString()));
            ModSkillData.Save(config.GetDataDir(), project.SkillData);
        }
    }
}