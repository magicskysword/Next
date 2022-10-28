using SkySwordKill.Next.FGUI.Component;
using SkySwordKill.Next.Utils;

namespace SkySwordKill.Next.Mod;

[SettingType("IntegerSlider")]
public class ModSettingDefinition_LongIntegerSlider : ModSettingDefinition
{
    public override string Type => "IntegerSlider";
    public long MinValue { get; set; }
    public long MaxValue { get; set; }
    public long DefaultValue { get; set; }
    public override void OnInit()
    {
        InitLong(Key ,DefaultValue);
        if (ModManager.TryGetModSetting(Key, out long value) && (value > MaxValue || value < MinValue))
        {
            ModManager.SetModSetting(Key, MathTools.Clamp(value, MinValue, MaxValue));
        }
    }

    public override void OnDrawer(IInspector inspector)
    {
        var drawer = new CtlLongSliderDrawer(Name, 
            MinValue,
            MaxValue,
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