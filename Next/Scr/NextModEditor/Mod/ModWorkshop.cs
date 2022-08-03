using System.Collections.Generic;
using System.IO;
using script.Steam;
using SkySwordKill.NextModEditor.Mod.Data;

namespace SkySwordKill.NextEditor.Mod
{
    public class ModWorkshop
    {
        public string Path { get; }
        public string ModInfoPath => $"{Path}/Mod.bin";
        public string ProjectDirPath => $"{Path}/plugins/Next";
        public WorkShopItem ModInfo { get; }
        public List<ModProject> Projects { get; } = new List<ModProject>();
        
        public ModWorkshop(WorkShopItem modInfo, string path)
        {
            ModInfo = modInfo ?? new WorkShopItem();
            Path = new FileInfo(path).FullName;
        }
        
        public void Save()
        {
            SaveWorkshopItem();
            SaveProjects();
        }

        public void SaveWorkshopItem()
        {
            ModUtils.WriteConfig(ModInfoPath, ModInfo);
        }
        
        public void SaveProjects()
        {
            Directory.CreateDirectory(ProjectDirPath);
            foreach (var project in Projects)
            {
                ModEditorManager.I.SaveProject(project, $"{ProjectDirPath}/{project.ProjectPathName}");
            }
        }

        public void LoadProjects()
        {
            if(Directory.Exists(ProjectDirPath))
            {
                foreach (var dir in Directory.GetDirectories(ProjectDirPath))
                {
                    var project = ModEditorManager.I.OpenProject(dir);
                    if (project != null)
                    {
                        Projects.Add(project);
                    }
                }
            }
        }
        
        public ModProject CreateProject(string name)
        {
            var newPath = System.IO.Path.Combine(ProjectDirPath, name);
            Directory.CreateDirectory(newPath);
            var project = ModEditorManager.I.CreateProject(newPath);
            Projects.Add(project);
            return project;
        }
        
        public void RemoveProject(ModProject project)
        {
            Projects.Remove(project);
        }
        
        public ModBuffData FindBuff(int buffID)
        {
            ModBuffData buffData = null;
            foreach (var project in Projects)
            {
                buffData = project.FindBuff(buffID);
                if(buffData != null)
                {
                    return buffData;
                }
            }

            ModEditorManager.I.DefaultBuffData.TryGetValue(buffID, out buffData);

            return buffData;
        }

        public List<ModBuffData> GetAllBuffData(bool containRefData = false)
        {
            Dictionary<int, ModBuffData> buffs = new Dictionary<int, ModBuffData>();

            if (containRefData)
            {
                foreach (var pair in ModEditorManager.I.DefaultBuffData)
                {
                    buffs.Add(pair.Key, pair.Value);
                }
            }
            
            foreach (var project in Projects)
            {
                foreach (var buffData in project.BuffData)
                {
                    buffs[buffData.Id] = buffData;
                }
            }

            var buffList = new List<ModBuffData>(buffs.Values);
            buffList.ModSort();
            return buffList;
        }
    }
}