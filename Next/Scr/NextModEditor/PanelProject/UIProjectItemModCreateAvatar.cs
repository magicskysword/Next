using SkySwordKill.NextEditor.PanelPage;

namespace SkySwordKill.NextEditor.PanelProject
{
    public class UIProjectItemModCreateAvatar : UIProjectItem
    {
        public override PanelPageBase CreatePage()
        {
            return new PanelPageModCreateAvatar()
            {
                Name = Name
            };
        }
    }
}