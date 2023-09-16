using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SkySwordKill.NextDataChecker;

public class DataSeidMetaChecker
{
    
#pragma warning disable CS8618
    public DataSeidMetaChecker(string seidNameSpace,string dataPath)
    {
        SeidNameSpace = seidNameSpace;
        DataPath = dataPath;
    }
#pragma warning restore CS8618
    
    private bool _init;
    
    public string SeidNameSpace { get; }
    public string DataPath { get; }
    public JObject DataMeta { get; set; }
    public Dictionary<int, Type> SeidTypeDic { get; set; } = new Dictionary<int, Type>();

    /// <summary>
    /// SeidMeta中缺少的Seid
    /// </summary>
    public List<int> LackSeid { get; set; } = new List<int>();
    
    /// <summary>
    /// Property与对象不一致的Seid
    /// </summary>
    public List<int> PropertyErrorSeid { get; set; } = new List<int>();

    /// <summary>
    /// IdName错误的Seid
    /// </summary>
    public List<int> IdNameErrorSeid { get; set; } = new List<int>();
    
    public int ErrorCount => LackSeid.Count + PropertyErrorSeid.Count + IdNameErrorSeid.Count;

    public void Init()
    {
        DataMeta = JObject.Parse(File.ReadAllText(DataPath));
        foreach (var type in typeof(IJSONClass).Assembly.GetTypes())
        {
            // 如果继承了IJSONClass接口，并且以SeidNameSpace开头
            if (type.IsAssignableTo(typeof(IJSONClass)) && type.Name.StartsWith(SeidNameSpace))
            {
                var seidField = type.GetField("SEIDID");
                if (seidField == null || seidField.GetValue(null) is not int seidId)
                    continue;
                if (SeidTypeDic.ContainsKey(seidId))
                {
                    Logger.Warning($"{SeidNameSpace} 重复的SEIDID:{seidId}，{SeidTypeDic[seidId].Name}和{type.Name}");
                    continue;
                }
                SeidTypeDic.Add(seidId, type);
            }
        }

        _init = true;
    }

    public void Check()
    {
        if (!_init)
        {
            Init();
        }
        
        LackSeid.Clear();

        foreach (var seidId in SeidTypeDic.Keys)
        {
            if (!DataMeta.ContainsKey(seidId.ToString()))
            {
                LackSeid.Add(seidId);
            }
        }
        foreach (var pair in DataMeta)
        {
            if (int.TryParse(pair.Key, out var seidId))
            {
                var obj = pair.Value as JObject;
                bool hasProperty = false;
                if (obj?.TryGetValue("Properties", out var properties) ?? false)
                {
                    if (properties is JArray propertiesArray)
                    {
                        hasProperty = propertiesArray.Count > 0;
                    }
                }
                
                if (!SeidTypeDic.ContainsKey(seidId) && hasProperty)
                {
                    Logger.Warning($"【{SeidNameSpace}】SeidMeta中存在没有数据的Seid:{pair.Key}");
                }
            }
            else
            {
                Logger.Error($"【{SeidNameSpace}】SeidMeta中存在非数字的Seid:{pair.Key}");
            }
        }

        
        foreach (var pair in SeidTypeDic)
        {
            var seidId = pair.Key;
            if (!DataMeta.TryGetValue(seidId.ToString(), out var metaJToken) || metaJToken is not JObject meta)
            {
                Logger.Error($"【{SeidNameSpace}】SeidMeta中缺少Seid:{seidId}");
                continue;
            }

            LackSeid.Remove(seidId);
            
            // 检查IDName
            if (!meta.TryGetValue("IDName", out var idName))
            {
                Logger.Error($"【{SeidNameSpace}】Seid:{seidId}没有定义IDName字段");
                continue;
            }
            
            // 默认第一个实例字段为Id字段
            var idField = pair.Value.GetFields()
                .Where(field => !field.IsStatic)
                .MinBy(field => field.MetadataToken);

            if (idField == null)
            {
                Logger.Error($"【{SeidNameSpace}】Seid:{seidId}没有定义实例字段");
                continue;
            }
            
            if (idField.Name != idName.ToString())
            {
                Logger.Error($"【{SeidNameSpace}】Seid:{seidId}的IDName字段不正确，当前为{idName}，应为:{idField.Name}");
                IdNameErrorSeid.Add(seidId);
            }
            
            // 检查Property
            if (meta.TryGetValue("Properties", out var properties))
            {
                if (meta.TryGetValue("Ignore", out var ignoreToken))
                {
                    if (ignoreToken.Value<bool>())
                    {
                        Logger.Info($"【{SeidNameSpace}】Seid:{seidId}的Ignore字段为true，跳过检查");
                        continue;
                    }
                }
                
                var propertyDic = new Dictionary<string, string>();
                var allFields = SeidTypeDic[seidId].GetFields()
                    .Where(field => !field.IsStatic)
                    .OrderBy(field => field.MetadataToken)
                    .ToList();
                
                if (properties is JArray propertiesArray)
                {
                    foreach (var token in propertiesArray)
                    {
                        propertyDic.Add(token["ID"]!.ToString(), token["Type"]!.ToString());
                    }
                }
                
                bool hasPropertyError = false;
                
                
                foreach (var fieldInfo in allFields.ToArray()[1..])
                {
                    if (!propertyDic.ContainsKey(fieldInfo.Name))
                    {
                        Logger.Error($"【{SeidNameSpace}】Seid:{seidId}的字段{fieldInfo.Name}没有定义");
                        hasPropertyError = true;
                    }
                    else if (GetFieldType(fieldInfo.FieldType) != propertyDic[fieldInfo.Name])
                    {
                        Logger.Warning($"【{SeidNameSpace}】Seid:{seidId}的字段{fieldInfo.Name}的Property类型不匹配，当前为{propertyDic[fieldInfo.Name]}，应为:{GetFieldType(fieldInfo.FieldType)}");
                        //hasPropertyError = true;
                        propertyDic.Remove(fieldInfo.Name);
                    }
                    else
                    {
                        propertyDic.Remove(fieldInfo.Name);
                    }
                }

                foreach (var propertyPair in propertyDic)
                {
                    Logger.Warning($"【{SeidNameSpace}】Seid:{seidId}的Property中存在没有对应字段的Property:{propertyPair.Key}，类型为{propertyPair.Value}");
                }

                if (hasPropertyError)
                {
                    PropertyErrorSeid.Add(seidId);
                }
            }
        }
    }

    public void Repair()
    {
        foreach (var seid in IdNameErrorSeid)
        {
            var idField = SeidTypeDic[seid].GetFields()
                .Where(field => !field.IsStatic)
                .MinBy(field => field.MetadataToken);
            
            var seidJObject = DataMeta[seid.ToString()]!.Value<JObject>();
            seidJObject!["IDName"] = idField!.Name;
            Logger.Info($"【{SeidNameSpace}】Seid:{seid}的IDName字段修复为:{idField.Name}");
        }

        foreach (var seid in PropertyErrorSeid)
        {
            var properties = (JArray)DataMeta[seid.ToString()]!["Properties"]!;
            var propertyDic = new Dictionary<string, JObject>();
            var allFields = SeidTypeDic[seid].GetFields()
                .Where(field => !field.IsStatic)
                .OrderBy(field => field.MetadataToken)
                .ToList();
            
            foreach (var token in properties)
            {
                propertyDic.Add(token["ID"]!.ToString(), (JObject)token);
            }
            
            foreach (var fieldInfo in allFields.ToArray()[1..])
            {
                if (!propertyDic.ContainsKey(fieldInfo.Name))
                {
                    var property = CreateProperty(fieldInfo, seid);
                    Logger.Info($"【{SeidNameSpace}】Seid:{seid}的字段{fieldInfo.Name}添加定义，类型为{property["Type"]}");
                    properties.Add(property);
                }
                else if (GetFieldType(fieldInfo.FieldType) != propertyDic[fieldInfo.Name]["Type"]!.ToString())
                {
                    // var property = propertyDic[fieldInfo.Name];
                    // var oldType = property["Type"]!.ToString();
                    // property["Type"] = GetFieldType(fieldInfo.FieldType);
                    // Logger.Info($"【{SeidNameSpace}】Seid:{seid}的字段{fieldInfo.Name}的类型从{oldType}修复为{property["Type"]}");
                }
            }
        }

        foreach (var seid in LackSeid)
        {
            var allFields = SeidTypeDic[seid].GetFields()
                .Where(field => !field.IsStatic)
                .OrderBy(field => field.MetadataToken)
                .ToList();
            
            var properties = new JArray();
            foreach (var fieldInfo in allFields.ToArray()[1..])
            {
                properties.Add(CreateProperty(fieldInfo, seid));
            }
            
            var seidJObject = new JObject();
            seidJObject["ID"] = seid;
            seidJObject["IDName"] = allFields[0].Name;
            seidJObject["Name"] = "//TODO:";
            seidJObject["Desc"] = "//TODO:";
            seidJObject["Properties"] = properties;
            seidJObject["SpecialDrawer"] = new JArray();
            Logger.Info($"【{SeidNameSpace}】Seid:{seid}已经添加到表中");
            
            DataMeta[seid.ToString()] = seidJObject;
        }
        
        var newDataMeta = new JObject();
        foreach (var property in DataMeta.Properties().OrderBy(j => int.Parse(j.Name)))
        {
            newDataMeta.Add(property.DeepClone());
        }
        
        File.WriteAllText(DataPath, newDataMeta.ToString(Formatting.Indented));
        Logger.Info($"【{SeidNameSpace}】SeidMeta已经修复，保存为：{Path.GetFullPath(DataPath)}");
    }

    public JObject CreateProperty(FieldInfo fieldInfo, int seidId)
    {
        var property = new JObject();
        property["ID"] = fieldInfo.Name;
        var propertyType = GetFieldType(fieldInfo.FieldType);
        if (string.IsNullOrEmpty(propertyType))
        {
            Logger.Warning($"【{SeidNameSpace}】Seid:{seidId}的字段{fieldInfo.Name}的类型{fieldInfo.FieldType}不支持");
            propertyType = "//TODO::Unknown";
        }
        property["Type"] = propertyType;
        property["Desc"] = "//TODO:";
        return property;
    }

    private string GetFieldType(Type propertyType)
    {
        if(propertyType == typeof(int))
            return "Int";
        else if (propertyType == typeof(float))
            return "Float";
        else if (propertyType == typeof(List<int>))
            return "IntArray";
        else if (propertyType == typeof(string))
            return "String";
        
        return string.Empty;
    }
}