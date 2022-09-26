using SkySwordKill.Next.Extension;
using SkySwordKill.Next.FGUI.Component;
using SkySwordKill.NextModEditor.Mod;
using SkySwordKill.NextModEditor.PanelPage;

namespace SkySwordKill.NextModEditor.PanelProject;

public class ProjectTreeItemModConfig : ProjectTreeEditorBaseItem
{
    public override PanelPageBase CreatePage()
    {
        return new PanelModConfig(TabName)
        {
            Project = Project,
            Mod = Mod,
        };
    }

    public override string EditorName => "ModEditor.Main.project.modConfig".I18N();

    public ProjectTreeItemModConfig(ModWorkshop mod,ModProject project) : base(mod, project)
    {
    }
}