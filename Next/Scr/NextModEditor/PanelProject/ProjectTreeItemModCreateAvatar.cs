using SkySwordKill.Next.Extension;
using SkySwordKill.Next.FGUI.Component;
using SkySwordKill.NextEditor.Mod;
using SkySwordKill.NextEditor.PanelPage;

namespace SkySwordKill.NextEditor.PanelProject
{
    public class ProjectTreeItemModCreateAvatar : ProjectTreeEditorBaseItem
    {
        public override PanelPageBase CreatePage()
        {
            return new PanelTableModCreateAvatarPage(TabName, Mod, Project);
        }

        public override string EditorName => "ModEditor.Main.project.modCreateAvatar".I18N();

        public ProjectTreeItemModCreateAvatar(ModWorkshop mod,ModProject project) : base(mod, project)
        {
        }
    }
}