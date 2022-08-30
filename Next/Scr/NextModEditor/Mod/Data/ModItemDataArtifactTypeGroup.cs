using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SkySwordKill.Next.Extension;

namespace SkySwordKill.NextModEditor.Mod.Data
{
    public class ModItemDataArtifactTypeGroup
    {
        [JsonProperty("ShapeDesc")]
        public Dictionary<string,string> ShapeDesc { get; set; }
        [JsonProperty("PropertyDesc")]
        public Dictionary<string,string> PropertyDesc { get; set; }
        
        public string GetDescription(string value)
        {
            if(string.IsNullOrEmpty(value))
                return "无";
            
            var strArr = value.Split('_');
            if (strArr.Length < 2)
                return "未知".I18NTodo();

            if (!ShapeDesc.TryGetValue(strArr[0], out var shape))
            {
                return "未知".I18NTodo();
            }
            
            if (!PropertyDesc.TryGetValue(strArr[1], out var property))
            {
                return "未知".I18NTodo();
            }

            return string.Format("{0}属性{1}".I18NTodo(), property, shape);
        }
    }
}