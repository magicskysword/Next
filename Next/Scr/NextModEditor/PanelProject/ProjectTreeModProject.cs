using System.IO;
using SkySwordKill.Next.FGUI.Component;
using SkySwordKill.NextEditor.Mod;

namespace SkySwordKill.NextEditor.PanelProject
{
    public class ProjectTreeModProject : ProjectTreeFolder
    {
        public ProjectTreeModProject(ModWorkshop mod, ModProject project)
        {
            Mod = mod;
            Project = project;
            Init();
        }
        
        public ModWorkshop Mod { get; set; }
        public ModProject Project { get; set; }

        public override string Name => Project.ProjectPathName;
        
        public void Init()
        {
            Children.Add(new ProjectTreeItemModConfig(Mod, Project));
            Children.Add(new ProjectTreeItemModCreateAvatar(Mod, Project));
            Children.Add(new ProjectTreeItemModBuffInfo(Mod, Project));
        }
    }
}