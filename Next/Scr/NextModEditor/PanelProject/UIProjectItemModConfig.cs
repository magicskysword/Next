using SkySwordKill.NextEditor.PanelPage;

namespace SkySwordKill.NextEditor.PanelProject
{
    public class UIProjectItemModConfig : UIProjectItem
    {
        public override PanelPageBase CreatePage()
        {
            return new PanelTabModConfig()
            {
                Name = Name
            };
        }
    }
}