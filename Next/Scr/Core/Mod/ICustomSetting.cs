using SkySwordKill.Next.FGUI.Component;

namespace SkySwordKill.Next.Mod;

public interface ICustomSetting
{
    void OnInit(ModSettingDefinition_Custom customSetting);
    
    void OnDrawer(ModSettingDefinition_Custom customSetting, IInspector inspector);
}