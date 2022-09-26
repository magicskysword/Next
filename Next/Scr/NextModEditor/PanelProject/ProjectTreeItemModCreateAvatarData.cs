using SkySwordKill.Next.Extension;
using SkySwordKill.Next.FGUI.Component;
using SkySwordKill.NextModEditor.Mod;
using SkySwordKill.NextModEditor.PanelPage;

namespace SkySwordKill.NextModEditor.PanelProject;

public class ProjectTreeItemModCreateAvatarData : ProjectTreeEditorBaseItem
{
    public override PanelPageBase CreatePage()
    {
        var page = new PanelTableModCreateAvatarDataPage(TabName, Mod, Project)
        {
            Editable = Editable
        };
        return page;
    }

    public override string EditorName => "ModEditor.Main.project.modCreateAvatarData".I18N();

    public ProjectTreeItemModCreateAvatarData(ModWorkshop mod,ModProject project) : base(mod, project)
    {
    }
}