using SkySwordKill.Next.FGUI.Component;

namespace SkySwordKill.Next.Mod;

[SettingType("FloatInput")]
public class ModSettingDefinition_DoubleFloatInput : ModSettingDefinition
{
    public override string Type => "FloatInput";
    public long DefaultValue { get; set; }
    public override void OnInit()
    {
        InitDouble(Key ,DefaultValue);
    }

    public override void OnDrawer(IInspector inspector)
    {
        var drawer = new CtlDoublePropertyDrawer(Name, 
            value => ModManager.SetModSetting(Key, value),
            () =>
            {
                ModManager.TryGetModSetting(Key, out double value);
                return value;
            });
        drawer.Tooltips = Description;
        inspector.AddDrawer(drawer);
    }
}