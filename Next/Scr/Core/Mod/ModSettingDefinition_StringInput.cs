using SkySwordKill.Next.FGUI.Component;

namespace SkySwordKill.Next.Mod;

[SettingType("StringInput")]
public class ModSettingDefinition_StringInput : ModSettingDefinition
{
    public override string Type => "StringInput";
    public string DefaultValue { get; set; }
    public override void OnInit()
    {
        InitString(Key, DefaultValue);
    }

    public override void OnDrawer(IInspector inspector)
    {
        var drawer = new CtlStringPropertyDrawer(Name, 
            value => ModManager.SetModSetting(Key, value),
            () =>
            {
                ModManager.TryGetModSetting(Key, out string value);
                return value;
            });
        drawer.Tooltips = Description;
        inspector.AddDrawer(drawer);
    }
}