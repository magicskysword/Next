using SkySwordKill.Next.FGUI.Component;
using SkySwordKill.NextModEditor.Mod;

namespace SkySwordKill.NextModEditor.PanelProject;

public abstract class ProjectListCreateWorkshopItem : ProjectListInspectItem
{
    public abstract ModWorkshop OnCreateWorkshop();
}