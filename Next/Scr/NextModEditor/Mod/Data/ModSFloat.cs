namespace SkySwordKill.NextModEditor.Mod.Data;

public class ModSFloat : ModSeidToken
{
    public float Value { get; set; } = 0;

    public ModSFloat()
    {
        PropertyType = ModSeidPropertyType.Float;
    }

    public ModSFloat(float value) : this()
    {
        Value = value;
    }
        
}