using System.Collections.Generic;
using System.Linq;
using FairyGUI;
using Fungus;
using SkySwordKill.Next;
using SkySwordKill.Next.Extension;
using SkySwordKill.Next.FGUI;
using SkySwordKill.Next.FGUI.Component;
using SkySwordKill.Next.FGUI.Dialog;
using SkySwordKill.NextFGUI.NextCore;
using SkySwordKill.NextModEditor.Mod;
using SkySwordKill.NextModEditor.Mod.Data;

namespace SkySwordKill.NextModEditor.PanelPage;

public class PanelTableModItemDataPage : PanelTablePageBase<ModItemData>
{
    public PanelTableModItemDataPage(string name, ModWorkshop mod, ModProject project) : base(name, mod, project)
    {
    }
        
    public override ModDataTableDataList<ModItemData> ModDataTableDataList { get; set; }

    protected override void OnInit()
    {
        ModDataTableDataList = new ModDataTableDataList<ModItemData>(Project.ItemData);

        AddTableHeader(new TableInfo(
            "ID".I18NTodo(),
            TableInfo.DEFAULT_GRID_WIDTH,
            data =>
            {
                var buff = (ModItemData)data;
                return buff.Id.ToString();
            }));

        AddTableHeader(new TableInfo(
            "名称".I18NTodo(),
            TableInfo.DEFAULT_GRID_WIDTH,
            data =>
            {
                var item = (ModItemData)data;
                return item.Name;
            }));

        AddTableHeader(new TableInfo(
            "类型".I18NTodo(),
            TableInfo.DEFAULT_GRID_WIDTH,
            data =>
            {
                var item = (ModItemData)data;
                var index = ModEditorManager.I.ItemDataTypes.FindIndex(tData => tData.Id == item.ItemType);
                if (index >= 0)
                {
                    var itemType = ModEditorManager.I.ItemDataTypes[index];
                    return $"{itemType.Id} : {itemType.Desc}";
                }

                return $"{item.ItemType} : 未知";
            }));

        AddTableHeader(new TableInfo(
            "说明".I18NTodo(),
            TableInfo.DEFAULT_GRID_WIDTH * 3,
            data =>
            {
                var item = (ModItemData)data;
                return Mod.GetItemInfo(item);
            }));

        AddTableHeader(new TableInfo(
            "描述".I18NTodo(),
            TableInfo.DEFAULT_GRID_WIDTH * 3,
            data =>
            {
                var item = (ModItemData)data;
                return item.Desc;
            }));
    }

    protected override void OnInspectItem(ModItemData data)
    {
        if (data == null)
        {
            return;
        }
            
        AddDrawer(new CtlIDPropertyDrawer(
            "ID".I18NTodo(),
            data,
            () => Project.ItemData,
            theData =>
            {
                var curData = (ModItemData)theData;
                return OnGetDataName(curData);
            },
            (theData, newId) =>
            {
                Project.ItemEquipSeidDataGroup.ChangeSeidID(theData.Id, newId);
                Project.ItemUseSeidDataGroup.ChangeSeidID(theData.Id, newId);
                theData.Id = newId;
                Project.ItemData.ModSort();
                CurInspectIndex = Project.ItemData.FindIndex(modData => modData == theData);
            },
            (theData, otherData) =>
            {
                Project.ItemEquipSeidDataGroup.SwiftSeidID(theData.Id, otherData.Id);
                Project.ItemUseSeidDataGroup.SwiftSeidID(theData.Id, otherData.Id);
                (otherData.Id, theData.Id) = (theData.Id, otherData.Id);
                Project.ItemData.ModSort();
                CurInspectIndex = Project.ItemData.FindIndex(modData => modData == theData);
            },
            () => { Inspector.Refresh(); })
        );
            
        AddDrawer(new CtlStringPropertyDrawer(
            "名称".I18NTodo(),
            str => data.Name = str,
            () => data.Name)
        );
            
        var iconDrawer = new CtlIconPreviewDrawer(() => Mod.GetItemIconUrl(data));
        AddDrawer(new CtlIntPropertyDrawer(
                "图标".I18NTodo(),
                value => data.Icon = value,
                () => data.Icon).AddChangeListener(Inspector.Refresh)
        ).AddChainDrawer(iconDrawer);
            
        AddDrawer(iconDrawer);
            
        AddDrawer(new CtlDropdownPropertyDrawer(
                "物品类型".I18NTodo(),
                () => ModEditorManager.I.ItemDataTypes.Select(type => $"{type.Id} : {type.Desc}"),
                index => data.ItemType = ModEditorManager.I.ItemDataTypes[index].Id,
                () => ModEditorManager.I.ItemDataTypes.FindIndex(type =>
                    type.Id == data.ItemType)
            )
        ).AddChangeListener(ReloadInspector);
            
        AddDrawer(new CtlIntPropertyDrawer(
            "堆叠上限".I18NTodo(),
            value => data.MaxStack = value,
            () => data.MaxStack)
        );
            
        AddDrawer(new CtlGroupDrawer("法宝", data.ItemType == 0 || 
                                           data.ItemType == 1 || 
                                           data.ItemType == 2,
                new CtlStringBindDataPropertyDrawer(
                    "武器外形",
                    value => data.ArtifactType = value,
                    () => data.ArtifactType,
                    value => ModEditorManager.I.ItemDataArtifactTypeGroup.GetDescription(value),
                    value =>
                    {
                    
                    }).AddChangeListener(Inspector.Refresh)
                ,
        new CtlSeidDataPropertyDrawer(
                    "装备特性",
                    data.Id,
                    data.SeidList,
                    Mod,
                    Project.ItemEquipSeidDataGroup,
                    ModEditorManager.I.ItemEquipSeidMetas,
                    seidId =>
                    {
                        if (ModEditorManager.I.ItemEquipSeidMetas.TryGetValue(seidId, out var seidData))
                            return $"{seidId} {seidData.Name}";
                        return $"{seidId} ???";
                    }).AddChangeListener(ReloadInspector)
            )
        );

        AddDrawer(new CtlGroupDrawer("领悟书籍", data.ItemType == 3 ||
                                             data.ItemType == 4 ||
                                             data.ItemType == 13,
                new CtlIntPropertyDrawer(
                    "领悟时间".I18NTodo(),
                    value => data.StudyCostTime = value,
                    () => data.StudyCostTime
                ),
                new CtlIntArrayBindTableExPropertyDrawer(
                    "领悟前置条件".I18NTodo(),
                    (comList, fromTable) =>
                    {
                        if (fromTable)
                        {
                            var dict = new Dictionary<int, int>();
                            foreach (var compression in comList)
                            {
                                dict[compression] = 0;
                            }

                            for (var index = 0; index < data.StudyRequirement.Count; index += 2)
                            {
                                var id = data.StudyRequirement[index];
                                if (index + 1 < data.StudyRequirement.Count && dict.ContainsKey(id))
                                    dict[id] = data.StudyRequirement[index + 1];
                            }

                            var list = new List<int>();
                            foreach (var pair in dict)
                            {
                                list.Add(pair.Key);
                                list.Add(pair.Value);
                            }
                            
                            data.StudyRequirement = list;
                        }
                        else
                        {
                            data.StudyRequirement = comList.ToList();
                        }
                    },
                    (fromTable) =>
                    {
                        if (fromTable)
                        {
                            return data.StudyRequirement.Where((_, index) => index % 2 == 0).ToList();
                        }
                        else
                        {
                            return data.StudyRequirement;
                        }
                    },
                    list => Mod.GetComprehensionWithPhaseDesc(list),
                    new List<TableInfo>()
                    {
                        new TableInfo(
                            "ID".I18NTodo(),
                            TableInfo.DEFAULT_GRID_WIDTH / 2,
                            obj =>
                            {
                                var item = (ModComprehensionData)obj;
                                return item.Id.ToString();
                            }),
                        new TableInfo(
                            "名称".I18NTodo(),
                            TableInfo.DEFAULT_GRID_WIDTH / 2,
                            obj =>
                            {
                                var item = (ModComprehensionData)obj;
                                return item.Name;
                            }),
                        new TableInfo(
                            "描述".I18NTodo(),
                            TableInfo.DEFAULT_GRID_WIDTH,
                            obj =>
                            {
                                var item = (ModComprehensionData)obj;
                                return item.Desc;
                            }),
                    },
                    () => new List<IModData>(Mod.GetAllComprehensionData())
                ).AddChangeListener(Inspector.Refresh)
            )
        );

        var forgeElementList = Mod.GetAllForgeElementData();
        var forgePropertyList = Mod.GetAllForgePropertyData();
        AddDrawer(new CtlGroupDrawer("材料", data.ItemType == 8,
                new CtlDropdownPropertyDrawer("五维类别",
                    () => forgeElementList
                        .Select(type => $"{type.Id} : {type.Desc}"),
                    index => data.ForgeElementType = forgeElementList[index].Id,
                    () => forgeElementList.GetIndex(data.ForgeElementType)
                ),
                new CtlDropdownPropertyDrawer("属性类别",
                    () => forgePropertyList
                        .Select(type => $"{type.Id} : {type.Desc}"),
                    index => data.ForgePropertyType = forgePropertyList[index].Id,
                    () => forgePropertyList.GetIndex(data.ForgePropertyType)
                )
            )
        );
            
        AddDrawer(new CtlGroupDrawer("丹药", data.ItemType == 5,
                new CtlIntPropertyDrawer(
                    "丹毒".I18NTodo(),
                    value => data.DrugPoison = value,
                    () => data.DrugPoison
                ),
                new CtlIntPropertyDrawer(
                    "可用次数".I18NTodo(), 
                    value => data.CanUseCount = value, 
                    () => data.CanUseCount),
                new CtlCheckboxPropertyDrawer(
                    "NPC可使用".I18NTodo(),
                    value => data.NpcCanUse = value ? 1 : 0,
                    () => data.NpcCanUse == 1
                )
            )
        );
            
        var customTypeList = new List<int>()
        {
            3,4,5,6,7,10,12,13,15,16
        };
            
        AddDrawer(new CtlGroupDrawer("消耗品", customTypeList.Contains(data.ItemType),
                new CtlDropdownPropertyDrawer(
                    "使用类型".I18NTodo(),
                    () => ModEditorManager.I.ItemDataUseTypes.Select(type => $"{type.Id} : {type.Desc}"),
                    index => data.SpecialType = ModEditorManager.I.ItemDataUseTypes[index].Id,
                    () => ModEditorManager.I.ItemDataUseTypes.GetIndex(data.SpecialType)
                ),
                new CtlSeidDataPropertyDrawer(
                    "消耗品特性",
                    data.Id,
                    data.SeidList,
                    Mod,
                    Project.ItemUseSeidDataGroup,
                    ModEditorManager.I.ItemUseSeidMetas,
                    seidId =>
                    {
                        if (ModEditorManager.I.ItemUseSeidMetas.TryGetValue(seidId, out var seidData))
                            return $"{seidId} {seidData.Name}";
                        return $"{seidId} ???";
                    }
                ).AddChangeListener(ReloadInspector)
            )
        );
            
            
        var alchemyTableInfoList = new List<TableInfo>()
        {
            new TableInfo("ID".I18NTodo(),
                TableInfo.DEFAULT_GRID_WIDTH,
                getData => ((ModAlchemyElementData)getData).Id.ToString()),
            new TableInfo("名称".I18NTodo(),
                TableInfo.DEFAULT_GRID_WIDTH,
                getData => ((ModAlchemyElementData)getData).Name),
            new TableInfo("描述".I18NTodo(),
                TableInfo.DEFAULT_GRID_WIDTH * 2,
                getData => ((ModAlchemyElementData)getData).Desc),
        };
            
        AddDrawer(new CtlGroupDrawer("药材", data.ItemType == 6,
                new CtlIntBindTablePropertyDrawer(
                    "药引".I18NTodo(),
                    value => data.AlchemyGuiding = value,
                    () => data.AlchemyGuiding,
                    id => Mod.GetAlchemyElementDesc(id),
                    alchemyTableInfoList,
                    () => new List<IModData>(Mod.GetAllAlchemyElementData())
                ),
                new CtlIntBindTablePropertyDrawer(
                    "主药".I18NTodo(),
                    value => data.AlchemyMain = value,
                    () => data.AlchemyMain,
                    id => Mod.GetAlchemyElementDesc(id),
                    alchemyTableInfoList,
                    () => new List<IModData>(Mod.GetAllAlchemyElementData())
                ),
                new CtlIntBindTablePropertyDrawer(
                    "辅药".I18NTodo(),
                    value => data.AlchemySub = value,
                    () => data.AlchemySub,
                    id => Mod.GetAlchemyElementDesc(id),
                    alchemyTableInfoList,
                    () => new List<IModData>(Mod.GetAllAlchemyElementData())
                )
            )
        );

        var groupOther = AddDrawer(new CtlGroupDrawer("其他", true, 
            new CtlDropdownPropertyDrawer(
                "品阶".I18NTodo(),
                () => ModEditorManager.I.ItemDataQualityTypes.Select(type => $"{type.Id} : {type.Desc}"),
                index => data.Quality = ModEditorManager.I.ItemDataQualityTypes[index].Id,
                () => ModEditorManager.I.ItemDataQualityTypes.GetIndex(data.Quality)
            ),
            new CtlDropdownPropertyDrawer(
                "功法/武器品质".I18NTodo(),
                () => ModEditorManager.I.ItemDataPhaseTypes.Select(type => $"{type.Id} : {type.Desc}"),
                index => data.Phase = ModEditorManager.I.ItemDataPhaseTypes[index].Id,
                () => ModEditorManager.I.ItemDataPhaseTypes.GetIndex(data.Phase)
            ),
            new CtlDropdownPropertyDrawer(
                "图鉴类型".I18NTodo(),
                () => ModEditorManager.I.GetItemGuideTypes().Select(type => $"{type.Id} : {type.Desc}"),
                index => data.GuideType = ModEditorManager.I.GetItemGuideTypes()[index].Id,
                () => ModEditorManager.I.GetItemGuideTypes().GetIndex(data.GuideType)
            ),
            new CtlIntPropertyDrawer(
                "价格".I18NTodo(),
                value => data.Price = value,
                () => data.Price
            ),
            new CtlDropdownPropertyDrawer(
                "商店投放类型".I18NTodo(),
                () => ModEditorManager.I.ItemDataShopTypes.Select(type => $"{type.Id} : {type.Desc}"),
                index => data.ShopType = ModEditorManager.I.ItemDataShopTypes[index].Id,
                () => ModEditorManager.I.ItemDataShopTypes.GetIndex(data.ShopType)
            ),
            new CtlCheckboxPropertyDrawer(
                "不可出售".I18NTodo(),
                value => data.CanNotSale = value ? 1 : 0,
                () => data.CanNotSale == 1
            ),
            new CtlIntArrayBindTablePropertyDrawer(
                "物品标识".I18NTodo(),
                list => data.ItemFlagList = list,
                () => data.ItemFlagList,
                list => Mod.GetItemFlagDesc(list),
                new List<TableInfo>()
                {
                    new TableInfo("ID".I18NTodo(),
                        TableInfo.DEFAULT_GRID_WIDTH,
                        getData => ((ModItemFlagData)getData).Id.ToString()),
                    new TableInfo("名称".I18NTodo(),
                        TableInfo.DEFAULT_GRID_WIDTH * 1.5f,
                        getData => ((ModItemFlagData)getData).Name),
                },
                () => new List<IModData>(Mod.GetAllItemFlagData())
            ).AddChangeListener(Inspector.Refresh),
            new CtlIntArrayBindTablePropertyDrawer(
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
            ).AddChangeListener(Inspector.Refresh)
        ));

        if (data.ItemType == 3 || data.ItemType == 4)
        {
            groupOther.AddDrawer(new CtlStringBindDataPropertyDrawer(
                "功能描述".I18NTodo(),
                value => data.Info = value,
                () => data.Info,
                input =>
                {
                    if(float.TryParse(input, out var fId))
                    {
                        var id = (int)fId;
                        if (data.ItemType == 3)
                        {
                            var skillData = Mod.FindSkillBySkillPkId(id);
                            if (skillData != null)
                                return $"【{skillData.SkillPkId} {skillData.Name}】{skillData.Desc}";
                        }
                        else
                        {
                            var staticSkillData = Mod.FindStaticSkillBySkillPkId(id);
                            if (staticSkillData != null)
                                return $"【{staticSkillData.SkillPkId} {staticSkillData.Name}】{staticSkillData.Desc}";
                        }
                        
                        return $"【{id}】？？？";
                    }
                    
                    return $"【{input}】？？？";
                },
                input =>
                {
                    int id = -1;
                    if (double.TryParse(input, out var fId))
                        id = (int)fId;
                    
                    var title = data.ItemType == 3 ? "选择神通描述".I18NTodo() : "选择功法描述".I18NTodo();
                    var tableInfo = new List<TableInfo>()
                    {
                        new("ID".I18NTodo(),
                            TableInfo.DEFAULT_GRID_WIDTH,
                            getData => ((IModData)getData).Id.ToString()),
                    };
                    List<IModData> dataList = new List<IModData>();
                    if (data.ItemType == 3)
                    {
                        tableInfo.Add(new TableInfo("名称".I18NTodo(),
                                TableInfo.DEFAULT_GRID_WIDTH,
                                getData => ((ModSkillData)getData).Name.ToString()));
                        tableInfo.Add(new TableInfo("描述".I18NTodo(),
                            TableInfo.DEFAULT_GRID_WIDTH,
                            getData => ((ModSkillData)getData).Desc.ToString()));
                        dataList.AddRange(Mod.GetAllSkillData()
                            .GroupBy(skillData => skillData.SkillPkId)
                            .Select(d => d.OrderByDescending(skill => skill.SkillLv).First()));
                    }
                    else
                    {
                        tableInfo.Add(new TableInfo("名称".I18NTodo(),
                            TableInfo.DEFAULT_GRID_WIDTH,
                            getData => ((ModSkillData)getData).Name.ToString()));
                        tableInfo.Add(new TableInfo("描述".I18NTodo(),
                            TableInfo.DEFAULT_GRID_WIDTH,
                            getData => ((ModSkillData)getData).Desc.ToString()));
                        dataList.AddRange(Mod.GetAllStaticSkillData()
                            .GroupBy(skillData => skillData.SkillPkId)
                            .Select(d => d.OrderByDescending(skill => skill.SkillLv).First()));
                    }
                    
                    Main.LogInfo($"进行物品编辑：{data.Id}");
                    WindowTableSelectorDialog.CreateDialog(title, tableInfo, new []{ id }, true
                        ,dataList, false, ids =>
                        {
                            var id = ids.FirstOrDefault();
                            if (id == -1)
                                return;
                            
                            data.Info = id.ToString();
                            Inspector.Refresh();
                        });
                }
            ));
        }
        else
        {
            groupOther.AddDrawer(new CtlStringAreaPropertyDrawer(
                "功能描述".I18NTodo(),
                value => data.Info = value,
                () => data.Info
            ));
        }
        
        groupOther.AddDrawer(new CtlStringAreaPropertyDrawer(
            "物品简介".I18NTodo(),
            value => data.Desc = value,
            () => data.Desc
        ));
    }

    public override string OnGetDataName(ModItemData data)
    {
        return $"{data.Id} {data.Name}";
    }

    protected override void OnBuildCustomPopupMenu(PopupMenu menu, ModItemData[] modDataArray)
    {
        menu.AddSeperator();
        var canCreateBook = modDataArray?.All(modData => modData.ItemType == 3 || modData.ItemType == 4);
        menu.AddItem("生成请教书籍".I18NTodo(), () => OnGenerateSkillLearnBook(modDataArray))
            .enabled = Editable && canCreateBook == true;
        menu.AddSeperator();
    }

    private void OnGenerateSkillLearnBook(ModItemData[] modDataArray)
    {
        if(!Editable)
            return;

        if (modDataArray.Any(modData => modData.Id > 1000000000))
        {
            var conflictData = modDataArray.First(modData => modData.Id > 1000000000);
            WindowConfirmDialog.CreateDialog("提示", $"当前物品ID:{conflictData}超出10,0000,0000(十亿)，无法创建对应请教书籍", false);
            return;
        }
            
        if(modDataArray.Any(modData => modData.ItemType != 3 && modData.ItemType != 4))
        {
            var conflictData = modDataArray.First(modData => modData.ItemType != 3 && modData.ItemType != 4);
            WindowConfirmDialog.CreateDialog("提示", $"当前物品ID:{conflictData}类型不为神通或功法书籍，无法创建对应请教书籍", false);
            return;
        }
        
        if(modDataArray.Any(modData => ModDataTableDataList.HasId(modData.Id + 1000000000)))
        {
            var conflictData = modDataArray.First(modData => ModDataTableDataList.HasId(modData.Id + 1000000000));
            WindowConfirmDialog.CreateDialog("提示", $"当前物品ID:{conflictData.Id}对应的请教书籍已存在，无法创建对应请教书籍", false);
            return;
        }
            
        WindowConfirmDialog.CreateDialog("提示", $"是否创建ID为[{string.Join(",", modDataArray.Select(modData => $"({modData.Id}){modData.Name}"))}]的请教书籍？",true, 
            () =>
            {
                var sequenceCommand = new SequenceCommand();
                foreach (var modData in modDataArray)
                {
                    var copyData = GetCopyData(modData);
                    var bookId = modData.Id + 1000000000;
                    sequenceCommand.AddCommand(new AddDataUndoCommand(
                        () =>
                        {
                            var data = OnPasteData(copyData, bookId);
                            data.ShopType = 99;
                            data.ItemFlagList.Clear();
                            data.Price = 0;
                            return data;
                        },
                        data => AddData((ModItemData)data),
                        data => RemoveData(data.Id)));
                }
                this.Record(sequenceCommand);
                Refresh();
            });
    }

    protected override ModItemData OnPasteData(CopyData copyData, int targetId)
    {
        var oldId = copyData.Data.Id;
        var json = copyData.Data.GetJsonData();
        var itemData = json.ToObject<ModItemData>();
        itemData.Id = targetId;
        Project.ItemData.Add(itemData);
        Project.ItemEquipSeidDataGroup.CopyAllSeid(copyData.Project.ItemEquipSeidDataGroup, oldId, targetId);
        Project.ItemUseSeidDataGroup.CopyAllSeid(copyData.Project.ItemUseSeidDataGroup, oldId, targetId);
        Project.ItemData.ModSort();
        return itemData;
    }
}