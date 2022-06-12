using SkySwordKill.NextEditor.PanelPage;

namespace SkySwordKill.NextEditor.PanelProject
{
    public class ProjectTreeItemModConfig : ProjectTreeItem
    {
        public override PanelPageBase CreatePage()
        {
            return new PanelEmptyTabModConfig()
            {
                Name = Name
            };
        }
    }
}