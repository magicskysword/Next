using SkySwordKill.Next.FGUI.Component;
using SkySwordKill.NextModEditor.Mod;
using SkySwordKill.NextModEditor.PanelPage;

namespace SkySwordKill.NextModEditor.PanelProject;

public class ProjectTreeItemModItemData : ProjectTreeEditorBaseItem
{
    public ProjectTreeItemModItemData(ModWorkshop mod, ModProject project) : base(mod, project)
    {
    }

    public override PanelPageBase CreatePage()
    {
        var page = new PanelTableModItemDataPage(TabName, Mod, Project)
        {
            Editable = Editable
        };
        return page;
    }

    public override string EditorName => "物品数据";
}