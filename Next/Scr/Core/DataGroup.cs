using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SkySwordKill.Next;

public class DataGroup<T>
{
    private const string DEFAULT_GROUP = "Default";
        
    [JsonProperty("DataDic")]
    public Dictionary<string, Dictionary<string, T>> DataDic { get; set; } =
        new Dictionary<string, Dictionary<string, T>>();
        
    public void Set(string key, T value)
    {
        Set(DEFAULT_GROUP, key, value);
    }

    public void Set(string group, string key, T value)
    {
        var targetGroup = GetGroup(group);

        if (value == null || value.Equals(default))
        {
            if (targetGroup.ContainsKey(key))
                targetGroup.Remove(key);
        }
        else
        {
            targetGroup[key] = value;
        }
    }

    public T Get(string key)
    {
        return Get(DEFAULT_GROUP, key);
    }

    public T Get(string group, string key)
    {
        var targetGroup = GetGroup(group);
        if (targetGroup.ContainsKey(key))
            return targetGroup[key];
        return default;
    }

    public void AddRange(Dictionary<string, T> dic)
    {
        var defaultGroup = GetDefaultGroup();
        foreach (var pair in dic)
        {
            defaultGroup[pair.Key] = pair.Value;
        }
    }

    public bool HasGroup(string group)
    {
        return DataDic.ContainsKey(group);
    }

    public Dictionary<string, T> GetDefaultGroup()
    {
        return GetGroup(DEFAULT_GROUP);
    }

    public Dictionary<string, T> GetGroup(string group)
    {
        if (!DataDic.TryGetValue(group,out var targetGroup))
        {
            targetGroup = new Dictionary<string, T>();
            DataDic[group] = targetGroup;
        }

        return targetGroup;
    }

    public bool Has(string key)
    {
        return GetDefaultGroup().ContainsKey(key);
    }
    
    public bool Has(string group, string key)
    {
        return HasGroup(group) && GetGroup(group).ContainsKey(key);
    }
}