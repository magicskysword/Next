using System.Linq;
using FairyGUI;
using SkySwordKill.Next.FGUI.Component;
using SkySwordKill.NextModEditor.Mod.Data;
using SkySwordKill.Next.Extension;
using SkySwordKill.NextEditor.Mod;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.NextEditor.PanelPage
{
    public class PanelTableModCreateAvatarDataPage : PanelTablePageBase<ModCreateAvatarData>
    {
        public PanelTableModCreateAvatarDataPage(string name, ModWorkshop mod, ModProject project) : base(name, mod, project)
        {
        }
        
        public override ModDataTableDataList<ModCreateAvatarData> ModDataTableDataList { get; set; }

        protected override void OnInit()
        {
            ModDataTableDataList = new ModDataTableDataList<ModCreateAvatarData>(Project.CreateAvatarData)
            {
                OnRemoveItem = data =>
                {
                    Project.CreateAvatarSeidDataGroup.RemoveAllSeid(data.Id);
                }
            };
            
            AddTableHeader(new TableInfo(
                "ModEditor.Main.modCreateAvatarData.id".I18N(),
                TableInfo.DEFAULT_GRID_WIDTH,
                data =>
                {
                    var buff = (ModCreateAvatarData)data;
                    return buff.Id.ToString();
                }));

            AddTableHeader(new TableInfo(
                "ModEditor.Main.modCreateAvatarData.name".I18N(),
                TableInfo.DEFAULT_GRID_WIDTH,
                data =>
                {
                    var buff = (ModCreateAvatarData)data;
                    return buff.Name;
                }));

            AddTableHeader(new TableInfo(
                "ModEditor.Main.modCreateAvatarData.createType".I18N(),
                TableInfo.DEFAULT_GRID_WIDTH,
                data =>
                {
                    var buff = (ModCreateAvatarData)data;
                    return
                        $"{buff.TalentTypeRelation} : {ModEditorManager.I.GetCreateAvatarTalentTypeDesc(buff.TalentTypeRelation)}";
                }));

            AddTableHeader(new TableInfo(
                "ModEditor.Main.modCreateAvatarData.group".I18N(),
                TableInfo.DEFAULT_GRID_WIDTH * 0.5f,
                data =>
                {
                    var buff = (ModCreateAvatarData)data;
                    return buff.Group.ToString();
                }));

            AddTableHeader(new TableInfo(
                "ModEditor.Main.modCreateAvatarData.desc".I18N(),
                TableInfo.DEFAULT_GRID_WIDTH * 3,
                data =>
                {
                    var buff = (ModCreateAvatarData)data;
                    return buff.Desc;
                }));
        }

        protected override void OnInspectItem(ModCreateAvatarData data)
        {
            if (data == null)
            {
                return;
            }
            
            AddDrawer(new CtlIDPropertyDrawer(
                    "ModEditor.Main.modCreateAvatarData.id".I18N(),
                    data,
                    () => Project.CreateAvatarData,
                    theData =>
                    {
                        var curData = (ModCreateAvatarData)theData;
                        return OnGetDataName(curData);
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
                    "ModEditor.Main.modCreateAvatarData.name".I18N(),
                    str => data.Name = str,
                    () => data.Name)
            );

            AddDrawer(new CtlIntPropertyDrawer(
                    "ModEditor.Main.modCreateAvatarData.group".I18N(),
                    value => data.Group = value,
                    () => data.Group)
            );

            AddDrawer(new CtlIntPropertyDrawer(
                "ModEditor.Main.modCreateAvatarData.cost".I18N(),
                value => data.Cost = value,
                () => data.Cost)
            );

            AddDrawer(new CtlDropdownPropertyDrawer(
                "ModEditor.Main.modCreateAvatarData.createType".I18N(),
                () => ModEditorManager.I.CreateAvatarDataTalentTypes.Select(type => $"{type.TypeID} : {type.Desc}"),
                index =>
                {
                    data.SetTalentType(ModEditorManager.I.CreateAvatarDataTalentTypes[index]);
                },
                () => ModEditorManager.I.CreateAvatarDataTalentTypes.FindIndex(type =>
                    type.TypeID == data.TalentTypeRelation))
            );
            
            AddDrawer(new CtlDropdownPropertyDrawer(
                "解锁需求".I18NTodo(),
                () => ModEditorManager.I.LevelTypes.Select(type => $"{type.TypeID} : {type.Desc}"),
                index =>
                {
                    data.RequireLevel = ModEditorManager.I.LevelTypes[index].TypeID;
                },
                () => ModEditorManager.I.LevelTypes.FindIndex(type =>
                    type.TypeID == data.TalentTypeRelation))
            );

            AddDrawer(new CtlSeidDataPropertyDrawer(
                "特性",
                data.Id,
                data.SeidList,
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
                value => data.Desc = value,
                () => data.Desc)
            );
            
            AddDrawer(new CtlStringAreaPropertyDrawer(
                "背景描述".I18N(),
                value => data.Info = value,
                () => data.Info)
            );
        }

        public override string OnGetDataName(ModCreateAvatarData data)
        {
            return $"{data.Id} {data.Name}";
        }

        protected override ModCreateAvatarData OnPasteData(CopyData copyData, int targetId)
        {
            var oldId = copyData.Data.Id;
            var json = copyData.Data.GetJsonData();
            var createAvatar = json.ToObject<ModCreateAvatarData>();
            createAvatar.Id = targetId;
            Project.CreateAvatarData.Add(createAvatar);
            Project.CreateAvatarSeidDataGroup.CopyAllSeid(copyData.Project.CreateAvatarSeidDataGroup, oldId, targetId);
            Project.CreateAvatarData.ModSort();
            return createAvatar;
        }
    }
}