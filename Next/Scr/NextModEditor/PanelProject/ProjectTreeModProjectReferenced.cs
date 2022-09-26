using SkySwordKill.Next.Extension;
using SkySwordKill.Next.FGUI.Component;
using SkySwordKill.NextModEditor.Mod;

namespace SkySwordKill.NextModEditor.PanelProject;

public class ProjectTreeModProjectReferenced : ProjectTreeFolder
{
    public ProjectTreeModProjectReferenced(ModWorkshop mod, ModProject project)
    {
        Mod = mod;
        Project = project;
        Init();
    }
        
    public ModWorkshop Mod { get; set; }
    public ModProject Project { get; set; }

    public void Init()
    {
        Children.Add(new ProjectTreeItemModAffixData(Mod, Project)
        {
            Editable = false,
        });
        Children.Add(new ProjectTreeItemModCreateAvatarData(Mod, Project)
        {
            Editable = false,
        });
        Children.Add(new ProjectTreeItemModItemData(Mod, Project)
        {
            Editable = false,
        });
        Children.Add(new ProjectTreeItemModSkillData(Mod, Project)
        {
            Editable = false,
        });
        Children.Add(new ProjectTreeItemModBuffData(Mod, Project)
        {
            Editable = false,
        });
    }

    public override string Name => Project.ProjectName;
    public override string Icon => "ui://NextCore/icon_link";
}