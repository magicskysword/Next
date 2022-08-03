using SkySwordKill.Next.Extension;
using SkySwordKill.Next.FGUI.Component;
using SkySwordKill.NextEditor.Mod;
using SkySwordKill.NextEditor.PanelPage;

namespace SkySwordKill.NextEditor.PanelProject
{
    public class ProjectTreeItemModBuffInfo : ProjectTreeEditorBaseItem
    {
        public override PanelPageBase CreatePage()
        {
            return new PanelTableModBuffInfoPage(TabName, Mod, Project);
        }

        public override string EditorName => "ModEditor.Main.project.modBuffInfo".I18N();

        public ProjectTreeItemModBuffInfo(ModWorkshop mod,ModProject project) : base(mod, project)
        {
        }
    }
}