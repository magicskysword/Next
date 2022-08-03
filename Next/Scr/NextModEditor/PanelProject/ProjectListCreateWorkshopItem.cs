using SkySwordKill.Next.FGUI.Component;
using SkySwordKill.NextEditor.Mod;

namespace SkySwordKill.NextEditor.PanelProject
{
    public abstract class ProjectListCreateWorkshopItem : ProjectListInspectItem
    {
        public abstract ModWorkshop OnCreateWorkshop();
    }
}