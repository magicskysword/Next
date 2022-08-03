using System.Collections.Generic;
using System.IO;
using System.Linq;
using SkySwordKill.Next.Mod;
using SkySwordKill.NextModEditor.Mod.Data;

namespace SkySwordKill.NextEditor.Mod
{
    public class ModProject
    {
        public string ProjectPath { get; set; }
        public string ProjectPathName => Path.GetFileName(ProjectPath);
        public ModConfig Config { get; set; }

        public List<ModCreateAvatarData> CreateAvatarData { get; set; }
        public ModCreateAvatarSeidDataGroup CreateAvatarSeidDataGroup { get; set; }
        public List<ModBuffData> BuffData { get; set; }
        public List<ModItemData> ItemData { get; set; }
        public ModBuffSeidDataGroup BuffSeidDataGroup { get; set; }
        public List<ModAffixData> AffixData { get; set; }

        private ModProject()
        {
            
        }

        public ModAffixData FindAffix(int affixID)
        {
            var affixData = AffixData.Find(data => data.Id == affixID);

            if (affixData == null)
            {
                ModEditorManager.I.DefaultAffixData.TryGetValue(affixID, out affixData);
            }

            return affixData;
        }

        public List<ModAffixData> GetAllAffix()
        {
            var list = new List<ModAffixData>();
            list.AddRange(ModEditorManager.I.DefaultAffixData.Values);
            foreach (var affixData in AffixData)
            {
                if (list.Find(data => data.Id == affixData.Id) == null)
                    list.Add(affixData);
            }

            list.Sort((dataA, dataB) => dataA.Id.CompareTo(dataB.Id));
            return list;
        }

        public ModBuffData FindBuff(int buffID)
        {
            var buffData = BuffData.Find(data => data.Id == buffID);
            return buffData;
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
                BuffSeidDataGroup = ModBuffSeidDataGroup.Create(ModEditorManager.I.BuffSeidMetas),
                AffixData = new List<ModAffixData>(),
            };

            return project;
        }
        
        public static ModProject Load(string dir)
        {
            var config = ModConfig.Load(dir);

            ModProject project = new ModProject
            {
                ProjectPath = dir,
                Config = config,
                CreateAvatarData = ModCreateAvatarData.Load(config.GetDataDir())?.Select(pair => pair.Value).ToList(),
                CreateAvatarSeidDataGroup =
                    ModCreateAvatarSeidDataGroup.Load(config.GetDataDir(), ModEditorManager.I.CreateAvatarSeidMetas),
                BuffData = ModBuffData.Load(config.GetDataDir()),
                ItemData = ModItemData.Load(config.GetDataDir()),
                BuffSeidDataGroup = ModBuffSeidDataGroup.Load(config.GetDataDir(), ModEditorManager.I.BuffSeidMetas),
                AffixData = ModAffixData.Load(config.GetDataDir()).Select(pair => pair.Value).ToList(),
            };

            project.CreateAvatarData.ModSort();
            project.BuffData.ModSort();
            project.ItemData.ModSort();
            project.AffixData.ModSort();

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
            ModBuffSeidDataGroup.Save(config.GetDataDir(), project.BuffSeidDataGroup);
            ModItemData.Save(config.GetDataDir(), project.ItemData);
            ModAffixData.Save(config.GetDataDir(), project.AffixData.ToDictionary(item => item.Id.ToString()));
        }
    }
}