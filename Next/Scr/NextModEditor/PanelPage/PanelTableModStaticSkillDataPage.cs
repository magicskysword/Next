using System.Collections.Generic;
using System.Linq;
using FairyGUI;
using SkySwordKill.Next.Extension;
using SkySwordKill.Next.FGUI;
using SkySwordKill.Next.FGUI.Component;
using SkySwordKill.Next.FGUI.Dialog;
using SkySwordKill.NextFGUI.NextCore;
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
        ModDataTableDataList = new ModDataTableDataList<ModStaticSkillData>(Project.StaticSkillData);
        
        AddTableHeader(new TableInfo(
            "ID".I18NTodo(),
            TableInfo.DEFAULT_GRID_WIDTH,
            data =>
            {
                var skill = (ModStaticSkillData)data;
                return skill.Id.ToString();
            }));
        
        AddTableHeader(new TableInfo(
            "功法唯一ID".I18NTodo(),
            TableInfo.DEFAULT_GRID_WIDTH,
            data =>
            {
                var skill = (ModStaticSkillData)data;
                return $"{skill.SkillPkId.ToString()}(lv.{skill.SkillLv})";
            }));
        
        AddTableHeader(new TableInfo(
            "品阶".I18NTodo(),
            TableInfo.DEFAULT_GRID_WIDTH,
            data =>
            {
                var skill = (ModStaticSkillData)data;
                return ModUtils.GetSkillLevelName(skill.Quality, skill.Phase);
            }));

        AddTableHeader(new TableInfo(
            "名称".I18NTodo(),
            TableInfo.DEFAULT_GRID_WIDTH,
            data =>
            {
                var skill = (ModStaticSkillData)data;
                return skill.Name;
            }));
            
        AddTableHeader(new TableInfo(
            "描述".I18NTodo(),
            TableInfo.DEFAULT_GRID_WIDTH * 3,
            data =>
            {
                var skill = (ModStaticSkillData)data;
                return skill.Desc;
            }));
    }

    protected override void OnInspectItem(ModStaticSkillData data)
    {
        if (data == null)
        {
            return;
        }
        
        AddDrawer(new CtlIDPropertyDrawer(
            "ID".I18NTodo(),
            data,
            () => Project.StaticSkillData,
            theData =>
            {
                var curData = (ModStaticSkillData)theData;
                return OnGetDataName(curData);
            },
            (theData, newId) =>
            {
                theData.Id = newId;
                Project.StaticSkillData.ModSort();
                CurInspectIndex = Project.StaticSkillData.FindIndex(modData => modData == theData);
            },
            (theData, otherData) =>
            {
                (otherData.Id, theData.Id) = (theData.Id, otherData.Id);
                Project.StaticSkillData.ModSort();
                CurInspectIndex = Project.StaticSkillData.FindIndex(modData => modData == theData);
            },
            () => { Inspector.Refresh(); })
        );
        
        AddDrawer(new CtlStringPropertyDrawer(
                "名称".I18NTodo(),
                value => data.Name = value,
                () => data.Name
            )
        );
        
        AddDrawer(new CtlIntPropertyDrawer(
                "功法唯一ID".I18NTodo(),
                value => data.SkillPkId = value,
                () => data.SkillPkId
            )
        );

        var iconDrawer = new CtlIconPreviewDrawer(() => Mod.GetStaticSkillIconUrl(data));
        AddDrawer(new CtlIntPropertyDrawer(
                "图标".I18NTodo(),
                value => data.Icon = value,
                () => data.Icon).AddChangeListener(Inspector.Refresh)
        ).AddChainDrawer(iconDrawer);
            
        AddDrawer(iconDrawer);
        
        AddDrawer(new CtlIntPropertyDrawer(
                "功法等级".I18NTodo(),
                value => data.SkillLv = value,
                () => data.SkillLv
            )
        );
            
        AddDrawer(new CtlDropdownPropertyDrawer(
                "功法品阶".I18NTodo(),
                () => ModEditorManager.I.SkillDataQuality.Select(s => $"{s.Id} : {s.Desc}"),
                index => data.Quality = ModEditorManager.I.SkillDataQuality[index].Id,
                () => ModEditorManager.I.SkillDataQuality.FindIndex(s => s.Id == data.Quality)
            )
        );
            
        AddDrawer(new CtlDropdownPropertyDrawer(
                "功法品质".I18NTodo(),
                () => ModEditorManager.I.SkillDataPhase.Select(s => $"{s.Id} : {s.Desc}"),
                index => data.Phase = ModEditorManager.I.SkillDataPhase[index].Id,
                () => ModEditorManager.I.SkillDataPhase.FindIndex(s => s.Id == data.Phase)
            )
        );
        
        AddDrawer(new CtlIntPropertyDrawer(
                "参悟月数".I18NTodo(),
                value => data.LearnCostMonth = value,
                () => data.LearnCostMonth
            )
        );
        
        AddDrawer(new CtlIntPropertyDrawer(
                "修炼速度".I18NTodo(),
                value => data.TrainingSpeed = value,
                () => data.TrainingSpeed
            )
        );
        
        AddDrawer(new CtlIntBindTablePropertyDrawer(
                "类型".I18NTodo(),
                value => data.AttackType = value,
                () => data.AttackType,
                value =>
                {
                    var type = ModEditorManager.I.StaticSkillTypes.Find(type => type.Id == value);
                    return $"【{type.Id}】{type.Desc}";
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
                () => new List<IModData>(ModEditorManager.I.StaticSkillTypes)
            )
        );
        
        AddDrawer(new CtlSeidDataPropertyDrawer(
            "特性",
            data.Id, 
            data.SeidList,
            Mod,
            Project.StaticSkillSeidDataGroup,
            ModEditorManager.I.StaticSkillSeidMetas,
            seidId =>
            {
                if (ModEditorManager.I.StaticSkillSeidMetas.TryGetValue(seidId, out var seidData))
                    return $"{seidId}  {seidData.Name}";
                return $"{seidId}  ???";
            })
        );
        
        AddDrawer(new CtlCheckboxPropertyDrawer(
                "斗法可用".I18NTodo(),
                value => data.Battle = value ? 1 : 0,
                () => data.Battle == 1
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
            "图鉴类型".I18NTodo(),
            () => ModEditorManager.I.GetStaticSkillGuideTypes().Select(type => $"{type.Id} : {type.Desc}"),
            index => data.GuideType = ModEditorManager.I.GetStaticSkillGuideTypes()[index].Id,
            () => ModEditorManager.I.GetStaticSkillGuideTypes().GetIndex(data.GuideType)
        ));
        
        AddDrawer(new CtlDropdownPropertyDrawer(
                "请教类型".I18NTodo(),
                () => ModEditorManager.I.SkillDataConsultTypes.Select(type => $"{type.Id} : {type.Desc}"),
                index => data.ConsultType = ModEditorManager.I.SkillDataConsultTypes[index].Id,
                () => ModEditorManager.I.SkillDataConsultTypes.FindIndex(type => type.Id == data.ConsultType)
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

    public override string OnGetDataName(ModStaticSkillData data)
    {
        return $"{data.Id} {data.Name}";
    }
    
    protected override void OnBuildCustomPopupMenu(PopupMenu menu, ModStaticSkillData[] modDataArray)
    {
        menu.AddSeperator();
        menu.AddItem("生成神通组".I18NTodo(), () => OnGenerateStaticSkillGroup(modDataArray[0]))
            .enabled = Editable && modDataArray?.Length == 1;
        menu.AddItem("生成神通书籍".I18NTodo(), () => OnGenerateStaticSkillBook(modDataArray[0]))
            .enabled = Editable && modDataArray?.Length == 1;
        menu.AddSeperator();
    }
    
    private void OnGenerateStaticSkillBook(ModStaticSkillData modData)
    {
        if(!Editable)
            return;
            
        if(modData == null)
            return;

        var itemId = modData.SkillPkId + 3000;
        if (Project.ItemData.HasId(itemId))
        {
            WindowConfirmDialog.CreateDialog("提示", $"ID为{itemId}的物品已经存在！", false);
            return;
        }
            
        WindowConfirmDialog.CreateDialog("提示", $"是否创建ID为{itemId}的功法书籍？",true, 
            () =>
            {
                var itemData = new ModItemData();
                itemData.Id = itemId;
                itemData.Icon = 3001;
                itemData.Name = modData.Name;
                itemData.Info = modData.SkillPkId.ToString();
                itemData.Desc = modData.Desc;
                itemData.Quality = modData.Quality;
                itemData.Phase = modData.Phase;
                itemData.ItemType = 4;
                itemData.MaxStack = 1;
                itemData.ShopType = 99;
                itemData.StudyCostTime = ((modData.Quality - 1) * 3 + modData.Phase) * 12;
                itemData.SpecialType = 1;
                itemData.SeidList.Add(2);
                Project.ItemData.Add(itemData);
            
                var seid = Project.ItemUseSeidDataGroup.GetOrCreateSeid(itemData.Id, 2);
                seid.SetValue("value1", modData.SkillPkId);
            });
    }

    private void OnGenerateStaticSkillGroup(ModStaticSkillData modData)
    {
        if(!Editable)
            return;
            
        if(modData == null)
            return;
            
        if (modData.SkillLv != 1)
        {
            WindowConfirmDialog.CreateDialog("提示", "只有功法等级为1的神通才能生成功法组".I18NTodo(), false);
            return;
        }

        for (int i = 1; i < 5; i++)
        {
            var id = modData.Id + i;
            if (ModDataTableDataList.HasId(id))
            {
                WindowConfirmDialog.CreateDialog("提示", 
                    $"已经存在id为{id}的功法，请确保从{modData.Id} ~ {modData.Id + 4}范围没有相应功法。".I18NTodo(), false);
                return;
            }
        }
            
        WindowConfirmDialog.CreateDialog("提示", 
            $"是否生成从{modData.Id} ~ {modData.Id + 4}的功法组？".I18NTodo(), true, () =>
            {
                var copyData = GetCopyData(modData);
                if (!modData.Name.EndsWith("1"))
                {
                    modData.Name += "1";
                }
                var sequenceCommand = new SequenceCommand();
                for (int i = 1; i < 5; i++)
                {
                    var id = modData.Id + i;
                    var tagLv = i + 1;
                    sequenceCommand.AddCommand(new AddDataUndoCommand(
                        () =>
                        {
                            var tagData = OnPasteData(copyData, id);
                            tagData.SkillLv = tagLv;
                            tagData.Name = modData.Name.Substring(0, modData.Name.Length - 1) + $"{tagLv}";
                            return tagData;
                        },
                        data => AddData((ModStaticSkillData)data),
                        data => RemoveData(data.Id)));
                }
                this.Record(sequenceCommand);
                Refresh();
            });
    }

    protected override ModStaticSkillData OnPasteData(CopyData copyData, int targetId)
    {
        var oldId = copyData.Data.Id;
        var json = copyData.Data.GetJsonData();
        var skillData = json.ToObject<ModStaticSkillData>();
        skillData.Id = targetId;
        Project.StaticSkillData.Add(skillData);
        Project.StaticSkillSeidDataGroup.CopyAllSeid(copyData.Project.StaticSkillSeidDataGroup, oldId, targetId);
        Project.StaticSkillData.ModSort();
        return skillData;
    }
}