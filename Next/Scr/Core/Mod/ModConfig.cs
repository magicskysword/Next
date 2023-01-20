using System;
using System.Collections.Generic;
using System.IO;
using BepInEx;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SkySwordKill.Next.Extension;

namespace SkySwordKill.Next.Mod;

public class ModConfig
{
    public string Name { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    [JsonConverter(typeof(ModSettingDefinitionListConverter))]
    public List<ModSettingDefinition> Settings { get; set; } = new();
    
    [JsonIgnore]
    public ModState State { get; set; }
    [JsonIgnore]
    public string Path { get; set; }
    [JsonIgnore]
    public Exception Exception { get; set; }
    /// <summary>
    /// 数据版本
    /// V1版本是老版本
    /// V2版本是新版本
    /// </summary>
    [JsonIgnore]
    public int DataVersion { get; set; } = 2;
    /// <summary>
    /// 设置Key，由ModConfigGroup初始化
    /// </summary>
    [JsonIgnore]
    public string SettingKey { get; set; }
    [JsonIgnore]
    public Dictionary<string, string> JsonPathCache = new Dictionary<string, string>();

    public string GetModStateDescription()
    {
        string modState = string.Empty;
        string colorCode = string.Empty;
            
        switch (State)
        {
            case ModState.Unload:
                modState = "Mod.Load.Unload".I18N();
                colorCode = "#000000";
                break;
            case ModState.Disable:
                modState = "Mod.Load.Disable".I18N();
                colorCode = "#808080";
                break;
            case ModState.Loading:
                modState = "Mod.Load.Loading".I18N();
                colorCode = "#000000";
                break;
            case ModState.LoadSuccess:
                modState = "Mod.Load.Success".I18N();
                colorCode = "#00FFFF";
                break;
            case ModState.LoadFail:
                modState = "Mod.Load.Fail".I18N();
                colorCode = "#FF0000";
                break;
        }

        return $"[color={colorCode}]{modState}[/color]";
    }

    public string GetDataDir()
    {
        return GetDataDir(DataVersion);
    }
        
    /// <summary>
    /// 根据版本号获取游戏数据文件夹
    /// <br/> -1 - 最新版本
    /// <br/> 1 - V1版本
    /// <br/> 2 - V2版本
    /// </summary>
    /// <returns></returns>
    public string GetDataDir(int dataVersion)
    {
        if (dataVersion == 1)
        {
            return Path;
        }

        return $"{Path}/Data";
    }
        
    public string GetNDataDir()
    {
        return GetNDataDir(DataVersion);
    }
        
    /// <summary>
    /// 根据版本号获取Next数据文件夹
    /// <br/> -1 - 最新版本
    /// <br/> 1 - V1版本
    /// <br/> 2 - V2版本
    /// </summary>
    /// <returns></returns>
    public string GetNDataDir(int dataVersion)
    {
        if (dataVersion == 1)
        {
            return Path;
        }

        return $"{Path}/NData";
    }

    /// <summary>
    /// 配置文件夹
    /// </summary>
    /// <returns></returns>
    public string GetConfigDir()
    {
        if (DataVersion == 2)
        {
            return $"{Path}/Config";
        }

        return Path;
    }
        
    public string GetAssetDir()
    {
        return $"{Path}/Assets";
    }
    
    public void Reload()
    {
        var newConfig = Load(Path, true);
        Name = newConfig.Name;
        Author = newConfig.Author;
        Version = newConfig.Version;
        Description = newConfig.Description;
        Settings = newConfig.Settings;
    }
        
    public static ModConfig Load(string dir, bool ignoreWarning = false)
    {
        ModConfig modConfig = null;
        int dataVersion;
        string filePath = Utility.CombinePaths(dir, $"modConfig.json");
        string filePathV2 = Utility.CombinePaths(dir, "Config", $"modConfig.json");
        if (File.Exists(filePath))
        {
            modConfig = JObject.Parse(File.ReadAllText(filePath)).ToObject<ModConfig>();
            dataVersion = 1;
        }
        else if (File.Exists(filePathV2))
        {
            modConfig = JObject.Parse(File.ReadAllText(filePathV2)).ToObject<ModConfig>();
            dataVersion = 2;
        }
        else
        {
            dataVersion = 1;
            if(!ignoreWarning)
                Main.LogWarning("ModManager.ModConfigDontExist".I18N() + $" dir : {dir}");
        }

        modConfig = modConfig ?? new ModConfig();
        modConfig.DataVersion = dataVersion;
        modConfig.Path = dir;
        return modConfig;
    }
    
    public static void Save(string dir,ModConfig modConfig)
    {
        string filePath = $"{dir}/modConfig.json";

        var json = JsonConvert.SerializeObject(modConfig, Formatting.Indented);
        File.WriteAllText(filePath, json);
    }
}