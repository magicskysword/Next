using System;
using SkySwordKill.Next.FGUI.Component;
using SkySwordKill.Next.Utils;

namespace SkySwordKill.Next.Mod;

[SettingType("Dropdown")]
public class ModSettingDefinition_Dropdown : ModSettingDefinition
{
    public override string Type => "Dropdown";
    public string[] Options { get; set; } = Array.Empty<string>();
    public long DefaultValue { get; set; }
    
    public override void OnInit()
    {
        InitLong(Key, DefaultValue);
        if (ModManager.TryGetModSetting(Key, out long value) && (value < 0 || value >= Options.Length))
        {
            ModManager.SetModSetting(Key, DefaultValue);
        }
    }

    public override void OnDrawer(IInspector inspector)
    {
        var drawer = new CtlDropdownPropertyDrawer(Name, 
            () => Options,
            index => ModManager.SetModSetting(Key, index),
            () =>
            {
                ModManager.TryGetModSetting(Key, out long value);
                return (int)value;
            });
        drawer.Tooltips = Description;
        inspector.AddDrawer(drawer);
    }
}