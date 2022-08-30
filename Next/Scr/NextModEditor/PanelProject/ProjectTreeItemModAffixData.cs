using SkySwordKill.Next.Extension;
using SkySwordKill.Next.FGUI.Component;
using SkySwordKill.NextEditor.Mod;
using SkySwordKill.NextEditor.PanelPage;

namespace SkySwordKill.NextEditor.PanelProject
{
    public class ProjectTreeItemModAffixData : ProjectTreeEditorBaseItem
    {
        public ProjectTreeItemModAffixData(ModWorkshop mod, ModProject project) : base(mod, project)
        {
        }

        public override PanelPageBase CreatePage()
        {
            var page = new PanelTableModAffixDataPage(TabName, Mod, Project)
            {
                Editable = Editable
            };
            return page;
        }

        public override string EditorName => "词缀数据".I18NTodo();
    }
}