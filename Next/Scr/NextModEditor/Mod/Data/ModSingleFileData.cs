using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using SkySwordKill.NextEditor.Mod;

namespace SkySwordKill.NextModEditor.Mod.Data
{
    public abstract class ModSingleFileData<T> : IModData where T : ModSingleFileData<T>
    {
        public abstract int Id { get; set; }
        public static string FileName { get; set; }

        public static Dictionary<string, T> Load(string dir)
        {
            Dictionary<string, T> dataDic = null;
            string filePath = $"{dir}/{FileName}";
            if (File.Exists(filePath))
            {
                dataDic = JsonConvert.DeserializeObject<Dictionary<string, T>>(File.ReadAllText(filePath));
            }

            if (dataDic == null)
                dataDic = new Dictionary<string, T>();

            foreach (var pair in dataDic)
            {
                if(pair.Key != pair.Value.Id.ToString())
                    throw new ModException($"{typeof(T)} ID与Key ID不一致");
            }

            return dataDic;
        }

        public static void Save(string dir, Dictionary<string, T> dataDic)
        {
            string filePath = $"{dir}/{FileName}";
            if (dataDic != null)
            {
                var json = JsonConvert.SerializeObject(dataDic, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
            else
            {
                File.Delete(filePath);
            }
        }
    }
}