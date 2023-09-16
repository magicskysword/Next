using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SkySwordKill.Next.Extension;
using SkySwordKill.Next.Mod;

namespace SkySwordKill.NextModEditor.Mod.Data;

public class ModSeidDataGroup
{
    public ModSeidMeta MetaData { get; set; }
    public List<ModSeidData> DataList { get; set; } = new List<ModSeidData>();

    private ModSeidDataGroup(ModSeidMeta meta)
    {
        MetaData = meta;
    }
    
    public static ModSeidDataGroup Create(ModSeidMeta meta)
    {
        ModSeidDataGroup data = new ModSeidDataGroup(meta);
        return data;
    }
        
    public static ModSeidDataGroup Load(string dir,ModSeidMeta meta)
    {
        ModSeidDataGroup data = null;
        string filePath = $"{dir}/{meta.Id}.json";
        if (File.Exists(filePath))
        {
            data = new ModSeidDataGroup(meta);
            var jsonData = JObject.Parse(File.ReadAllText(filePath));
            foreach (var property in jsonData.Properties())
            {
                try
                {
                    var seidData = ModSeidData.LoadSeidData(meta, (JObject)property.Value);
                    if (property.Name != seidData.Id.ToString())
                        throw new ModException("Seid ID与Key ID不一致");
                    data.DataList.Add(seidData);
                }
                catch (ModDataIdNotExistException)
                {
                    throw new ModException(string.Format("{0}中的{1}的ID不存在".I18NTodo(), meta.Id, property.Name));
                }
                catch (Exception e)
                {
                    throw new JsonException($"Seid Json {filePath} {property.Name} 读取失败", e);
                }
            }
        }

        if(data == null)
            data = new ModSeidDataGroup(meta);

        return data;
    }

    public static void Save(string dir, ModSeidDataGroup dataGroup)
    {
        string filePath = $"{dir}/{dataGroup.MetaData.Id}.json";
        if(dataGroup.DataList.Count == 0)
        {
            File.Delete(filePath);
            return;
        }
            
        var jObject = new JObject();
        foreach (var seidData in dataGroup.DataList)
        {
            var jsonData = ModSeidData.SaveSeidData(dataGroup.MetaData, seidData);
            jObject.Add(seidData.Id.ToString(),jsonData);
        }

        File.WriteAllText(filePath, jObject.ToString(Formatting.Indented));
    }
}