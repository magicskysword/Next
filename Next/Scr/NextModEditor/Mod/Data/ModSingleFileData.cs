using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using SkySwordKill.Next.Mod;

namespace SkySwordKill.NextModEditor.Mod.Data;

public abstract class ModSingleFileData<T> : IModData where T : ModSingleFileData<T>
{
    public abstract int Id { get; set; }
    public static string FileName { get; set; }

    public static List<T> Load(string dir)
    {
        Dictionary<string, T> dataDic = null;
        string filePath = $"{dir}/{FileName}";
        if (File.Exists(filePath))
        {
            try
            {
                dataDic = JsonConvert.DeserializeObject<Dictionary<string, T>>(File.ReadAllText(filePath));
            }
            catch (Exception e)
            {
                throw new Exception($"加载 {typeof(T).Name} 数据失败！文件路径：{filePath}", e);
            }
        }

        if (dataDic == null)
            dataDic = new Dictionary<string, T>();

        foreach (var pair in dataDic)
        {
            if(pair.Key != pair.Value.Id.ToString())
                throw new ModException($"{typeof(T)} ID与Key ID不一致");
        }

        return dataDic.Select(dic => dic.Value).ToList();
    }

    public static void Save(string dir, List<T> dataList)
    {
        string filePath = $"{dir}/{FileName}";
        var dataDic = dataList.ToDictionary(data => data.Id.ToString(), data => data);
        if (dataDic != null && dataDic.Count > 0)
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