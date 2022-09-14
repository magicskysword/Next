using System.Linq;
using SkySwordKill.Next.Extension;
using SkySwordKill.Next.FGUI.Component;
using SkySwordKill.NextEditor.Mod;
using SkySwordKill.NextFGUI.NextCore;
using SkySwordKill.NextModEditor.Mod.Data;

namespace SkySwordKill.NextEditor.PanelPage
{
    public class PanelTableModAffixDataPage : PanelTablePageBase<ModAffixData>
    {
        public PanelTableModAffixDataPage(string name, ModWorkshop mod, ModProject project) : base(name, mod, project)
        {
        }

        public override ModDataTableDataList<ModAffixData> ModDataTableDataList { get; set; }
        protected override void OnInit()
        {
            ModDataTableDataList = new ModDataTableDataList<ModAffixData>(Project.AffixData);
            
            AddTableHeader(new TableInfo(
                "ID".I18NTodo(),
                TableInfo.DEFAULT_GRID_WIDTH,
                data =>
                {
                    var affixData = (ModAffixData)data;
                    return affixData.Id.ToString();
                }));
            
            AddTableHeader(new TableInfo(
                "名称".I18NTodo(),
                TableInfo.DEFAULT_GRID_WIDTH,
                data =>
                {
                    var affixData = (ModAffixData)data;
                    return affixData.Name;
                }));
            
            AddTableHeader(new TableInfo(
                "词缀项目".I18NTodo(),
                TableInfo.DEFAULT_GRID_WIDTH,
                data =>
                {
                    var affixData = (ModAffixData)data;
                    return $"{affixData.ProjectTypeNum} : {affixData.ProjectTypeName}";
                }));
            
            AddTableHeader(new TableInfo(
                "词缀类型".I18NTodo(),
                TableInfo.DEFAULT_GRID_WIDTH,
                data =>
                {
                    var affixData = (ModAffixData)data;
                    var projectType = ModEditorManager.I.AffixDataAffixTypes.Find(x => x.TypeID == affixData.AffixType);
                    if(projectType != null)
                        return $"{projectType.TypeID} : {projectType.TypeName}";
                    return $"{affixData.AffixType} : ???";
                }));
            
            AddTableHeader(new TableInfo(
                "描述".I18NTodo(),
                TableInfo.DEFAULT_GRID_WIDTH * 3,
                data =>
                {
                    var affixData = (ModAffixData)data;
                    return affixData.Desc;
                }));
        }

        protected override void OnInspectItem(ModAffixData data)
        {
            if (data == null)
            {
                return;
            }

            AddDrawer(new CtlIDPropertyDrawer(
                "ID".I18NTodo(),
                data,
                () => Project.AffixData,
                theData =>
                {
                    var curData = (ModAffixData)theData;
                    return OnGetDataName(curData);
                },
                (theData, newId) =>
                {
                    theData.Id = newId;
                    Project.AffixData.ModSort();
                    CurInspectIndex = Project.AffixData.FindIndex(modData => modData == theData);
                },
                (theData, otherData) =>
                {
                    (otherData.Id, theData.Id) = (theData.Id, otherData.Id);
                    Project.AffixData.ModSort();
                    CurInspectIndex = Project.AffixData.FindIndex(modData => modData == theData);
                },
                () => { Inspector.Refresh(); })
            );
            
            AddDrawer(new CtlStringPropertyDrawer(
                "名称".I18NTodo(),
                str => data.Name = str,
                () => data.Name)
            );

            AddDrawer(new CtlDropdownPropertyDrawer(
                "词缀项目".I18NTodo(),
                () => ModEditorManager.I.AffixDataProjectTypes.Select(p => $"{p.TypeNum} : {p.TypeName}"),
                i => data.SetProjectType(ModEditorManager.I.AffixDataProjectTypes[i]),
                () => ModEditorManager.I.AffixDataProjectTypes.FindIndex(p => p.TypeNum == data.ProjectTypeNum))
            );
            
            AddDrawer(new CtlDropdownPropertyDrawer(
                "词缀类型".I18NTodo(),
                () => ModEditorManager.I.AffixDataAffixTypes.Select(p => $"{p.TypeID} : {p.TypeName}"),
                i => data.AffixType = ModEditorManager.I.AffixDataAffixTypes[i].TypeID,
                () => ModEditorManager.I.AffixDataAffixTypes.FindIndex(p => p.TypeID == data.AffixType))
            );
            
            AddDrawer(new CtlStringAreaPropertyDrawer(
                "描述".I18NTodo(),
                str => data.Desc = str,
                () => data.Desc)
            );
        }

        public override string OnGetDataName(ModAffixData data)
        {
            return $"{data.Id} {data.Name}";
        }

        protected override ModAffixData OnPasteData(CopyData copyData, int targetId)
        {
            var json = copyData.Data.GetJsonData();
            var affixData = json.ToObject<ModAffixData>();
            affixData.Id = targetId;
            Project.AffixData.Add(affixData);
            Project.AffixData.ModSort();
            return affixData;
        }
    }
}