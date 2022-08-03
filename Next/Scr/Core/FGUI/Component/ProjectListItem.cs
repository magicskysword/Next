namespace SkySwordKill.Next.FGUI.Component
{
    public abstract class ProjectListItem : ProjectListItemBase, IProjectItem
    {
        public abstract string ID { get; }
        public abstract PanelPageBase CreatePage();
    }
}