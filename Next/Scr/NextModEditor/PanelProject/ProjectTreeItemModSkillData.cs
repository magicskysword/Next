using SkySwordKill.Next.Extension;
using SkySwordKill.Next.FGUI.Component;
using SkySwordKill.NextEditor.Mod;
using SkySwordKill.NextEditor.PanelPage;

namespace SkySwordKill.NextEditor.PanelProject
{
    public class ProjectTreeItemModSkillData : ProjectTreeEditorBaseItem
    {
        public ProjectTreeItemModSkillData(ModWorkshop mod, ModProject project) : base(mod, project)
        {
        }

        public override PanelPageBase CreatePage()
        {
            var page = new PanelTableModSkillDataPage(TabName, Mod, Project)
            {
                Editable = Editable
            };
            return page;
        }

        public override string EditorName => "神通数据".I18NTodo();
    }
}