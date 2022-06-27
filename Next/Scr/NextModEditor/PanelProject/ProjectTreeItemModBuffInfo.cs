using SkySwordKill.NextEditor.PanelPage;

namespace SkySwordKill.NextEditor.PanelProject
{
    public class ProjectTreeItemModBuffInfo : ProjectTreeItem
    {
        public override PanelPageBase CreatePage()
        {
            return new PanelTableModBuffInfoPage()
            {
                Name = Name
            };
        }
    }
}