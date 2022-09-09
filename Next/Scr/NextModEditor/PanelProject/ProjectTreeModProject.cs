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

        public override string Name => Project.ProjectName;
        
        public void Init()
        {
            Children.Add(new ProjectTreeItemModConfig(Mod, Project));
            Children.Add(new ProjectTreeItemModAffixData(Mod, Project));
            Children.Add(new ProjectTreeItemModCreateAvatarData(Mod, Project));
            Children.Add(new ProjectTreeItemModItemData(Mod, Project));
            //Children.Add(new ProjectTreeItemModSkillData(Mod, Project));
            Children.Add(new ProjectTreeItemModBuffData(Mod, Project));
        }
    }
}