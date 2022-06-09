using SkySwordKill.NextEditor.PanelPage;

namespace SkySwordKill.NextEditor.PanelProject
{
    public class UIProjectItemModBuffInfo : UIProjectItem
    {
        public override PanelPageBase CreatePage()
        {
            return new PanelPageModBuffInfo()
            {
                Name = Name
            };
        }
    }
}