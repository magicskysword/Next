using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
                modConfig = JObject.Parse(File.ReadAllText(filePath)).ToObject<ModConfig>();
            }

            modConfig = modConfig ?? new ModConfig();

            return modConfig;
        }
    
        public static void Save(string dir,ModConfig modConfig)
        {
            string filePath = $"{dir}/modConfig.json";

            var json = JObject.FromObject(modConfig).ToString(Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
    }
}