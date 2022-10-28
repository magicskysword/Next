using System.Collections.Generic;
using System.IO;
using System.Linq;
using script.Steam;
using SkySwordKill.Next.Extension;

namespace SkySwordKill.Next.Mod;

public class ModGroup
{
    public string GroupName { get; set; }
    public string GroupKey { get; set; }
    public DirectoryInfo ModDir { get; set; }
    public ModType Type { get; set; }
    public List<ModConfig> ModConfigs { get; set; } = new List<ModConfig>();
    public WorkShopItem SteamModInfo { get; set; }
    
    public ModGroup(DirectoryInfo directory, ModType type)
    {
        GroupKey = directory.Name;
        ModDir = directory;
        Type = type;
    }
    
    public void Init(bool resetModState,bool showLog)
    {
        if(resetModState)
        {
            ModConfigs.Clear();
            if (Directory.Exists(ModDir + @"/plugins/Next/"))
            {
                var nextDir = new DirectoryInfo(ModDir + @"/plugins/Next/");
                foreach (DirectoryInfo modDirInfo in nextDir.GetDirectories("mod*", SearchOption.TopDirectoryOnly))
                {
                    ModConfig item = ModManager.LoadModConfig(modDirInfo.FullName, showLog);
                    item.SettingKey = $"{GroupKey}.{Path.GetFileNameWithoutExtension(item.Path)}";
                    ModConfigs.Add(item);
                }
            }
        }
        else
        {
            var newModConfigs = new List<ModConfig>();
            if (Directory.Exists(ModDir + @"/plugins/Next/"))
            {
                var nextDir = new DirectoryInfo(ModDir + @"/plugins/Next/");
                foreach (DirectoryInfo modDirInfo in nextDir.GetDirectories("mod*", SearchOption.TopDirectoryOnly))
                {
                    if (showLog)
                    {
                        Main.LogInfo(string.Format("ModManager.LoadMod".I18N(), modDirInfo.Name));
                    }
                    
                    var item = ModConfigs.Find(x => x.Path == modDirInfo.FullName);
                    if(item == null)
                    {
                        item = ModManager.LoadModConfig(modDirInfo.FullName, showLog);
                        item.SettingKey = $"{GroupKey}.{Path.GetFileNameWithoutExtension(item.Path)}";
                    }
                    else
                    {
                        item.Reload();
                    }
                    newModConfigs.Add(item);
                }
            }
            ModConfigs = newModConfigs;
        }
        SteamModInfo = ModManager.ReadConfig(ModDir.FullName);
        GroupName = SteamModInfo?.Title ?? string.Empty;
        
        ModConfigs = ModManager.SortMod(ModConfigs).ToList();
    }
    
    public void MoveModUp(ModConfig modConfig)
    {
        int index = ModConfigs.IndexOf(modConfig);
        if (index > 0)
        {
            MoveMod(modConfig, index - 1);
        }
    }
    
    public void MoveModDown(ModConfig modConfig)
    {
        int index = ModConfigs.IndexOf(modConfig);
        if (index < ModConfigs.Count - 1)
        {
            MoveMod(modConfig, index + 1);
        }
    }
    
    public void MoveMod(ModConfig mod, int index)
    {
        ModConfigs.Remove(mod);
        ModConfigs.Insert(index, mod);
        ApplyModSetting();
    }

    public void ApplyModSetting()
    {
        var nextModSetting = Main.I.NextModSetting;
        var modIndex = 0;
        foreach (var mod in ModConfigs)
        {
            var modSetting = nextModSetting.GetOrCreateModSetting(mod);
            modSetting.priority = modIndex++;
            if (Type == ModType.Workshop)
            {
                modSetting.enable = true;
            }
        }
    }
}