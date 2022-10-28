using System;

namespace SkySwordKill.Next.Mod;

public class SettingTypeAttribute : Attribute
{
    public string TypeName { get; set; }
    
    public SettingTypeAttribute(string typeName)
    {
        TypeName = typeName;
    }
}