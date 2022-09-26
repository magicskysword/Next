using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using SkySwordKill.Next.Mod;

namespace SkySwordKill.Next;

public class NextModSetting
{
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
        nextModSetting = nextModSetting ?? new NextModSetting();
            
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

    public ModSetting GetOrCreateModSetting(ModConfig modConfig)
    {
        var modId = Path.GetFileNameWithoutExtension(modConfig.Path);
        return GetOrCreateModSetting(modId);
    }

    public ModSetting GetOrCreateModSetting(string modId)
    {
            
        if (!modSettings.TryGetValue(modId, out var modSetting))
        {
            modSetting = new ModSetting();
            modSettings.Add(modId, modSetting);
        }
        return modSetting;
    }
}