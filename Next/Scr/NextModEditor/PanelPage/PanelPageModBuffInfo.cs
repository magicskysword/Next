using System.Collections.Generic;
using SkySwordKill.NextModEditor.Mod.Data;
using SkySwordKill.Next.Extension;
using SkySwordKill.NextEditor.Drawer;
using SkySwordKill.NextEditor.Mod;

namespace SkySwordKill.NextEditor.PanelPage
{
    public class PanelPageModBuffInfo : PanelPageTable
{
    public override void OnAdd()
    {
        base.OnAdd();

        AddTableHeader(new TableInfo(
            "ModEditor.Main.modBuffInfo.id".I18N(),
            TableInfo.DEFAULT_GRID_WIDTH,
            data =>
            {
                var buff = (ModBuffData)data;
                return buff.ID.ToString();
            }));

        AddTableHeader(new TableInfo(
            "ModEditor.Main.modBuffInfo.name".I18N(),
            TableInfo.DEFAULT_GRID_WIDTH,
            data =>
            {
                var buff = (ModBuffData)data;
                return buff.Name;
            }));

        AddTableHeader(new TableInfo(
            "ModEditor.Main.modBuffInfo.desc".I18N(),
             TableInfo.DEFAULT_GRID_WIDTH * 3,
            data =>
            {
                var buff = (ModBuffData)data;
                return buff.Desc;
            }));
    }

    protected override void OnInspectItem(object data)
    {
        Inspector.Clear();

        if (data == null)
        {
            return;
        }

        var buffData = (ModBuffData)data;

        Inspector.AddDrawer(new ModIDPropertyDrawer(
                "ModEditor.Main.modBuffInfo.id".I18N(),
                buffData,
                () => Project.BuffData,
                theData =>
                {
                    var curData = (ModBuffData)theData;
                    return $"{curData.ID} {curData.Name}";
                },
                (theData, newId) =>
                {
                    Project.BuffSeidDataGroup.ChangeSeidID(theData.ID, newId);
                    theData.ID = newId;
                    Project.BuffData.ModSort();
                    CurInspectIndex = Project.BuffData.FindIndex(modData => modData == theData);
                },
                (theData, otherData) =>
                {
                    Project.BuffSeidDataGroup.SwiftSeidID(otherData.ID, theData.ID);
                    (otherData.ID, theData.ID) = (theData.ID, otherData.ID);
                    Project.BuffData.ModSort();
                    CurInspectIndex = Project.BuffData.FindIndex(modData => modData == theData);
                },
                () => { Inspector.Refresh(); })
            { OnChanged = RefreshTable }
        );

        Inspector.AddDrawer(new ModStringPropertyDrawer(
                "ModEditor.Main.modBuffInfo.name".I18N(),
                str => buffData.Name = str,
                () => buffData.Name)
            { OnChanged = RefreshCurrentRow }
        );

        Inspector.AddDrawer(new ModStringPropertyDrawer(
            "ModEditor.Main.modBuffInfo.skillEffect".I18N(),
            str => buffData.SkillEffect = str,
            () => buffData.SkillEffect)
        );

        Inspector.AddDrawer(new ModIntPropertyDrawer(
            "ModEditor.Main.modBuffInfo.icon".I18N(),
            num => buffData.Icon = num,
            () => buffData.Icon)
        );

        Inspector.AddDrawer(new ModStringAreaPropertyDrawer(
                "ModEditor.Main.modBuffInfo.desc".I18N(),
                str => buffData.Desc = str,
                () => buffData.Desc)
            { OnChanged = RefreshCurrentRow }
        );
        
        Inspector.AddDrawer(new ModIntBindMultiDataPropertyDrawer(
            "ModEditor.Main.modBuffInfo.affix".I18N(),
            list => buffData.AffixList = list,
            () => buffData.AffixList,
            list => Project.GetAffixDesc(list),
            new List<TableInfo>()
            {
                new TableInfo("ModEditor.Main.modAffixData.id".I18N(),
                    TableInfo.DEFAULT_GRID_WIDTH / 2,
                    getData=>((ModAffixData)getData).ID.ToString()),
                new TableInfo("ModEditor.Main.modAffixData.name".I18N(),
                    TableInfo.DEFAULT_GRID_WIDTH / 2,
                    getData=>((ModAffixData)getData).Name),
                new TableInfo("ModEditor.Main.modAffixData.desc".I18N(),
                    TableInfo.DEFAULT_GRID_WIDTH * 3,
                    getData=>((ModAffixData)getData).Desc),
            },
            Project.GetAllAffix()
        ));

        Inspector.Refresh();
    }

    protected override object GetData(int index)
    {
        return Project.BuffData[index];
    }

    protected override int GetDataCount()
    {
        return Project.BuffData.Count;
    }
}
}