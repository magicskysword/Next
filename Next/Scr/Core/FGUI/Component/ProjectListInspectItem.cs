namespace SkySwordKill.Next.FGUI.Component;

public abstract class ProjectListInspectItem : ProjectListItemBase
{
    public CtlPropertyInspector Inspector { get; set; }
        
    public abstract void OnInspect();
}