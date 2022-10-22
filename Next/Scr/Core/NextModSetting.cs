using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using SkySwordKill.Next.Mod;

namespace SkySwordKill.Next;

public class NextModSetting
{
    public Dictionary<string, ModGroupSetting> groupSettings = new Dictionary<string, ModGroupSetting>();
    public Dictionary<string, ModSetting> modSettings = new Dictionary<string, ModSetting>();

    public static NextModSetting LoadSetting()
    {
        NextModSetting nextModSetting = null;
        var filePath = Main.PathModSettingFile.Value;
        if (File.Exists(filePath))
        {
            try
            {
                var json = File.ReadAllText(filePath);
                nextModSetting = JsonConvert.DeserializeObject<NextModSetting>(json);
            }
            catch (Exception)
            {
                // ignored
            }
        }
        nextModSetting ??= new NextModSetting();
            
        return nextModSetting;
    }

    public static void SaveSetting(NextModSetting modSetting)
    {
        var filePath = Main.PathModSettingFile.Value;
        try
        {
            var json = JsonConvert.SerializeObject(modSetting, Formatting.Indented);
            File.WriteAllText(filePath,json);
        }
        catch (Exception e)
        {
            Main.LogError(e);
        }
    }

    public ModGroupSetting GetOrCreateModGroupSetting(ModGroup modGroup)
    {
        if (!groupSettings.TryGetValue(modGroup.GroupKey, out var modSetting))
        {
            modSetting = new ModGroupSetting();
            groupSettings.Add(modGroup.GroupKey, modSetting);
        }
        modSetting.BindGroup = modGroup;
        return modSetting;
    }
    
    public ModSetting GetOrCreateModSetting(ModConfig config)
    {
        var key = config.SettingKey;
        if (!modSettings.TryGetValue(key, out var modSetting))
        {
            modSetting = new ModSetting();
            modSettings.Add(key, modSetting);
        }
        modSetting.BindMod = config;
        return modSetting;
    }

    public void ClearPriority()
    {
        var priority = 0;
        foreach (var pair in groupSettings)
        {
            pair.Value.priority = priority++;
        }
    }
}