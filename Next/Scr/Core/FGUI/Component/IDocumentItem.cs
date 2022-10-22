namespace SkySwordKill.Next.FGUI.Component;

public interface IDocumentItem
{
    string ID { get; }
    PanelPageBase CreatePage();
}