using System.Collections.Generic;
using System.IO;
using SkySwordKill.NextModEditor.Mod;

namespace SkySwordKill.NextModEditor.Mod.Data;

public class ModSeidDataGroupBase<TGroup> : IModSeidDataGroup where TGroup : ModSeidDataGroupBase<TGroup>, new()
{
    public Dictionary<int, ModSeidDataGroup> DataGroups { get; set; } = new Dictionary<int, ModSeidDataGroup>();
    public static string DirPath { get; set; }

    public static TGroup Create(Dictionary<int, ModSeidMeta> metas)
    {
        var data = new TGroup();

        foreach (var pair in metas)
        {
            if(pair.Value.Properties.Count == 0)
                continue;
            data.DataGroups.Add(pair.Key, ModSeidDataGroup.Create(pair.Value));
        }
        return data;
    }

    public static TGroup Load(string dir, Dictionary<int, ModSeidMeta> metas)
    {
        var data = new TGroup();
        var path = $"{dir}/{DirPath}";
        foreach (var pair in metas)
        {
            if(pair.Value.Properties.Count == 0)
                continue;
            data.DataGroups.Add(pair.Key, ModSeidDataGroup.Load(path, pair.Value));
        }

        return data;
    }

    public static void Save(string dir, TGroup dataGroup)
    {
        var path = $"{dir}/{DirPath}";
        Directory.CreateDirectory(path);
        foreach (var pair in dataGroup.DataGroups)
        {
            var meta = pair.Value.MetaData;
            if(meta.Properties.Count == 0)
                continue;
            ModSeidDataGroup.Save(path, pair.Value);
        }
    }
    
    public ModSeidData GetSeid(int dataId, int seidId)
    {
        if (DataGroups.TryGetValue(seidId, out var dataGroup))
        {
            return dataGroup.DataList.Find(data => data.Id == dataId);
        }

        return null;
    }
    
    public ModSeidData GetOrCreateSeid(int dataId, int seidId)
    {
        if (DataGroups.TryGetValue(seidId, out var dataGroup))
        {
            var data = dataGroup.DataList.Find(d => d.Id == dataId);
            if (data == null)
            {
                data = ModSeidData.CreateSeidData(dataId, dataGroup.MetaData);
                dataGroup.DataList.Add(data);
            }
            return data;
        }
        
        return null;
    }

    public bool RemoveSeid(int dataId, int seidId)
    {
        if (DataGroups.TryGetValue(seidId, out var dataGroup))
        {
            var index = dataGroup.DataList.FindIndex(data => data.Id == dataId);
            if (index >= 0)
            {
                dataGroup.DataList.RemoveAt(index);
                return true;
            }
        }
        
        return false;
    }
        
    public void RemoveAllSeid(int dataId)
    {
        foreach (var pair in DataGroups)
        {
            var dataList = pair.Value.DataList;
            var index = dataList.FindIndex(data => data.Id == dataId);
            if (index >= 0)
            {
                dataList.RemoveAt(index);
            }
        }
    }
    
    /// <summary>
    /// 将data的所有Seid迁移至新ID
    /// </summary>
    /// <param name="oldDataId"></param>
    /// <param name="newDataId"></param>
    public void ChangeSeidID(int oldDataId,int newDataId)
    {
        foreach (var seidDataGroup in DataGroups.Values)
        {
            var seidData = seidDataGroup.DataList.Find(data => data.Id == oldDataId);
            if (seidData != null)
                seidData.Id = newDataId;
        }
    }

    /// <summary>
    /// 交换两个Data的Seid的ID
    /// </summary>
    /// <param name="dataId1"></param>
    /// <param name="dataId2"></param>
    public void SwiftSeidID(int dataId1,int dataId2)
    {
        foreach (var seidDataGroup in DataGroups.Values)
        {
            var seidData1 = seidDataGroup.DataList.Find(data => data.Id == dataId1);
            var seidData2 = seidDataGroup.DataList.Find(data => data.Id == dataId2);
            if (seidData1 != null)
                seidData1.Id = dataId2;
            if (seidData2 != null)
                seidData2.Id = dataId1;
        }
    }
        
    public void CopyAllSeid(ModSeidDataGroupBase<TGroup> from, int oldId, int targetId)
    {
        foreach (var pair in from.DataGroups)
        {
            var dataList = pair.Value.DataList;
            var index = dataList.FindIndex(data => data.Id == oldId);
            if (index >= 0)
            {
                var json = ModSeidData.SaveSeidData(pair.Value.MetaData ,dataList[index]);
                var data = ModSeidData.LoadSeidData(pair.Value.MetaData, json);
                data.Id = targetId;
                DataGroups[pair.Key].DataList.Add(data);
            }
        }
    }
}