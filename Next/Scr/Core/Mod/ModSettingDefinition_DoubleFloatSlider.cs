using SkySwordKill.Next.FGUI.Component;
using SkySwordKill.Next.Utils;

namespace SkySwordKill.Next.Mod;

[SettingType("FloatSlider")]
public class ModSettingDefinition_DoubleFloatSlider : ModSettingDefinition
{
    public override string Type => "FloatSlider";
    public long MinValue { get; set; }
    public long MaxValue { get; set; }
    public long DefaultValue { get; set; }
    public override void OnInit()
    {
        InitDouble(Key ,DefaultValue);
        if (ModManager.TryGetModSetting(Key, out double value) && (value > MaxValue || value < MinValue))
        {
            ModManager.SetModSetting(Key, MathTools.Clamp(value, MinValue, MaxValue));
        }
    }

    public override void OnDrawer(IInspector inspector)
    {
        var drawer = new CtlDoubleSliderDrawer(Name, 
            MinValue,
            MaxValue,
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