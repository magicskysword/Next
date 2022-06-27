using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SkySwordKill.NextModEditor.Mod.Data
{
    public enum ModSeidPropertyType
    {
        Int,
        IntArray,
        Float,
        String
    }

    public class ModSeidProperty
    {
        public string ID { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ModSeidPropertyType Type { get; set; }
        public string Desc { get; set; } = string.Empty;
        public string[] SpecialDrawer { get; set; } = Array.Empty<string>();
    }
}