using System.IO;
using SkySwordKill.Next.Extension;
using SkySwordKill.Next.FGUI.Component;
using SkySwordKill.Next.Mod;

namespace SkySwordKill.Next.ModGUI;

public class ProjectTreeItemModConfig : ProjectTreeItem
{
    public ProjectTreeItemModConfig(ModConfig config, ModGroup group)
    {
        ModConfig = config;
        ModGroup = group;
    }

    public ModConfig ModConfig { get; }
    public ModGroup ModGroup { get; }
    public override string ResURL => "ui://NextCore/BtnTreeItemMod";

    public override string Name =>
        $"{ModConfig.Name} - {ModConfig.Version} ({Path.GetFileNameWithoutExtension(ModConfig.Path)})";


    public void OnInspector(CtlInspectorBase inspector)
    {
        var modPath = ModConfig.Path ?? "Mod.Unknown".I18N();
        var modName = ModConfig.Name ?? "Mod.Unknown".I18N();
        var modAuthor = ModConfig.Author ?? "Mod.Unknown".I18N();
        var modVersion = ModConfig.Version ?? "Mod.Unknown".I18N();
        var modDesc = ModConfig.Description ?? "Mod.Unknown".I18N();
        
        inspector.AddDrawer(new CtlTitleDrawer(modName));

        var groupInfo = new CtlGroupDrawer("Mod信息".I18NTodo(), true,
            new CtlInfoDrawer("Mod.State".I18N(), ModConfig.GetModStateDescription(), 16),
            new CtlInfoDrawer("Mod.Author".I18N(), modAuthor, 16),
            new CtlInfoDrawer("Mod.Version".I18N(), modVersion, 16),
            new CtlInfoDrawer("Mod.Description".I18N(), modDesc, 16),
            new CtlInfoLinkDrawer("Mod.Directory".I18N(), modPath, 16, modPath)
        );

        var groupSetting = new CtlGroupDrawer("Mod设置".I18NTodo(), true);
        foreach (var settingDefinition in ModConfig.Settings)
        {
            if (settingDefinition is ModSettingDefinition_Custom customDefinition)
            {
                var customSetting = ModManager.GetCustomSetting(customDefinition.CustomType);
                customSetting.OnDrawer(customDefinition, groupSetting);
            }
            else
            {
                settingDefinition.OnDrawer(groupSetting);
            }
        }
        
        inspector.AddDrawer(groupInfo);
        if(groupSetting.Drawers.Count > 0)
            inspector.AddDrawer(groupSetting);
    }
}