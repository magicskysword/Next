using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace SkySwordKill.Next
{
    public class NextLanguage
    {
        [JsonIgnore]
        public static Dictionary<string,NextLanguage> languages = new Dictionary<string,NextLanguage>();

        public static void InitLanguage()
        {
            languages.Clear();
            
            Main.LogInfo($"Load language folder : {Main.PathLanguageDir.Value}");
            var languageFiles = Directory.GetFiles(Main.PathLanguageDir.Value);
            foreach (var file in languageFiles)
            {
                try
                {
                    var json = File.ReadAllText(file);
                    var language = JsonConvert.DeserializeObject<NextLanguage>(json);
                    if (language == null)
                        throw new JsonException("json data is empty.");
                    language.FileName = Path.GetFileNameWithoutExtension(file);
                    languages.Add(language.FileName,language);
                    Main.LogInfo($"Load language {language.LanguageName} success.");
                }
                catch (Exception e)
                {
                    Main.LogError($"Load language file {file} error!");
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
        
        [JsonIgnore]
        public string FileName { get; set; }
        public string LanguageName { get; set; }
        public string ConfigDir { get; set; }
        public Dictionary<string, string> Translation { get; set; } = new Dictionary<string, string>();
    }
}