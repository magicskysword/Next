using SkySwordKill.Next.FGUI.Component;
using SkySwordKill.NextModEditor.Mod;
using SkySwordKill.NextModEditor.PanelPage;

namespace SkySwordKill.NextModEditor.PanelProject;

public class ProjectTreeItemBaseFungus : ProjectTreeEditorBaseItem
{
    public override PanelPageBase CreatePage()
    {
        return new PanelBaseFungusPage(TabName);
    }

    public override string EditorName => "剧情预览";

    public ProjectTreeItemBaseFungus(ModWorkshop mod,ModProject project) : base(mod, project)
    {
    }
}