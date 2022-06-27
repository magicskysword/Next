using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using SkySwordKill.NextEditor.Mod;

namespace SkySwordKill.NextModEditor.Mod.Data
{
    public abstract class ModFolderFileData<T> : IModData where T : ModFolderFileData<T>
    {
        public abstract int ID { get; set; }
        public static string FolderName { get; set; }

        public static List<T> Load(string dir)
        {
            List<T> dataList = new List<T>();
            var buffDir = $"{dir}/{FolderName}";
            if (!Directory.Exists(buffDir))
                return dataList;
            foreach (var filePath in Directory.GetFiles(buffDir))
            {
                var data = JsonConvert.DeserializeObject<T>(File.ReadAllText(filePath));

                if (Path.GetFileNameWithoutExtension(filePath) != data?.ID.ToString())
                    throw new ModException("文件ID与定义ID不一致");
            
                dataList.Add(data);
            }

            return dataList;
        }

        public static void Save(string dir, List<T> dataDic)
        {
            var buffDir = $"{dir}/{FolderName}";
            if (Directory.Exists(buffDir))
                Directory.Delete(buffDir, true);
            Directory.CreateDirectory(buffDir);
            foreach (var data in dataDic)
            {
                var filePath = $"{buffDir}/{data.ID}.json";
                var json = JsonConvert.SerializeObject(data, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
        }
    }
}