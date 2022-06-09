using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SkySwordKill.NextEditor.Mod;

namespace SkySwordKill.NextModEditor.Mod.Data
{
    public abstract class ModSingleFileData<T> : IModData where T : ModSingleFileData<T>
    {
        public abstract int ID { get; set; }
        public static string FileName { get; set; }

        public static Dictionary<string, T> Load(string dir)
        {
            Dictionary<string, T> dataDic = null;
            string filePath = $"{dir}/{FileName}";
            if (File.Exists(filePath))
            {
                dataDic = JObject.Parse(File.ReadAllText(filePath)).ToObject<Dictionary<string, T>>();
            }

            if (dataDic == null)
                dataDic = new Dictionary<string, T>();

            foreach (var pair in dataDic)
            {
                if(pair.Key != pair.Value.ID.ToString())
                    throw new ModException($"{typeof(T)} ID与Key ID不一致");
            }

            return dataDic;
        }

        public static void Save(string dir, Dictionary<string, T> dataDic)
        {
            string filePath = $"{dir}/{FileName}";
            if (dataDic != null)
            {
                var json = JObject.FromObject(dataDic).ToString(Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
            else
            {
                File.Delete(filePath);
            }
        }
    }
}