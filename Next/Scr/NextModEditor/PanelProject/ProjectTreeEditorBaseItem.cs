using SkySwordKill.Next.FGUI.Component;
using SkySwordKill.NextEditor.Mod;

namespace SkySwordKill.NextEditor.PanelProject
{
    public abstract class ProjectTreeEditorBaseItem : ProjectTreeItem
    {
        public ProjectTreeEditorBaseItem(ModWorkshop mod,ModProject project)
        {
            Mod = mod;
            Project = project;
        }

        public ModWorkshop Mod;
        public ModProject Project;
        
        public override string ID => $"projectItem_{Project.ProjectPath}_{GetType().FullName}";
        public override string Name => EditorName;
        public abstract string EditorName { get; }
        public virtual string TabName => $"{EditorName} - {Project.ProjectName}";
        public bool Editable { get; set; } = true;
    }
}