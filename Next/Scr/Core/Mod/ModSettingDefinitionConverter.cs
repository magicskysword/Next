using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SkySwordKill.Next.Mod;

public class ModSettingDefinitionListConverter : JsonConverter
{
    public static void Init()
    {
        foreach (var type in typeof(ModSettingDefinition).Assembly.GetTypes())
        {
            if (type.IsSubclassOf(typeof(ModSettingDefinition)) && !type.IsAbstract)
            {
                var settingType = type.GetCustomAttribute<SettingTypeAttribute>();
                if (settingType != null)
                {
                    s_typeMap.Add(type.GetCustomAttribute<SettingTypeAttribute>().TypeName, type);
                }
            }
        }
    }
    
    private static Dictionary<string, Type> s_typeMap = new Dictionary<string, Type>();

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(List<ModSettingDefinition>);
    }
    
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        serializer.Serialize(writer, value);
    }
    
    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var jArray = JArray.Load(reader);

        var list = new List<ModSettingDefinition>(jArray.Count);
        foreach (var token in jArray)
        {
            var jObject = (JObject)token;
            string type = (string)jObject["Type"];

            ModSettingDefinition item;
            if (s_typeMap.TryGetValue(type, out var typeValue))
            {
                item = (ModSettingDefinition)Activator.CreateInstance(typeValue);
            }
            else
            {
                throw new Exception("Unknown mod setting type: " + type);
            }

            serializer.Populate(jObject.CreateReader(), item);
            if (item is ModSettingDefinition_Custom customSetting)
            {
                customSetting.RawJson = jObject;
            }

            list.Add(item);
        }

        return list;
    }

    
}