using SkySwordKill.Next.Extension;
using SkySwordKill.Next.FGUI.Component;
using SkySwordKill.NextModEditor.Mod;
using SkySwordKill.NextModEditor.PanelPage;

namespace SkySwordKill.NextModEditor.PanelProject;

public class ProjectTreeItemModBuffData : ProjectTreeEditorBaseItem
{
    public override PanelPageBase CreatePage()
    {
        var page = new PanelTableModBuffDataPage(TabName, Mod, Project)
        {
            Editable = Editable
        };
        return page;
    }

    public override string EditorName => "ModEditor.Main.project.modBuffData".I18N();

    public ProjectTreeItemModBuffData(ModWorkshop mod,ModProject project) : base(mod, project)
    {
    }
}