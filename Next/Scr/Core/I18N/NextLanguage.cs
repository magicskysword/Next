using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace SkySwordKill.Next.I18N;

public class NextLanguage
{
    public static List<NextLanguage> Languages { get; set; } = new List<NextLanguage>();

    private Dictionary<string, string> _translation = new Dictionary<string, string>();
    
    public string LanguageDir { get; set; }
    public NextLanguageConfig Config { get; set; }

    public Dictionary<string, string> Translation => _translation;

    public static void InitLanguage()
    {
        Languages.Clear();
            
        Main.LogInfo($"Load language folder : {Main.PathLanguageDir.Value}");
        var languageDir = Directory.GetDirectories(Main.PathLanguageDir.Value);
        foreach (var dir in languageDir)
        {
            try
            {
                var fileName = Path.Combine(dir, "language.json");
                if (!File.Exists(fileName))
                {
                    Main.LogWarning($"language config [{fileName}] not find in {dir}!");
                    continue;
                }
                var json = File.ReadAllText(fileName);
                var config = JsonConvert.DeserializeObject<NextLanguageConfig>(json);
                if (config == null)
                    throw new JsonException("json data is invalid.");
                var language = new NextLanguage
                {
                    LanguageDir = dir,
                    Config = config
                };
                language.LoadLocalization();
                Languages.Add(language);
            }
            catch (Exception e)
            {
                Main.LogError($"Load language file {dir} error!");
                Main.LogError(e);
            }
        }
    }

    private void LoadLocalization()
    {
        var localizationDir = Path.Combine(LanguageDir, "Localization");
        if (!Directory.Exists(localizationDir))
            return;
        foreach (var file in Directory.GetFiles(localizationDir))
        {
            try
            {
                var json = File.ReadAllText(file);
                var localization = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                if (localization == null)
                    throw new JsonException("json data is invalid.");
                foreach (var pair in localization)
                {
                    if(_translation.ContainsKey(pair.Key))
                        Main.LogWarning($"Localization key {pair.Key} is already exist.");
                    _translation[pair.Key] = pair.Value;
                }
            }
            catch (Exception e)
            {
                Main.LogError($"Load localization file {file} error!");
                Main.LogError(e);
            }
        }
    }

    public static string Get(NextLanguage language, string key)
    {
        if (language == null)
        {
            return key;
        }

        if (language.Translation.TryGetValue(key, out var value))
        {
            return value;
        }

        return key;
    }

    public static bool TryGetLanguageByDir(string languageDir, out NextLanguage language)
    {
        foreach (var nextLanguage in Languages)
        {
            if (nextLanguage.LanguageDir == languageDir)
            {
                language = nextLanguage;
                return true;
            }
        }

        language = null;
        return false;
    }
}