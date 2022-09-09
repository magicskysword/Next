using System.Collections.Generic;
using System.IO;
using script.Steam;
using SkySwordKill.Next.FGUI;
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
                ModEditorManager.I.SaveProject(project, $"{ProjectDirPath}/{project.ProjectDirectory}");
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
            ModBuffData data = null;
            foreach (var project in Projects)
            {
                data = project.FindBuff(buffID);
                if(data != null)
                {
                    return data;
                }
            }

            return ModEditorManager.I.ReferenceProject.FindBuff(buffID);
        }
        
        public ModAffixData FindAffix(int id)
        {
            ModAffixData data = null;
            foreach (var project in Projects)
            {
                data = project.FindAffix(id);
                if(data != null)
                {
                    return data;
                }
            }
            
            return ModEditorManager.I.ReferenceProject.FindAffix(id);
        }
        
        public ModItemData FindItem(int id)
        {
            ModItemData data = null;
            foreach (var project in Projects)
            {
                data = project.FindItem(id);
                if(data != null)
                {
                    return data;
                }
            }
            
            return ModEditorManager.I.ReferenceProject.FindItem(id);
        }
        
        public ModItemFlagData FindItemFlag(int id)
        {
            ModItemFlagData data = null;
            foreach (var project in Projects)
            {
                data = project.FindItemFlag(id);
                if(data != null)
                {
                    return data;
                }
            }
            
            return ModEditorManager.I.ReferenceProject.FindItemFlag(id);
        }
        
        public ModSkillData FindSkill(int id)
        {
            ModSkillData data = null;
            foreach (var project in Projects)
            {
                data = project.FindSkill(id);
                if(data != null)
                {
                    return data;
                }
            }
            
            return ModEditorManager.I.ReferenceProject.FindSkill(id);
        }
        
        public ModSkillData FindSkillBySkillId(int id)
        {
            ModSkillData data = null;
            foreach (var project in Projects)
            {
                data = project.FindSkillBySkillId(id);
                if(data != null)
                {
                    return data;
                }
            }
            
            return ModEditorManager.I.ReferenceProject.FindSkillBySkillId(id);
        }
        
        public ModComprehensionData FindComprehension(int id)
        {
            ModComprehensionData data = null;
            foreach (var project in Projects)
            {
                data = project.FindComprehension(id);
                if(data != null)
                {
                    return data;
                }
            }
            
            return ModEditorManager.I.ReferenceProject.FindComprehension(id);
        }
        
        public ModComprehensionPhaseData FindComprehensionPhase(int id)
        {
            ModComprehensionPhaseData data = null;
            foreach (var project in Projects)
            {
                data = project.FindComprehensionPhase(id);
                if(data != null)
                {
                    return data;
                }
            }
            
            return ModEditorManager.I.ReferenceProject.FindComprehensionPhase(id);
        }
        
        public ModAlchemyElementData FindAlchemyElement(int id)
        {
            ModAlchemyElementData data = null;
            foreach (var project in Projects)
            {
                data = project.FindAlchemyElement(id);
                if(data != null)
                {
                    return data;
                }
            }
            
            return ModEditorManager.I.ReferenceProject.FindAlchemyElement(id);
        }

        public List<ModBuffData> GetAllBuffData(bool containRefData = true)
        {
            Dictionary<int, ModBuffData> buffs = new Dictionary<int, ModBuffData>();

            if (containRefData)
            {
                foreach (var buffData in ModEditorManager.I.ReferenceProject.BuffData)
                {
                    buffs.Add(buffData.Id, buffData);
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
        
        public List<ModAffixData> GetAllAffixData(bool containRefData = true)
        {
            Dictionary<int, ModAffixData> dataDic = new Dictionary<int, ModAffixData>();

            if (containRefData)
            {
                foreach (var buffData in ModEditorManager.I.ReferenceProject.AffixData)
                {
                    dataDic.Add(buffData.Id, buffData);
                }
            }
            
            foreach (var project in Projects)
            {
                foreach (var buffData in project.AffixData)
                {
                    dataDic[buffData.Id] = buffData;
                }
            }

            var dataList = new List<ModAffixData>(dataDic.Values);
            dataList.ModSort();
            return dataList;
        }

        public List<ModItemData> GetAllItemData(bool containRefData = true)
        {
            Dictionary<int, ModItemData> dataDic = new Dictionary<int, ModItemData>();

            if (containRefData)
            {
                foreach (var data in ModEditorManager.I.ReferenceProject.ItemData)
                {
                    dataDic.Add(data.Id, data);
                }
            }
            
            foreach (var project in Projects)
            {
                foreach (var data in project.ItemData)
                {
                    dataDic[data.Id] = data;
                }
            }

            var dataList = new List<ModItemData>(dataDic.Values);
            dataList.ModSort();
            return dataList;
        }

        public List<ModItemFlagData> GetAllItemFlagData(bool containRefData = true)
        {
            Dictionary<int, ModItemFlagData> dataDic = new Dictionary<int, ModItemFlagData>();

            if (containRefData)
            {
                foreach (var data in ModEditorManager.I.ReferenceProject.ItemFlagData)
                {
                    dataDic.Add(data.Id, data);
                }
            }
            
            foreach (var project in Projects)
            {
                foreach (var data in project.ItemFlagData)
                {
                    dataDic[data.Id] = data;
                }
            }

            var dataList = new List<ModItemFlagData>(dataDic.Values);
            dataList.ModSort();
            return dataList;
        }

        public List<ModForgeElementData> GetAllForgeElementData(bool containRefData = true)
        {
            Dictionary<int, ModForgeElementData> dataDic = new Dictionary<int, ModForgeElementData>();

            if (containRefData)
            {
                foreach (var data in ModEditorManager.I.ReferenceProject.ForgeElement)
                {
                    dataDic.Add(data.Id, data);
                }
            }
            
            foreach (var project in Projects)
            {
                foreach (var data in project.ForgeElement)
                {
                    dataDic[data.Id] = data;
                }
            }

            var dataList = new List<ModForgeElementData>(dataDic.Values);
            dataList.ModSort();
            return dataList;
        }
        
        public List<ModForgePropertyData> GetAllForgePropertyData(bool containRefData = true)
        {
            Dictionary<int, ModForgePropertyData> dataDic = new Dictionary<int, ModForgePropertyData>();

            if (containRefData)
            {
                foreach (var data in ModEditorManager.I.ReferenceProject.ForgeProperty)
                {
                    dataDic.Add(data.Id, data);
                }
            }
            
            foreach (var project in Projects)
            {
                foreach (var data in project.ForgeProperty)
                {
                    dataDic[data.Id] = data;
                }
            }

            var dataList = new List<ModForgePropertyData>(dataDic.Values);
            dataList.ModSort();
            return dataList;
        }
        
        public List<ModAlchemyElementData> GetAllAlchemyElementData(bool containRefData = true)
        {
            Dictionary<int, ModAlchemyElementData> dataDic = new Dictionary<int, ModAlchemyElementData>();

            if (containRefData)
            {
                foreach (var data in ModEditorManager.I.ReferenceProject.AlchemyElement)
                {
                    dataDic.Add(data.Id, data);
                }
            }
            
            foreach (var project in Projects)
            {
                foreach (var data in project.AlchemyElement)
                {
                    dataDic[data.Id] = data;
                }
            }

            var dataList = new List<ModAlchemyElementData>(dataDic.Values);
            dataList.ModSort();
            return dataList;
        }
        
        public List<ModComprehensionData> GetAllComprehensionData(bool containRefData = true)
        {
            Dictionary<int, ModComprehensionData> dataDic = new Dictionary<int, ModComprehensionData>();

            if (containRefData)
            {
                foreach (var data in ModEditorManager.I.ReferenceProject.Comprehension)
                {
                    dataDic.Add(data.Id, data);
                }
            }
            
            foreach (var project in Projects)
            {
                foreach (var data in project.Comprehension)
                {
                    dataDic[data.Id] = data;
                }
            }

            var dataList = new List<ModComprehensionData>(dataDic.Values);
            dataList.ModSort();
            return dataList;
        }
        
        public List<ModSkillData> GetAllSkillData(bool containRefData = true)
        {
            Dictionary<int, ModSkillData> dataDic = new Dictionary<int, ModSkillData>();

            if (containRefData)
            {
                foreach (var data in ModEditorManager.I.ReferenceProject.SkillData)
                {
                    dataDic.Add(data.Id, data);
                }
            }
            
            foreach (var project in Projects)
            {
                foreach (var data in project.SkillData)
                {
                    dataDic[data.Id] = data;
                }
            }

            var dataList = new List<ModSkillData>(dataDic.Values);
            dataList.ModSort();
            return dataList;
        }
        
        public string GetBuffIconUrl(ModBuffData buff)
        {
            if (buff.Icon == 0)
            {
                return GetImageUrl($"Buff Icon/{buff.Id}");
            }
            
            return GetImageUrl($"Buff Icon/{buff.Icon}");
        }
        
        public string GetItemIconUrl(ModItemData item)
        {
            if (item.Icon == 0)
            {
                return GetImageUrl($"Item Icon/{item.Id}");
            }
            
            return GetImageUrl($"Item Icon/{item.Icon}");
        }
        
        public string GetImageUrl(string resPath)
        {
            foreach (var project in Projects)
            {
                if (project.TryGetFilePath($"{resPath}.png", out var path))
                {
                    return $"{NextGLoader.UI_FILE_PREFIX}{path}";
                }
            }
            
            return resPath;
        }
    }
}