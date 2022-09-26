namespace SkySwordKill.Next.FGUI.Component;

public interface IProjectItem
{
    string ID { get; }
    PanelPageBase CreatePage();
}