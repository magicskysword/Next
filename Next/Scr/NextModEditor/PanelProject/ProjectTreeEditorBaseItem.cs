using SkySwordKill.Next.FGUI.Component;
using SkySwordKill.NextModEditor.Mod;

namespace SkySwordKill.NextModEditor.PanelProject;

public abstract class ProjectTreeEditorBaseItem : ProjectTreeItem, IDocumentItem
{
    public ProjectTreeEditorBaseItem(ModWorkshop mod,ModProject project)
    {
        Mod = mod;
        Project = project;
    }

    public ModWorkshop Mod;
    public ModProject Project;
        
    public string ID => $"projectItem_{Project.ProjectPath}_{GetType().FullName}";
    public abstract PanelPageBase CreatePage();

    public override string Name => EditorName;
    public abstract string EditorName { get; }
    public virtual string TabName => $"{EditorName} - {Project.ProjectName}";
    public bool Editable { get; set; } = true;
}