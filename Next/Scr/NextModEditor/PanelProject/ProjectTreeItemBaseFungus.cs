using SkySwordKill.NextEditor.PanelPage;

namespace SkySwordKill.NextEditor.PanelProject
{
    public class ProjectTreeItemBaseFungus : ProjectTreeItem
    {
        public override PanelPageBase CreatePage()
        {
            return new PanelBaseFungusPage()
            {
                Name = Name
            };
        }
    }
}