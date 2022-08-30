using SkySwordKill.Next.FGUI.Component;
using SkySwordKill.NextEditor.Mod;
using SkySwordKill.NextEditor.PanelPage;

namespace SkySwordKill.NextEditor.PanelProject
{
    public class ProjectTreeItemModItemData : ProjectTreeEditorBaseItem
    {
        public ProjectTreeItemModItemData(ModWorkshop mod, ModProject project) : base(mod, project)
        {
        }

        public override PanelPageBase CreatePage()
        {
            var page = new PanelTableModItemDataPage(TabName, Mod, Project)
            {
                Editable = Editable
            };
            return page;
        }

        public override string EditorName => "物品数据";
    }
}