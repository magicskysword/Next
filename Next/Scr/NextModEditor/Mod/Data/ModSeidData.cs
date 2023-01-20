using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using SkySwordKill.Next;
using SkySwordKill.Next.Mod;

namespace SkySwordKill.NextModEditor.Mod.Data;

public class ModSeidData : IModData
{
    private static readonly string[] _FallbackSeidIds = new string[]
    {
        "id",
        "skillid"
    };
    
    public int Id { get; set; }
    public Dictionary<string, ModSeidToken> DataDic { get; set; } = new Dictionary<string, ModSeidToken>();

    public static ModSeidData LoadSeidData(ModSeidMeta meta, JObject jObject)
    {
        var data = new ModSeidData();
        if (!jObject.TryGetValue(meta.IDName, out JToken idToken))
        {
            foreach (var fallbackSeidId in _FallbackSeidIds)
            {
                if (jObject.TryGetValue(fallbackSeidId, out idToken))
                {
                    break;
                }
            }
            
            if (idToken == null)
            {
                throw new ModDataIdNotExistException();
            }
        }
        
        
        data.Id = idToken.ToObject<int>();
        foreach (var seidProperty in meta.Properties)
        {
            JToken token = null;
            switch (seidProperty.Type)
            {
                case ModSeidPropertyType.Int:
                    var sInt = new ModSInt();
                    if (jObject.TryGetValue(seidProperty.ID, out token))
                        sInt.Value = token.ToObject<int>();
                    data.DataDic.Add(seidProperty.ID,sInt);
                    break;
                case ModSeidPropertyType.IntArray:
                    var sIntArray = new ModSIntArray();
                    if (jObject.TryGetValue(seidProperty.ID, out token))
                    {
                        sIntArray.Value.AddRange(token.ModLoadIntArray());
                    }
                    data.DataDic.Add(seidProperty.ID,sIntArray);
                    break;
                case ModSeidPropertyType.Float:
                    var sFloat = new ModSFloat();
                    if (jObject.TryGetValue(seidProperty.ID, out token))
                        sFloat.Value = token.ToObject<float>();
                    data.DataDic.Add(seidProperty.ID,sFloat);
                    break;
                case ModSeidPropertyType.String:
                    var sString = new ModSString();
                    if (jObject.TryGetValue(seidProperty.ID, out token))
                        sString.Value = token.ToObject<string>();
                    data.DataDic.Add(seidProperty.ID,sString);
                    break;
            }
        }

        return data;
    }

    public T GetToken<T>(string dataId) where T : ModSeidToken
    {
        return DataDic[dataId] as T;
    }

    public static ModSeidData CreateSeidData(int ID,ModSeidMeta meta)
    {
        var data = new ModSeidData();
        data.Id = ID;
        foreach (var seidProperty in meta.Properties)
        {
            switch (seidProperty.Type)
            {
                case ModSeidPropertyType.Int:
                    var sInt = new ModSInt();
                    data.DataDic.Add(seidProperty.ID,sInt);
                    break;
                case ModSeidPropertyType.IntArray:
                    var sIntArray = new ModSIntArray();
                    data.DataDic.Add(seidProperty.ID,sIntArray);
                    break;
                case ModSeidPropertyType.Float:
                    var sFloat = new ModSFloat();
                    data.DataDic.Add(seidProperty.ID,sFloat);
                    break;
                case ModSeidPropertyType.String:
                    var sString = new ModSString();
                    data.DataDic.Add(seidProperty.ID,sString);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        return data;
    }

    public static JObject SaveSeidData(ModSeidMeta meta, ModSeidData seidData)
    {
        var data = new JObject();
        data.Add(meta.IDName,seidData.Id);
        foreach (var seidProperty in meta.Properties)
        {
            switch (seidProperty.Type)
            {
                case ModSeidPropertyType.Int:
                    data.Add(seidProperty.ID, seidData.GetToken<ModSInt>(seidProperty.ID).Value);
                    break;
                case ModSeidPropertyType.IntArray:
                    var jArray = JArray.FromObject(seidData.GetToken<ModSIntArray>(seidProperty.ID).Value);
                    data.Add(seidProperty.ID, jArray);
                    break;
                case ModSeidPropertyType.Float:
                    data.Add(seidProperty.ID, seidData.GetToken<ModSFloat>(seidProperty.ID).Value);
                    break;
                case ModSeidPropertyType.String:
                    data.Add(seidProperty.ID, seidData.GetToken<ModSString>(seidProperty.ID).Value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return data;
    }

    public void SetValue(string key, int data)
    {
        if (DataDic.TryGetValue(key, out var token) && token is ModSInt sInt)
        {
            sInt.Value = data;
        }
        else
        {
            Main.LogError($"无法将Seid：{Id}的{key}设置为{data}，因为该属性不存在或者类型不匹配");
        }
    }
}