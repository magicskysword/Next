using System.Collections.Generic;
using System.Linq;
using SkySwordKill.Next.FGUI.Component;
using SkySwordKill.NextModEditor.Mod.Data;
using SkySwordKill.Next.Extension;
using SkySwordKill.NextFGUI.NextCore;
using SkySwordKill.NextModEditor.Mod;

namespace SkySwordKill.NextModEditor.PanelPage
{
    public class PanelTableModBuffDataPage : PanelTablePageBase<ModBuffData>
    {
        public PanelTableModBuffDataPage(string name, ModWorkshop mod, ModProject project) : base(name, mod, project)
        {
        }
        
        public override ModDataTableDataList<ModBuffData> ModDataTableDataList { get; set; }

        protected override void OnInit()
        {
            ModDataTableDataList = new ModDataTableDataList<ModBuffData>(Project.BuffData);
            
            AddTableHeader(new TableInfo(
                "ModEditor.Main.modBuffData.id".I18N(),
                TableInfo.DEFAULT_GRID_WIDTH,
                data =>
                {
                    var buff = (ModBuffData)data;
                    return buff.Id.ToString();
                }));

            AddTableHeader(new TableInfo(
                "ModEditor.Main.modBuffData.name".I18N(),
                TableInfo.DEFAULT_GRID_WIDTH,
                data =>
                {
                    var buff = (ModBuffData)data;
                    return buff.Name;
                }));

            AddTableHeader(new TableInfo(
                "ModEditor.Main.modBuffData.desc".I18N(),
                TableInfo.DEFAULT_GRID_WIDTH * 3,
                data =>
                {
                    var buff = (ModBuffData)data;
                    return buff.Desc;
                }));
        }

        protected override void OnInspectItem(ModBuffData data)
        {
            if (data == null)
            {
                return;
            }

            AddDrawer(new CtlIDPropertyDrawer(
                    "ModEditor.Main.modBuffData.id".I18N(),
                    data,
                    () => Project.BuffData,
                    theData =>
                    {
                        var curData = (ModBuffData)theData;
                        return OnGetDataName(curData);
                    },
                    (theData, newId) =>
                    {
                        Project.BuffSeidDataGroup.ChangeSeidID(theData.Id, newId);
                        theData.Id = newId;
                        Project.BuffData.ModSort();
                        CurInspectIndex = Project.BuffData.FindIndex(modData => modData == theData);
                    },
                    (theData, otherData) =>
                    {
                        Project.BuffSeidDataGroup.SwiftSeidID(otherData.Id, theData.Id);
                        (otherData.Id, theData.Id) = (theData.Id, otherData.Id);
                        Project.BuffData.ModSort();
                        CurInspectIndex = Project.BuffData.FindIndex(modData => modData == theData);
                    },
                    () => { Inspector.Refresh(); })
            );

            AddDrawer(new CtlStringPropertyDrawer(
                    "ModEditor.Main.modBuffData.name".I18N(),
                    str => data.Name = str,
                    () => data.Name)
            );

            AddDrawer(new CtlStringPropertyDrawer(
                "ModEditor.Main.modBuffData.skillEffect".I18N(),
                str => data.SkillEffect = str,
                () => data.SkillEffect)
            );

            var iconDrawer = new CtlIconPreviewDrawer(() => Mod.GetBuffIconUrl(data));
            AddDrawer(new CtlIntPropertyDrawer(
                    "ModEditor.Main.modBuffData.icon".I18NTodo(),
                    value => data.Icon = value,
                    () => data.Icon).AddChangeListener(Inspector.Refresh)
            ).AddChainDrawer(iconDrawer);
            
            AddDrawer(iconDrawer);

            AddDrawer(new CtlStringAreaPropertyDrawer(
                    "ModEditor.Main.modBuffData.desc".I18N(),
                    str => data.Desc = str,
                    () => data.Desc)
            );
            
            AddDrawer(new CtlDropdownPropertyDrawer(
                "类型".I18NTodo(),
                () => ModEditorManager.I.BuffDataBuffTypes.Select(type => $"{type.TypeID} : {type.TypeName}"),
                index => data.BuffType = ModEditorManager.I.BuffDataBuffTypes[index].TypeID,
                () => ModEditorManager.I.BuffDataBuffTypes.FindIndex(type =>
                    type.TypeID == data.BuffType))
            );
            
            AddDrawer(new CtlDropdownPropertyDrawer(
                "叠加类型".I18NTodo(),
                () => ModEditorManager.I.BuffDataOverlayTypes.Select(type => $"{type.ID} : {type.Desc}"),
                index => data.BuffOverlayType = ModEditorManager.I.BuffDataOverlayTypes[index].ID,
                () => ModEditorManager.I.BuffDataOverlayTypes.FindIndex(type =>
                    type.ID == data.BuffOverlayType))
            );
            
            AddDrawer(new CtlDropdownPropertyDrawer(
                "触发方式".I18NTodo(),
                () => ModEditorManager.I.BuffDataTriggerTypes.Select(type => $"{type.ID} : {type.Desc}"),
                index => data.Trigger = ModEditorManager.I.BuffDataTriggerTypes[index].ID,
                () => ModEditorManager.I.BuffDataTriggerTypes.FindIndex(type =>
                    type.ID == data.Trigger))
            );
            
            AddDrawer(new CtlDropdownPropertyDrawer(
                "移除方式".I18NTodo(),
                () => ModEditorManager.I.BuffDataRemoveTriggerTypes.Select(type => $"{type.TypeID} : {type.TypeName}"),
                index => data.RemoverTrigger = ModEditorManager.I.BuffDataRemoveTriggerTypes[index].TypeID,
                () => ModEditorManager.I.BuffDataRemoveTriggerTypes.FindIndex(type =>
                    type.TypeID == data.RemoverTrigger))
            );

            AddDrawer(new CtlCheckboxPropertyDrawer(
                "是否隐藏".I18NTodo(),
                isOn => data.IsHide = isOn ? 1 : 0,
                () => data.IsHide == 1)
            );
            
            AddDrawer(new CtlCheckboxPropertyDrawer(
                "只显示一层".I18NTodo(),
                isOn => data.ShowOnlyOne = isOn ? 1 : 0,
                () => data.ShowOnlyOne == 1)
            );

            AddDrawer(new CtlIntArrayBindTablePropertyDrawer(
                "ModEditor.Main.modBuffData.affix".I18N(),
                list => data.AffixList = list,
                () => data.AffixList,
                list => Mod.GetAffixDesc(list),
                new List<TableInfo>()
                {
                    new TableInfo("ModEditor.Main.modAffixData.id".I18N(),
                        TableInfo.DEFAULT_GRID_WIDTH / 2,
                        getData => ((ModAffixData)getData).Id.ToString()),
                    new TableInfo("ModEditor.Main.modAffixData.name".I18N(),
                        TableInfo.DEFAULT_GRID_WIDTH / 2,
                        getData => ((ModAffixData)getData).Name),
                    new TableInfo("ModEditor.Main.modAffixData.desc".I18N(),
                        TableInfo.DEFAULT_GRID_WIDTH * 3,
                        getData => ((ModAffixData)getData).Desc),
                },
                () => new List<IModData>(Mod.GetAllAffixData())
            ));
            
            AddDrawer(new CtlSeidDataPropertyDrawer(
                "特性",
                data.Id,
                data.SeidList,
                Mod,
                Project.BuffSeidDataGroup,
                ModEditorManager.I.BuffSeidMetas,
                seidId =>
                {
                    if (ModEditorManager.I.BuffSeidMetas.TryGetValue(seidId, out var seidData))
                        return $"{seidId}  {seidData.Name}";
                    return $"{seidId}  ???";
                })
            );
        }

        public override string OnGetDataName(ModBuffData data)
        {
            return $"{data.Id} {data.Name}";
        }

        protected override ModBuffData OnPasteData(CopyData copyData, int targetId)
        {
            var oldId = copyData.Data.Id;
            var json = copyData.Data.GetJsonData();
            var buffData = json.ToObject<ModBuffData>();
            buffData.Id = targetId;
            Project.BuffData.Add(buffData);
            Project.BuffSeidDataGroup.CopyAllSeid(copyData.Project.BuffSeidDataGroup, oldId, targetId);
            Project.BuffData.ModSort();
            return buffData;
        }
    }
}