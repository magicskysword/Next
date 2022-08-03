using System.Linq;
using FairyGUI;
using SkySwordKill.Next.FGUI.Component;
using SkySwordKill.NextModEditor.Mod.Data;
using SkySwordKill.Next.Extension;
using SkySwordKill.NextEditor.Mod;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.NextEditor.PanelPage
{
    public class PanelTableModCreateAvatarPage : PanelTablePageBase<ModCreateAvatarData>
    {
        public PanelTableModCreateAvatarPage(string name, ModWorkshop mod, ModProject project) : base(name, mod, project)
        {
        }
        
        public override ModDataTableDataList<ModCreateAvatarData> ModDataTableDataList { get; set; }

        protected override void OnInit()
        {
            ModDataTableDataList = new ModDataTableDataList<ModCreateAvatarData>(Project.CreateAvatarData);
            
            AddTableHeader(new TableInfo(
                "ModEditor.Main.modCreateAvatar.id".I18N(),
                TableInfo.DEFAULT_GRID_WIDTH,
                data =>
                {
                    var buff = (ModCreateAvatarData)data;
                    return buff.Id.ToString();
                }));

            AddTableHeader(new TableInfo(
                "ModEditor.Main.modCreateAvatar.name".I18N(),
                TableInfo.DEFAULT_GRID_WIDTH,
                data =>
                {
                    var buff = (ModCreateAvatarData)data;
                    return buff.Name;
                }));

            AddTableHeader(new TableInfo(
                "ModEditor.Main.modCreateAvatar.createType".I18N(),
                TableInfo.DEFAULT_GRID_WIDTH,
                data =>
                {
                    var buff = (ModCreateAvatarData)data;
                    return
                        $"{buff.TalentTypeRelation} : {ModEditorManager.I.GetCreateAvatarTalentTypeDesc(buff.TalentTypeRelation)}";
                }));

            AddTableHeader(new TableInfo(
                "ModEditor.Main.modCreateAvatar.group".I18N(),
                TableInfo.DEFAULT_GRID_WIDTH * 0.5f,
                data =>
                {
                    var buff = (ModCreateAvatarData)data;
                    return buff.Group.ToString();
                }));

            AddTableHeader(new TableInfo(
                "ModEditor.Main.modCreateAvatar.desc".I18N(),
                TableInfo.DEFAULT_GRID_WIDTH * 3,
                data =>
                {
                    var buff = (ModCreateAvatarData)data;
                    return buff.Desc;
                }));
        }

        protected override void OnInspectItem(IModData data)
        {
            if (data == null)
            {
                return;
            }

            var createAvatarData = (ModCreateAvatarData)data;

            AddDrawer(new CtlIDPropertyDrawer(
                    "ModEditor.Main.modCreateAvatar.id".I18N(),
                    createAvatarData,
                    () => Project.CreateAvatarData,
                    theData =>
                    {
                        var curData = (ModCreateAvatarData)theData;
                        return $"{curData.Id} {curData.Name}";
                    },
                    (theData, newId) =>
                    {
                        Project.CreateAvatarSeidDataGroup.ChangeSeidID(theData.Id, newId);
                        theData.Id = newId;
                        Project.CreateAvatarData.ModSort();
                        CurInspectIndex = Project.CreateAvatarData.FindIndex(modData => modData == theData);
                    },
                    (theData, otherData) =>
                    {
                        Project.CreateAvatarSeidDataGroup.SwiftSeidID(otherData.Id, theData.Id);
                        (otherData.Id, theData.Id) = (theData.Id, otherData.Id);
                        Project.CreateAvatarData.ModSort();
                        CurInspectIndex = Project.CreateAvatarData.FindIndex(modData => modData == theData);
                    },
                    () => { Inspector.Refresh(); })
            );

            AddDrawer(new CtlStringPropertyDrawer(
                    "ModEditor.Main.modCreateAvatar.name".I18N(),
                    str => createAvatarData.Name = str,
                    () => createAvatarData.Name)
            );

            AddDrawer(new CtlIntPropertyDrawer(
                    "ModEditor.Main.modCreateAvatar.group".I18N(),
                    value => createAvatarData.Group = value,
                    () => createAvatarData.Group)
            );

            AddDrawer(new CtlIntPropertyDrawer(
                "ModEditor.Main.modCreateAvatar.cost".I18N(),
                value => createAvatarData.Cost = value,
                () => createAvatarData.Cost)
            );

            AddDrawer(new CtlDropdownPropertyDrawer(
                "ModEditor.Main.modCreateAvatar.createType".I18N(),
                () => ModEditorManager.I.CreateAvatarDataTalentTypes.Select(type => $"{type.TypeID} : {type.Desc}"),
                index =>
                {
                    createAvatarData.SetTalentType(ModEditorManager.I.CreateAvatarDataTalentTypes[index]);
                },
                () => ModEditorManager.I.CreateAvatarDataTalentTypes.FindIndex(type =>
                    type.TypeID == createAvatarData.TalentTypeRelation))
            );
            
            AddDrawer(new CtlDropdownPropertyDrawer(
                "解锁需求".I18NTodo(),
                () => ModEditorManager.I.CreateAvatarDataLevelTypes.Select(type => $"{type.TypeID} : {type.Desc}"),
                index =>
                {
                    createAvatarData.RequireLevel = ModEditorManager.I.CreateAvatarDataLevelTypes[index].TypeID;
                },
                () => ModEditorManager.I.CreateAvatarDataLevelTypes.FindIndex(type =>
                    type.TypeID == createAvatarData.TalentTypeRelation))
            );

            AddDrawer(new CtlSeidDataPropertyDrawer(
                "特性",
                createAvatarData.Id,
                createAvatarData.SeidList,
                Mod,
                Project.CreateAvatarSeidDataGroup,
                ModEditorManager.I.CreateAvatarSeidMetas,
                seidId =>
                {
                    if (ModEditorManager.I.CreateAvatarSeidMetas.TryGetValue(seidId, out var seidData))
                        return $"{seidId}  {seidData.Name}";
                    return $"{seidId}  ???";
                })
            );
            
            AddDrawer(new CtlStringAreaPropertyDrawer(
                "效果".I18N(),
                value => createAvatarData.Desc = value,
                () => createAvatarData.Desc)
            );
            
            AddDrawer(new CtlStringAreaPropertyDrawer(
                "背景描述".I18N(),
                value => createAvatarData.Info = value,
                () => createAvatarData.Info)
            );
        }
    }
}