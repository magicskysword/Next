using SkySwordKill.Next.FGUI.Component;
using SkySwordKill.NextEditor.Mod;
using SkySwordKill.NextEditor.PanelPage;

namespace SkySwordKill.NextEditor.PanelProject
{
    public class ProjectTreeModWorkshop : ProjectTreeItem
    {
        public ProjectTreeModWorkshop(ModWorkshop mod)
        {
            Mod = mod;
        }
        
        public ModWorkshop Mod { get; set; }
        
        public override string Name => "工坊项目设置";
        public override string ID => "workshop";
        public override PanelPageBase CreatePage()
        {
            return new PanelModWorkshop(Name, Mod);
        }
    }
}