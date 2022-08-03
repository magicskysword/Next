using SkySwordKill.Next.FGUI.Component;
using SkySwordKill.NextEditor.Mod;
using SkySwordKill.NextEditor.PanelPage;

namespace SkySwordKill.NextEditor.PanelProject
{
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
}