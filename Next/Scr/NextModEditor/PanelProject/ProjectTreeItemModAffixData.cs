using SkySwordKill.Next.Extension;
using SkySwordKill.Next.FGUI.Component;
using SkySwordKill.NextModEditor.Mod;
using SkySwordKill.NextModEditor.PanelPage;

namespace SkySwordKill.NextModEditor.PanelProject;

public class ProjectTreeItemModAffixData : ProjectTreeEditorBaseItem
{
    public ProjectTreeItemModAffixData(ModWorkshop mod, ModProject project) : base(mod, project)
    {
    }

    public override PanelPageBase CreatePage()
    {
        var page = new PanelTableModAffixDataPage(TabName, Mod, Project)
        {
            Editable = Editable
        };
        return page;
    }

    public override string EditorName => "词缀数据".I18NTodo();
}