using SkySwordKill.Next.Extension;
using SkySwordKill.Next.FGUI.Component;
using SkySwordKill.NextEditor.Mod;
using SkySwordKill.NextEditor.PanelPage;

namespace SkySwordKill.NextEditor.PanelProject
{
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
}