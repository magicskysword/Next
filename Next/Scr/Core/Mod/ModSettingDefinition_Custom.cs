using Newtonsoft.Json.Linq;
using SkySwordKill.Next.FGUI.Component;

namespace SkySwordKill.Next.Mod;

[SettingType("Custom")]
public class ModSettingDefinition_Custom : ModSettingDefinition
{
    public override string Type => "Custom";
    public string CustomType { get; set; }
    public JObject RawJson { get; set; }
    
    public override void OnInit()
    {
        
    }

    public override void OnDrawer(IInspector inspector)
    {
        
    }
    
    public T GetConfig<T>(string key)
    {
        var value = RawJson[key];
        return value == null ? default : value.ToObject<T>();
    }
}