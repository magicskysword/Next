using System.IO;
using Newtonsoft.Json;

namespace SkySwordKill.NextEditor.Mod
{
    public class ModConfig : Next.Mod.ModConfig
    {
        public static ModConfig Load(string dir)
        {
            ModConfig modConfig = null;
            string filePath = $"{dir}/modConfig.json";
            if (File.Exists(filePath))
            {
                modConfig = JsonConvert.DeserializeObject<ModConfig>(File.ReadAllText(filePath));
            }

            modConfig = modConfig ?? new ModConfig();

            return modConfig;
        }
    
        public static void Save(string dir,ModConfig modConfig)
        {
            string filePath = $"{dir}/modConfig.json";

            var json = JsonConvert.SerializeObject(modConfig, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
    }
}