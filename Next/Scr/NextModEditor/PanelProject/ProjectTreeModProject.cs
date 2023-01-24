using SkySwordKill.Next.FGUI.Component;
using SkySwordKill.NextModEditor.Mod;

namespace SkySwordKill.NextModEditor.PanelProject;

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
        AddChild(new ProjectTreeItemModConfig(Mod, Project));
        AddChild(new ProjectTreeItemModAffixData(Mod, Project));
        AddChild(new ProjectTreeItemModCreateAvatarData(Mod, Project));
        AddChild(new ProjectTreeItemModItemData(Mod, Project));
        AddChild(new ProjectTreeItemModSkillData(Mod, Project));
        AddChild(new ProjectTreeItemModStaticSkillData(Mod, Project));
        AddChild(new ProjectTreeItemModBuffData(Mod, Project));
    }
}