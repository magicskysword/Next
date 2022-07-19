using SkySwordKill.Next.FGUI.Component;
using SkySwordKill.NextEditor.PanelPage;

namespace SkySwordKill.NextEditor.PanelProject
{
    public class ProjectTreeItemModCreateAvatar : ProjectTreeItem
    {
        public override PanelPageBase CreatePage()
        {
            return new PanelTableModCreateAvatarPage()
            {
                Name = Name
            };
        }
    }
}