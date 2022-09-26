namespace SkySwordKill.NextModEditor.Mod.Data;

public class ModSString : ModSeidToken
{
    public string Value { get; set; } = string.Empty;

    public ModSString()
    {
        PropertyType = ModSeidPropertyType.String;
    }

    public ModSString(string value) : this()
    {
        Value = value;
    }
}