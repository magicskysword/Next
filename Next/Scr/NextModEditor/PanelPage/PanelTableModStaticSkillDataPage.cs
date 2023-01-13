using SkySwordKill.NextModEditor.Mod;
using SkySwordKill.NextModEditor.Mod.Data;

namespace SkySwordKill.NextModEditor.PanelPage;

public class PanelTableModStaticSkillDataPage : PanelTablePageBase<ModStaticSkillData>
{
    public PanelTableModStaticSkillDataPage(string name, ModWorkshop mod, ModProject project) : base(name, mod, project)
    {
    }

    public override ModDataTableDataList<ModStaticSkillData> ModDataTableDataList { get; set; }
    protected override void OnInit()
    {
        
    }

    protected override void OnInspectItem(ModStaticSkillData data)
    {
        
    }

    public override string OnGetDataName(ModStaticSkillData data)
    {
        return string.Empty;
    }

    protected override ModStaticSkillData OnPasteData(CopyData copyData, int targetId)
    {
        var oldId = copyData.Data.Id;
        var json = copyData.Data.GetJsonData();
        var skillData = json.ToObject<ModStaticSkillData>();
        skillData.Id = targetId;
        // Project.SkillData.Add(skillData);
        // Project.SkillSeidDataGroup.CopyAllSeid(copyData.Project.SkillSeidDataGroup, oldId, targetId);
        // Project.SkillData.ModSort();
        return skillData;
    }
}