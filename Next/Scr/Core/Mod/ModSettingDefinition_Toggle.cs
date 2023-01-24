using SkySwordKill.Next.FGUI.Component;

namespace SkySwordKill.Next.Mod;

[SettingType("Toggle")]
public class ModSettingDefinition_Toggle : ModSettingDefinition
{
    public override string Type => "Toggle";
    public bool DefaultValue { get; set; }
    
    public override void OnInit()
    {
        InitBool(Key, DefaultValue);
    }

    public override void OnDrawer(IInspector inspector)
    {
       var drawer = new CtlCheckboxPropertyDrawer(Name, 
           value => ModManager.SetModSetting(Key, value),
           () =>
           {
               ModManager.TryGetModSetting(Key, out bool? value);
               return value ?? false;
           });
       drawer.Tooltips = Description;
       inspector.AddDrawer(drawer);
    }
}