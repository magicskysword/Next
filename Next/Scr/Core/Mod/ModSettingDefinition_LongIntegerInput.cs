using SkySwordKill.Next.FGUI.Component;

namespace SkySwordKill.Next.Mod;

[SettingType("IntegerInput")]
public class ModSettingDefinition_LongIntegerInput : ModSettingDefinition
{
    public override string Type => "IntegerInput";
    public long DefaultValue { get; set; }
    public override void OnInit()
    {
        InitLong(Key ,DefaultValue);
    }

    public override void OnDrawer(IInspector inspector)
    {
        var drawer = new CtlLongPropertyDrawer(Name, 
            value => ModManager.SetModSetting(Key, value),
            () =>
            {
                ModManager.TryGetModSetting(Key, out long value);
                return value;
            });
        drawer.Tooltips = Description;
        inspector.AddDrawer(drawer);
    }
}