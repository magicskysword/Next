using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkySwordKill.Next.Extension;
using SkySwordKill.Next.FGUI.Component;
using SkySwordKill.NextEditor.Mod;
using SkySwordKill.NextFGUI.NextCore;
using SkySwordKill.NextModEditor.Mod.Data;

namespace SkySwordKill.NextEditor.PanelPage
{
    public class PanelTableModSkillDataPage : PanelTablePageBase<ModSkillData>
    {
        public PanelTableModSkillDataPage(string name, ModWorkshop mod, ModProject project) : base(name, mod, project)
        {
        }

        public override ModDataTableDataList<ModSkillData> ModDataTableDataList { get; set; }
        protected override void OnInit()
        {
            ModDataTableDataList = new ModDataTableDataList<ModSkillData>(Project.SkillData)
            {
                OnRemoveItem = data =>
                {
                    
                }
            };
            
            AddTableHeader(new TableInfo(
                "ID".I18NTodo(),
                TableInfo.DEFAULT_GRID_WIDTH,
                data =>
                {
                    var skill = (ModSkillData)data;
                    return skill.Id.ToString();
                }));
            
            AddTableHeader(new TableInfo(
                "神通ID".I18NTodo(),
                TableInfo.DEFAULT_GRID_WIDTH,
                data =>
                {
                    var skill = (ModSkillData)data;
                    return $"{skill.SkillId.ToString()}(lv.{skill.SkillLv})";
                }));
            
            AddTableHeader(new TableInfo(
                "等阶".I18NTodo(),
                TableInfo.DEFAULT_GRID_WIDTH * 2,
                data =>
                {
                    var skill = (ModSkillData)data;
                    return ModUtils.GetSkillLevelName(skill.Quality, skill.Phase);
                }));

            AddTableHeader(new TableInfo(
                "名称".I18NTodo(),
                TableInfo.DEFAULT_GRID_WIDTH,
                data =>
                {
                    var skill = (ModSkillData)data;
                    return skill.Name;
                }));
            
            AddTableHeader(new TableInfo(
                "描述".I18NTodo(),
                TableInfo.DEFAULT_GRID_WIDTH * 2,
                data =>
                {
                    var skill = (ModSkillData)data;
                    return skill.Desc;
                }));
        }

        protected override void OnInspectItem(ModSkillData data)
        {
            if (data == null)
            {
                return;
            }

            AddDrawer(new CtlIDPropertyDrawer(
                "ID".I18NTodo(),
                data,
                () => Project.BuffData,
                theData =>
                {
                    var curData = (ModSkillData)theData;
                    return OnGetDataName(curData);
                },
                (theData, newId) =>
                {
                    theData.Id = newId;
                    Project.SkillData.ModSort();
                    CurInspectIndex = Project.SkillData.FindIndex(modData => modData == theData);
                },
                (theData, otherData) =>
                {
                    (otherData.Id, theData.Id) = (theData.Id, otherData.Id);
                    Project.SkillData.ModSort();
                    CurInspectIndex = Project.SkillData.FindIndex(modData => modData == theData);
                },
                () => { Inspector.Refresh(); })
            );

            AddDrawer(new CtlIntPropertyDrawer(
                    "神通ID".I18NTodo(),
                    value => data.SkillId = value,
                    () => data.SkillId
                )
            );

            AddDrawer(new CtlIntPropertyDrawer(
                    "神通等级".I18NTodo(),
                    value => data.SkillLv = value,
                    () => data.SkillLv
                )
            );

            AddDrawer(new CtlStringPropertyDrawer(
                    "名称".I18NTodo(),
                    value => data.Name = value,
                    () => data.Name
                )
            );

            AddDrawer(new CtlIntPropertyDrawer(
                    "释放优先级".I18NTodo(),
                    value => data.SkillPriority = value,
                    () => data.SkillPriority
                )
            );

            AddDrawer(new CtlStringPropertyDrawer(
                    "特效".I18NTodo(),
                    value => data.Effect = value,
                    () => data.Effect
                )
            );

            AddDrawer(new CtlStringAreaPropertyDrawer(
                    "描述".I18NTodo(),
                    value => data.Desc = value,
                    () => data.Desc
                )
            );
            
            AddDrawer(new CtlStringAreaPropertyDrawer(
                    "图鉴描述".I18NTodo(),
                    value => data.GuideDesc = value,
                    () => data.GuideDesc,
                    true,
                    onAnalysisRef: str => ModUtils.AnalysisRefData(Mod ,str)
                )
            );

            AddDrawer(new CtlDropdownPropertyDrawer(
                    "请教类型".I18NTodo(),
                    () => ModEditorManager.I.SkillDataConsultTypes.Select(type => $"{type.Id} : {type.Desc}"),
                    index => data.ConsultType = ModEditorManager.I.SkillDataConsultTypes[index].Id,
                    () => ModEditorManager.I.SkillDataConsultTypes.FindIndex(type => type.Id == data.ConsultType)
                )
            );
            
            AddDrawer(new CtlIntArrayBindTablePropertyDrawer(
                "攻击类型".I18NTodo(),
                value => data.AttackTypeList = value,
                () => data.AttackTypeList,
                value =>
                {
                    var sb = new StringBuilder();
                    for (var index = 0; index < value.Count; index++)
                    {
                        var id = value[index];
                        var attackType = ModEditorManager.I.AttackTypes.Find(type => type.Id == id);
                        if (attackType != null)
                        {
                            sb.Append($"【{attackType.Id}】{attackType.Desc}");
                        }
                        else
                        {
                            sb.Append($"【{id}】？");
                        }
                        if(index != value.Count - 1)
                            sb.Append("\n");
                    }

                    return sb.ToString();
                },
                new List<TableInfo>()
                {
                    new TableInfo(
                        "ID".I18NTodo(),
                        TableInfo.DEFAULT_GRID_WIDTH,
                        value => ((ModAttackType)value).Id.ToString()
                        ),
                    new TableInfo(
                        "类型".I18NTodo(),
                        TableInfo.DEFAULT_GRID_WIDTH * 1.5f,
                        value => ((ModAttackType)value).Desc
                    ),
                },
                () => new List<IModData>(ModEditorManager.I.AttackTypes)
                )
            );

            AddDrawer(new CtlIntArrayBindTablePropertyDrawer(
                    "词缀".I18NTodo(),
                    list => data.AffixList = list,
                    () => data.AffixList,
                    list => Mod.GetAffixDesc(list),
                    new List<TableInfo>()
                    {
                        new TableInfo("ID".I18NTodo(),
                            TableInfo.DEFAULT_GRID_WIDTH / 2,
                            getData => ((ModAffixData)getData).Id.ToString()),
                        new TableInfo("名称".I18NTodo(),
                            TableInfo.DEFAULT_GRID_WIDTH / 2,
                            getData => ((ModAffixData)getData).Name),
                        new TableInfo("描述".I18NTodo(),
                            TableInfo.DEFAULT_GRID_WIDTH * 3,
                            getData => ((ModAffixData)getData).Desc),
                    },
                    () => new List<IModData>(Mod.GetAllAffixData())
                )
            );
        }

        public override string OnGetDataName(ModSkillData data)
        {
            return $"{data.Id} {data.Name}";
        }

        protected override ModSkillData OnPasteData(CopyData copyData, int targetId)
        {
            var oldId = copyData.Data.Id;
            var json = copyData.Data.GetJsonData();
            var skillData = json.ToObject<ModSkillData>();
            skillData.Id = targetId;
            Project.SkillData.Add(skillData);
            Project.SkillData.ModSort();
            return skillData;
        }
    }
}