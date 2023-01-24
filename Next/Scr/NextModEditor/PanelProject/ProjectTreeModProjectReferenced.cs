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
        AddChild(new ProjectTreeItemModAffixData(Mod, Project)
        {
            Editable = false,
        });
        AddChild(new ProjectTreeItemModCreateAvatarData(Mod, Project)
        {
            Editable = false,
        });
        AddChild(new ProjectTreeItemModItemData(Mod, Project)
        {
            Editable = false,
        });
        AddChild(new ProjectTreeItemModSkillData(Mod, Project)
        {
            Editable = false,
        });
        AddChild(new ProjectTreeItemModStaticSkillData(Mod, Project)
        {
            Editable = false,
        });
        AddChild(new ProjectTreeItemModBuffData(Mod, Project)
        {
            Editable = false,
        });
    }

    public override string Name => Project.ProjectName;
    public override string Icon => "ui://NextCore/icon_link";
}