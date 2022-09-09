using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FairyGUI;
using SkySwordKill.Next;
using SkySwordKill.Next.FGUI.Dialog;
using SkySwordKill.Next.FGUI;
using SkySwordKill.Next.FGUI.Component;
using SkySwordKill.NextEditor.Mod;
using SkySwordKill.NextFGUI.NextCore;
using SkySwordKill.NextModEditor.Mod.Data;

namespace SkySwordKill.NextEditor.Panel
{
    public class WindowSeidEditorDialog : WindowDialogBase
    {
        public class SeidNodeInfo
        {
            public bool IsSeid;
            public string NodeName;
            public string NodeIcon;
            public int SeidID;
        }
        
        private WindowSeidEditorDialog() 
            : base("NextCore", "WinSeidEditorDialog")
        {
            
        }

        public string Title { get; set; }
        public ModWorkshop Mod { get;private set; }
        public int OwnerId { get;private set; }
        public Dictionary<int, ModSeidMeta> SeidMetas { get;private set; }
        public IModSeidDataGroup SeidGroup { get;private set; }
        public List<int> SeidList { get;private set; }
        public List<int> DisableSeidList { get;private set; }
        public Action OnClose { get;private set; }
        public bool Editable { 
            get => _editable;
            set
            {
                _editable = value;
                Inspector.Editable = value;
                Inspector.Refresh();
                RefreshButtonState();
            }
        }

        public UI_WinSeidEditorDialog SeidEditor => contentPane as UI_WinSeidEditorDialog;
        public CtlPropertyInspector Inspector { get; set; }
        public GButton BtnAdd => SeidEditor.m_btnAdd;
        public GButton BtnRemove => SeidEditor.m_btnRemove;
        public GButton BtnEnable => SeidEditor.m_btnEnable;
        public GButton BtnDisable => SeidEditor.m_btnDisable;
        public GButton BtnMoveUp => SeidEditor.m_btnMoveUp;
        public GButton BtnMoveDown => SeidEditor.m_btnMoveDown;
        
        private int? CurrentSeidId { get; set; }
        
        private List<GTreeNode> _seidNodeList = new List<GTreeNode>();
        private bool _editable;

        public static WindowSeidEditorDialog CreateDialog(string title,ModWorkshop mod,int ownerId, IModSeidDataGroup seidGroup,Dictionary<int, ModSeidMeta> seidMetas,
            List<int> seidList, Action onClose)
        {
            var window = new WindowSeidEditorDialog();

            window.Title = title;
            window.Mod = mod;
            window.OwnerId = ownerId;
            window.SeidGroup = seidGroup;
            window.SeidMetas = seidMetas;
            window.SeidList = seidList;
            window.DisableSeidList = new List<int>();
            window.OnClose = onClose;

            window.Show();
            
            return window;
        }

        protected override void OnInit()
        {
            base.OnInit();

            SeidEditor.m_frame.title = Title;
            SeidEditor.m_frame.m_closeButton.onClick.Add(Hide);
            
            Inspector = new CtlPropertyInspector(SeidEditor.m_inspector);

            foreach (var pair in SeidGroup.DataGroups)
            {
                var seidId = pair.Key;
                var seid = SeidGroup.GetSeid(OwnerId, seidId);
                if (seid != null && !SeidList.Contains(pair.Key))
                {
                    DisableSeidList.Add(pair.Key);
                }
            }
            
            SeidEditor.m_list.treeNodeRender = OnTreeNodeRender;
            SeidEditor.m_list.onClickItem.Add(OnClickSeidItem);
            
            BtnAdd.onClick.Add(OnClickAdd);
            BtnRemove.onClick.Add(OnClickRemove);
            BtnEnable.onClick.Add(OnClickEnable);
            BtnDisable.onClick.Add(OnClickDisable);
            BtnMoveUp.onClick.Add(OnClickMoveUp);
            BtnMoveDown.onClick.Add(OnClickMoveDown);
            
            Refresh();
            UnselectTargetSeid();
        }

        private void Refresh()
        {
            SeidEditor.m_list.rootNode.RemoveChildren();
            _seidNodeList.Clear();
            AddSeidList("已启用特性", SeidList, _seidNodeList);
            AddSeidList("已禁用特性", DisableSeidList, _seidNodeList);
        }

        private bool GetSelectedSeid(out int seidId)
        {
            var node = SeidEditor.m_list.GetSelectedNode();
            seidId = -1;

            // 非Seid节点不能删除
            if (!(node?.data is SeidNodeInfo nodeInfo) || !nodeInfo.IsSeid)
            {
                
                return false;
            }

            seidId = nodeInfo.SeidID;
            return true;
        }

        private void SelectSeid(int seidId)
        {
            foreach (var node in _seidNodeList)
            {
                if(node.data is SeidNodeInfo nodeInfo && nodeInfo.IsSeid && nodeInfo.SeidID == seidId)
                {
                    SeidEditor.m_list.SelectNode(node);
                    SetTargetSeid(seidId);
                    return;
                }
            }
            UnselectTargetSeid();
        }

        private void OnClickAdd(EventContext context)
        {
            var metas = SeidMetas.Select(pair => (IModData)pair.Value).ToList();
            metas.ModSort();

            WindowTableSelectorDialog.CreateDialog("创建特性",
                new List<TableInfo>()
                {
                    new TableInfo("ID", TableInfo.DEFAULT_GRID_WIDTH * 0.4f, obj => ((ModSeidMeta)obj).Id.ToString()),
                    new TableInfo("特性名称", TableInfo.DEFAULT_GRID_WIDTH * 1.8f, obj => ((ModSeidMeta)obj).Name),
                    new TableInfo("特性描述", TableInfo.DEFAULT_GRID_WIDTH * 2f, obj => ((ModSeidMeta)obj).Desc),
                },
                null,
                false,
                metas,
                false,
                ids =>
                {
                    var seidId = ids[0];
                    AddSeid(seidId);
                    SelectSeid(seidId);
                });
        }

        private void OnClickRemove(EventContext context)
        {
            if (GetSelectedSeid(out var seidId))
                RemoveSeid(seidId);
        }

        private void OnClickDisable(EventContext context)
        {
            if (GetSelectedSeid(out var seidId))
            {
                DisableSeid(seidId);
                SelectSeid(seidId);
            }
        }

        private void OnClickEnable(EventContext context)
        {
            if (GetSelectedSeid(out var seidId))
            {
                EnableSeid(seidId);
                SelectSeid(seidId);
            }
        }
        
        private void OnClickMoveUp(EventContext context)
        {
            if (GetSelectedSeid(out var seidId))
            {
                SeidMoveUp(seidId);
                SelectSeid(seidId);
            }
        }

        private void OnClickMoveDown(EventContext context)
        {
            if (GetSelectedSeid(out var seidId))
            {
                SeidMoveDown(seidId);
                SelectSeid(seidId);
            }
        }

        private void AddSeid(int seidId)
        {
            if(SeidList.Contains(seidId) || DisableSeidList.Contains(seidId))
            {
                WindowConfirmDialog.CreateDialog("提示", "该特性已存在！", false);
                return;
            }
            SeidList.Add(seidId);
            SeidGroup.GetOrCreateSeid(OwnerId, seidId);
            Refresh();
        }
        
        private void RemoveSeid(int seidId)
        {
            if (SeidList.Contains(seidId))
            {
                SeidList.Remove(seidId);
            }
            else if(DisableSeidList.Contains(seidId))
            {
                DisableSeidList.Remove(seidId);
            }
            SeidGroup.RemoveSeid(OwnerId, seidId);
            Refresh();
        }
        
        private void EnableSeid(int seidId)
        {
            if(DisableSeidList.Contains(seidId))
            {
                DisableSeidList.Remove(seidId);
                if(!SeidList.Contains(seidId))
                    SeidList.Add(seidId);
            }
            Refresh();
        }
        
        private void DisableSeid(int seidId)
        {
            if(SeidList.Contains(seidId))
            {
                SeidList.Remove(seidId);
                if(!DisableSeidList.Contains(seidId))
                    DisableSeidList.Add(seidId);
            }
            Refresh();
        }
        
        private void SeidMoveUp(int seidId)
        {
            List<int> list;
            if (SeidList.Contains(seidId))
            {
                list = SeidList;
            }
            else if(DisableSeidList.Contains(seidId))
            {
                list = DisableSeidList;
            }
            else
            {
                return;
            }
            var index = list.IndexOf(seidId);
            if(index == 0)
                return;
            list.RemoveAt(index);
            list.Insert(index - 1, seidId);
            Refresh();
        }
        
        private void SeidMoveDown(int seidId)
        {
            List<int> list;
            if (SeidList.Contains(seidId))
            {
                list = SeidList;
            }
            else if(DisableSeidList.Contains(seidId))
            {
                list = DisableSeidList;
            }
            else
            {
                return;
            }
            var index = list.IndexOf(seidId);
            if(index == list.Count - 1)
                return;
            list.RemoveAt(index);
            list.Insert(index + 1, seidId);
            Refresh();
        }

        protected override void OnHide()
        {
            OnClose();
            base.OnHide();
        }

        private void OnClickSeidItem(EventContext context)
        {
            var item = context.data as GObject;
            var node = item?.treeNode;

            
            if (node.data is SeidNodeInfo nodeInfo && nodeInfo.IsSeid)
            {
                SetTargetSeid(nodeInfo.SeidID);
            }
            else
            {
                UnselectTargetSeid();
            }
        }

        private void RefreshButtonState()
        {
            if (Editable)
            {
                BtnAdd.enabled = true;
                if (CurrentSeidId != null)
                {
                    BtnRemove.enabled = true;
                    BtnMoveUp.enabled = true;
                    BtnMoveDown.enabled = true;

                    var isEnable = IsSeidEnable(CurrentSeidId.Value);
            
                    BtnEnable.enabled = !isEnable;
                    BtnDisable.enabled = isEnable;
                }
                else
                {
                    BtnRemove.enabled = false;
                    BtnMoveUp.enabled = false;
                    BtnMoveDown.enabled = false;
                    BtnEnable.enabled = false;
                    BtnDisable.enabled = false;
                }
            }
            else
            {
                BtnAdd.enabled = false;
                BtnRemove.enabled = false;
                BtnMoveUp.enabled = false;
                BtnMoveDown.enabled = false;
                BtnEnable.enabled = false;
                BtnDisable.enabled = false;
            }
        }

        private void UnselectTargetSeid()
        {
            Inspector.Clear();
            CurrentSeidId = null;
            RefreshButtonState();
        }

        private void SetTargetSeid(int seidId)
        {
            Inspector.Clear();
            CurrentSeidId = seidId;
            RefreshButtonState();

            if(SeidMetas.TryGetValue(seidId,out var seidMeta))
            {
                Inspector.AddDrawer(new CtlTitleDrawer(seidMeta.Name));
                Inspector.AddDrawer(new CtlTextDrawer(seidMeta.Desc));

                var seidData = SeidGroup.GetOrCreateSeid(OwnerId, seidId);
                foreach (var seidProperty in seidMeta.Properties)
                {
                    switch (seidProperty.Type)
                    {
                        case ModSeidPropertyType.Int:
                        {
                            CreateIntDrawer(seidProperty, seidData);
                            break;
                        }
                        case ModSeidPropertyType.IntArray:
                        {
                            CreateIntArrayDrawer(seidProperty, seidData);
                            break;
                        }
                        case ModSeidPropertyType.Float:
                        {
                            CreateFloatDrawer(seidProperty, seidData);
                            break;
                        }
                        case ModSeidPropertyType.String:
                        {
                            CreateStringDrawer(seidProperty, seidData);
                            break;
                        }
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            
            Inspector.Refresh();
        }

        private void CreateIntDrawer(ModSeidProperty seidProperty, ModSeidData seidData)
        {
            CtlPropertyDrawerBase drawer;
            var sInt = seidData.GetToken<ModSInt>(seidProperty.ID);
            if (seidProperty.SpecialDrawer.Contains("BuffDrawer"))
            {
                var intPropertyDrawer = new CtlIntBindTablePropertyDrawer(seidProperty.Desc,
                    value => sInt.Value = value,
                    () => sInt.Value,
                    buff =>
                    {
                        var buffData = Mod.FindBuff(buff);
                        if (buffData != null)
                        {
                            return $"【{buff} {buffData.Name}】{buffData.Desc}";
                        }

                        return $"【{buff}  ？】";
                    },
                    new List<TableInfo>()
                    {
                        new TableInfo("ID",
                            TableInfo.DEFAULT_GRID_WIDTH,
                            getData => ((ModBuffData)getData).Id.ToString()),
                        new TableInfo("名称",
                            TableInfo.DEFAULT_GRID_WIDTH,
                            getData => ((ModBuffData)getData).Name),
                        new TableInfo("描述",
                            TableInfo.DEFAULT_GRID_WIDTH * 2,
                            getData => ((ModBuffData)getData).Desc),
                    },
                    () => new List<IModData>(Mod.GetAllBuffData(true)));
                drawer = intPropertyDrawer;
            }
            else if (seidProperty.SpecialDrawer.Contains("SkillDrawer"))
            {
                var intPropertyDrawer = new CtlIntBindTablePropertyDrawer(seidProperty.Desc,
                    value => sInt.Value = value,
                    () => sInt.Value,
                    value =>
                    {
                        var skillData = Mod.FindSkillBySkillId(value);
                        if (skillData != null)
                        {
                            return $"【{value} {skillData.Name}】{skillData.Desc}";
                        }

                        return $"【{value}  ？】";
                    },
                    new List<TableInfo>()
                    {
                        new TableInfo("ID",
                            TableInfo.DEFAULT_GRID_WIDTH,
                            getData => ((ModSkillData)getData).Id.ToString()),
                        new TableInfo("神通ID",
                            TableInfo.DEFAULT_GRID_WIDTH,
                            getData => ((ModSkillData)getData).SkillId.ToString()),
                        new TableInfo("名称",
                            TableInfo.DEFAULT_GRID_WIDTH,
                            getData => ((ModSkillData)getData).Name),
                        new TableInfo("描述",
                            TableInfo.DEFAULT_GRID_WIDTH * 2,
                            getData => ((ModSkillData)getData).Desc),
                    },
                    () => new List<IModData>(Mod
                        .GetAllSkillData(true)
                        .GroupBy(skillData => skillData.SkillId)
                        .Select(d => 
                            d.OrderByDescending(skill => skill.SkillLv).First())
                        ),
                    modData => ((ModSkillData)modData).SkillId);
                drawer = intPropertyDrawer;
            }
            else if (seidProperty.SpecialDrawer.Contains("SeidDrawer"))
            {
                var intPropertyDrawer = new CtlIntBindTablePropertyDrawer(seidProperty.Desc,
                    value => sInt.Value = value,
                    () => sInt.Value,
                    tagSeidId =>
                    {
                        if (SeidGroup.DataGroups.TryGetValue(tagSeidId, out var tagSeidGroup))
                        {
                            var meta = tagSeidGroup.MetaData;
                            return $"【{tagSeidId} {meta.Name}】{meta.Desc}";
                        }

                        return $"【{tagSeidId}  ？】";
                    },
                    new List<TableInfo>()
                    {
                        new TableInfo("ID",
                            TableInfo.DEFAULT_GRID_WIDTH,
                            getData => ((ModSeidMeta)getData).Id.ToString()),
                        new TableInfo("名称",
                            TableInfo.DEFAULT_GRID_WIDTH,
                            getData => ((ModSeidMeta)getData).Name),
                        new TableInfo("描述",
                            TableInfo.DEFAULT_GRID_WIDTH * 2,
                            getData => ((ModSeidMeta)getData).Desc),
                    },
                    () => new List<IModData>(
                        SeidGroup.DataGroups.Values.Select(seidDataGroup => seidDataGroup.MetaData)).ModSort());
                drawer = intPropertyDrawer;
            }
            else
            {
                var intPropertyDrawer = new CtlIntPropertyDrawer(seidProperty.Desc,
                    value => sInt.Value = value,
                    () => sInt.Value);
                drawer = intPropertyDrawer;
            }
            Inspector.AddDrawer(drawer);
            CreateIntSpecialDrawer(drawer, seidProperty, sInt);
        }

        private void CreateIntSpecialDrawer(CtlPropertyDrawerBase drawer, ModSeidProperty seidProperty, ModSInt sInt)
        {
            foreach (var drawerId in seidProperty.SpecialDrawer)
            {
                switch (drawerId)
                {
                    case "SeidDrawer":
                    case "SkillDrawer":
                    case "BuffDrawer":
                        continue;
                    case "BuffTypeDrawer":
                    {
                        var dropdownPropertyDrawer = new CtlDropdownPropertyDrawer("",
                            () => ModEditorManager.I.BuffDataBuffTypes.Select(type => $"{type.TypeID} {type.TypeName}"),
                            index =>
                            {
                                var typeId = ModEditorManager.I.BuffDataBuffTypes[index].TypeID;
                                sInt.Value = typeId;
                                drawer.Refresh();
                                drawer.OnChanged?.Invoke();
                            },
                            () => ModEditorManager.I.BuffDataBuffTypes.FindIndex(type => type.TypeID == sInt.Value));
                        Inspector.AddDrawer(dropdownPropertyDrawer);
                        drawer.OnChanged += dropdownPropertyDrawer.Refresh;
                        break;
                    }
                    case "LevelTypeDrawer":
                    {
                        var dropdownPropertyDrawer = new CtlDropdownPropertyDrawer("",
                            () => ModEditorManager.I.LevelTypes.Select(type => $"{type.TypeID} {type.Desc}"),
                            index =>
                            {
                                var typeId = ModEditorManager.I.LevelTypes[index].TypeID;
                                sInt.Value = typeId;
                                drawer.Refresh();
                                drawer.OnChanged?.Invoke();
                            },
                            () => ModEditorManager.I.LevelTypes.FindIndex(type => type.TypeID == sInt.Value));
                        Inspector.AddDrawer(dropdownPropertyDrawer);
                        drawer.OnChanged += dropdownPropertyDrawer.Refresh;
                        break;
                    }
                    case "BuffRemoveTriggerTypeDrawer":
                    {
                        var dropdownPropertyDrawer = new CtlDropdownPropertyDrawer("",
                            () => ModEditorManager.I.BuffDataRemoveTriggerTypes.Select(type =>
                                $"{type.TypeID} {type.TypeName}"),
                            index =>
                            {
                                var typeId = ModEditorManager.I.BuffDataRemoveTriggerTypes[index].TypeID;
                                sInt.Value = typeId;
                                drawer.Refresh();
                                drawer.OnChanged?.Invoke();
                            },
                            () => ModEditorManager.I.BuffDataRemoveTriggerTypes.FindIndex(type =>
                                type.TypeID == sInt.Value));
                        Inspector.AddDrawer(dropdownPropertyDrawer);
                        drawer.OnChanged += dropdownPropertyDrawer.Refresh;
                        break;
                    }
                    case "AttackTypeDrawer":
                    {
                        var dropdownPropertyDrawer = new CtlDropdownPropertyDrawer("",
                            () => ModEditorManager.I.AttackTypes.Select(type =>
                                $"{type.Id} {type.Desc}"),
                            index =>
                            {
                                var typeId = ModEditorManager.I.AttackTypes[index].Id;
                                sInt.Value = typeId;
                                drawer.Refresh();
                                drawer.OnChanged?.Invoke();
                            },
                            () => ModEditorManager.I.AttackTypes.FindIndex(type =>
                                type.Id == sInt.Value));
                        Inspector.AddDrawer(dropdownPropertyDrawer);
                        drawer.OnChanged += dropdownPropertyDrawer.Refresh;
                        break;
                    }
                    case "ElementTypeDrawer":
                    {
                        var dropdownPropertyDrawer = new CtlDropdownPropertyDrawer("",
                            () => ModEditorManager.I.ElementTypes.Select(type =>
                                $"{type.Id} {type.Desc}"),
                            index =>
                            {
                                var typeId = ModEditorManager.I.ElementTypes[index].Id;
                                sInt.Value = typeId;
                                drawer.Refresh();
                                drawer.OnChanged?.Invoke();
                            },
                            () => ModEditorManager.I.ElementTypes.FindIndex(type =>
                                type.Id == sInt.Value));
                        Inspector.AddDrawer(dropdownPropertyDrawer);
                        drawer.OnChanged += dropdownPropertyDrawer.Refresh;
                        break;
                    }
                    case "TargetTypeDrawer":
                    {
                        var dropdownPropertyDrawer = new CtlDropdownPropertyDrawer("",
                            () => ModEditorManager.I.TargetTypes.Select(type =>
                                $"{type.TypeID} {type.TypeName}"),
                            index =>
                            {
                                var typeId = ModEditorManager.I.TargetTypes[index].TypeID;
                                sInt.Value = typeId;
                                drawer.Refresh();
                                drawer.OnChanged?.Invoke();
                            },
                            () => ModEditorManager.I.TargetTypes.FindIndex(type =>
                                type.TypeID == sInt.Value));
                        Inspector.AddDrawer(dropdownPropertyDrawer);
                        drawer.OnChanged += dropdownPropertyDrawer.Refresh;
                        break;
                    }
                    default:
                        Main.LogWarning($"未知的特殊绘制器 {drawerId}");
                        break;
                }
            }
        }

        private void CreateIntArrayDrawer(ModSeidProperty seidProperty, ModSeidData seidData)
        {
            CtlPropertyDrawerBase drawer;
            var sIntArray = seidData.GetToken<ModSIntArray>(seidProperty.ID);

            if (seidProperty.SpecialDrawer.Contains("BuffArrayDrawer"))
            {
                var intArrayPropertyDrawer = new CtlIntArrayBindTablePropertyDrawer(seidProperty.Desc, 
                    value => sIntArray.Value = value,
                    () => sIntArray.Value,
                    buffs =>
                    {
                        var sb = new StringBuilder();
                        for (var index = 0; index < buffs.Count; index++)
                        {
                            var buff = buffs[index];
                            var buffData = Mod.FindBuff(buff);
                            if (buffData != null)
                            {
                                sb.Append($"【{buff} {buffData.Name}】{buffData.Desc}");
                            }
                            else
                            {
                                sb.Append($"【{buff}  ？】");
                            }
                            if(index != buffs.Count - 1)
                                sb.Append("\n");
                        }

                        return sb.ToString();
                    },
                    new List<TableInfo>()
                    {
                        new TableInfo("ID",
                            TableInfo.DEFAULT_GRID_WIDTH,
                            getData => ((ModBuffData)getData).Id.ToString()),
                        new TableInfo("名称",
                            TableInfo.DEFAULT_GRID_WIDTH,
                            getData => ((ModBuffData)getData).Name),
                        new TableInfo("描述",
                            TableInfo.DEFAULT_GRID_WIDTH * 2,
                            getData => ((ModBuffData)getData).Desc),
                    },
                    () => new List<IModData>(Mod.GetAllBuffData()));
                drawer = intArrayPropertyDrawer;
            }
            else if (seidProperty.SpecialDrawer.Contains("SkillArrayDrawer"))
            {
                var intPropertyDrawer = new CtlIntArrayBindTablePropertyDrawer(seidProperty.Desc,
                    value => sIntArray.Value = value,
                    () => sIntArray.Value,
                    value =>
                    {
                        var sb = new StringBuilder();
                        for (var index = 0; index < value.Count; index++)
                        {
                            var skillId = value[index];
                            var skillData = Mod.FindSkillBySkillId(skillId);
                            if (skillData != null)
                            {
                                sb.Append($"【{skillData.Name}({skillData.SkillId})】{skillData.Desc}");
                            }
                            else
                            {
                                sb.Append($"【？({skillId})】");
                            }
                            if(index != value.Count - 1)
                                sb.Append("\n");
                        }

                        return sb.ToString();
                    },
                    new List<TableInfo>()
                    {
                        new TableInfo("ID",
                            TableInfo.DEFAULT_GRID_WIDTH,
                            getData => ((ModSkillData)getData).Id.ToString()),
                        new TableInfo("神通ID",
                            TableInfo.DEFAULT_GRID_WIDTH,
                            getData => ((ModSkillData)getData).SkillId.ToString()),
                        new TableInfo("名称",
                            TableInfo.DEFAULT_GRID_WIDTH,
                            getData => ((ModSkillData)getData).Name),
                        new TableInfo("描述",
                            TableInfo.DEFAULT_GRID_WIDTH * 2,
                            getData => ((ModSkillData)getData).Desc),
                    },
                    () => new List<IModData>(Mod
                        .GetAllSkillData(true)
                        .GroupBy(skillData => skillData.SkillId)
                        .Select(d => 
                            d.OrderByDescending(skill => skill.SkillLv).First())
                    ),
                    modData => ((ModSkillData)modData).SkillId);
                drawer = intPropertyDrawer;
            }
            else if (seidProperty.SpecialDrawer.Contains("ItemArrayDrawer"))
            {
                var intArrayPropertyDrawer = new CtlIntArrayBindTablePropertyDrawer(seidProperty.Desc, 
                    value => sIntArray.Value = value,
                    () => sIntArray.Value,
                    items =>
                    {
                        var sb = new StringBuilder();
                        for (var index = 0; index < items.Count; index++)
                        {
                            var item = items[index];
                            var itemData = Mod.FindItem(item);
                            if (itemData != null)
                            {
                                sb.Append($"【{item} {itemData.Name}】{itemData.Desc}");
                            }
                            else
                            {
                                sb.Append($"【{item}  ？】");
                            }
                            if(index != items.Count - 1)
                                sb.Append("\n");
                        }

                        return sb.ToString();
                    },
                    new List<TableInfo>()
                    {
                        new TableInfo("ID",
                            TableInfo.DEFAULT_GRID_WIDTH,
                            getData => ((ModItemData)getData).Id.ToString()),
                        new TableInfo("名称",
                            TableInfo.DEFAULT_GRID_WIDTH,
                            getData => ((ModItemData)getData).Name),
                        new TableInfo("描述",
                            TableInfo.DEFAULT_GRID_WIDTH * 2,
                            getData => ((ModItemData)getData).Desc),
                    },
                    () => new List<IModData>(Mod.GetAllItemData()));
                drawer = intArrayPropertyDrawer;
            }
            else if (seidProperty.SpecialDrawer.Contains("SeidArrayDrawer"))
            {
                var intArrayPropertyDrawer = new CtlIntArrayBindTablePropertyDrawer(seidProperty.Desc, 
                    value => sIntArray.Value = value,
                    () => sIntArray.Value,
                    seidList =>
                    {
                        var sb = new StringBuilder();
                        for (var index = 0; index < seidList.Count; index++)
                        {
                            var tagSeidId = seidList[index];
                            if (SeidGroup.DataGroups.TryGetValue(tagSeidId, out var tagSeidGroup))
                            {
                                var meta = tagSeidGroup.MetaData;
                                sb.Append($"【{tagSeidId} {meta.Name}】{meta.Desc}");
                            }
                            else
                            {
                                sb.Append($"【{tagSeidId}  ？】");
                            }
                            if(index != seidList.Count - 1)
                                sb.Append("\n");
                        }

                        return sb.ToString();
                    },
                    new List<TableInfo>()
                    {
                        new TableInfo("ID",
                            TableInfo.DEFAULT_GRID_WIDTH,
                            getData => ((ModSeidMeta)getData).Id.ToString()),
                        new TableInfo("名称",
                            TableInfo.DEFAULT_GRID_WIDTH,
                            getData => ((ModSeidMeta)getData).Name),
                        new TableInfo("描述",
                            TableInfo.DEFAULT_GRID_WIDTH * 2,
                            getData => ((ModSeidMeta)getData).Desc),
                    },
                    () => new List<IModData>(
                        SeidGroup.DataGroups.Values.Select(seidDataGroup => seidDataGroup.MetaData)).ModSort());
                drawer = intArrayPropertyDrawer;
            }
            else
            {
                var intArrayPropertyDrawer = new CtlIntArrayPropertyDrawer(seidProperty.Desc,
                    value => sIntArray.Value = value,
                    () => sIntArray.Value);
                
                drawer = intArrayPropertyDrawer;
            }
            
            Inspector.AddDrawer(drawer);
            CreateIntArraySpecialDrawer(drawer, seidProperty, sIntArray);
        }

        private void CreateIntArraySpecialDrawer(CtlPropertyDrawerBase drawer, ModSeidProperty seidProperty,
            ModSIntArray sIntArray)
        {
            foreach (var drawerId in seidProperty.SpecialDrawer)
            {
                switch (drawerId)
                {
                    case "ItemArrayDrawer":
                    case "SeidArrayDrawer":
                    case "SkillArrayDrawer":
                    case "BuffArrayDrawer":
                        continue;
                    default:
                        Main.LogWarning($"未知的特殊绘制器 {drawerId}");
                        break;
                }
            }
        }

        private void CreateStringDrawer(ModSeidProperty seidProperty, ModSeidData seidData)
        {
            CtlPropertyDrawerBase drawer;
            var sString = seidData.GetToken<ModSString>(seidProperty.ID);
            
            var stringPropertyDrawer = new CtlStringPropertyDrawer(seidProperty.Desc,
                value => sString.Value = value,
                () => sString.Value);
            Inspector.AddDrawer(stringPropertyDrawer);
            drawer = stringPropertyDrawer;
            CreateStringSpecialDrawer(drawer, seidProperty, sString);
        }

        private void CreateStringSpecialDrawer(CtlPropertyDrawerBase drawer, ModSeidProperty seidProperty, ModSString sString)
        {
            foreach (var drawerId in seidProperty.SpecialDrawer)
            {
                switch (drawerId)
                {
                    case "ComparisonOperatorTypeDrawer":
                    {
                        var buffTypeDrawer = new CtlDropdownPropertyDrawer("",
                            () => ModEditorManager.I.ComparisonOperatorTypes.Select(type =>
                                $"{type.TypeStrID} {type.TypeName}"),
                            index =>
                            {
                                var typeId = ModEditorManager.I.ComparisonOperatorTypes[index].TypeStrID;
                                sString.Value = typeId;
                                drawer.Refresh();
                                drawer.OnChanged?.Invoke();
                            },
                            () => ModEditorManager.I.ComparisonOperatorTypes.FindIndex(type =>
                                type.TypeStrID == sString.Value));
                        Inspector.AddDrawer(buffTypeDrawer);
                        drawer.OnChanged += buffTypeDrawer.Refresh;
                        break;
                    }
                    default:
                        Main.LogWarning($"未知的特殊绘制器 {drawerId}");
                        break;
                }
            }
        }

        private void CreateFloatDrawer(ModSeidProperty seidProperty, ModSeidData seidData)
        {
            CtlPropertyDrawerBase drawer;
            var sFloat = seidData.GetToken<ModSFloat>(seidProperty.ID);
            
            var floatPropertyDrawer = new CtlFloatPropertyDrawer(seidProperty.Desc,
                value => sFloat.Value = value,
                () => sFloat.Value);
            
            Inspector.AddDrawer(floatPropertyDrawer);
            drawer = floatPropertyDrawer;
        }

        private bool IsSeidEnable(int seidId)
        {
            return SeidList.Contains(seidId);
        }

        private void OnTreeNodeRender(GTreeNode node, GComponent item)
        {
            var btn = item.asButton;
            var nodeData = (SeidNodeInfo)node.data;
            
            btn.title = nodeData.NodeName;
            btn.icon = nodeData.NodeIcon;
        }

        private void AddSeidList(string listName, List<int> seidList, List<GTreeNode> seidNodes)
        {
            var listData = new SeidNodeInfo()
            {
                IsSeid = false,
                NodeName = listName,
                NodeIcon = "",
            };
            
            var listNode = new GTreeNode(true)
            {
                data = listData
            };

            foreach (var seidId in seidList)
            {
                var seidData = new SeidNodeInfo()
                {
                    IsSeid = true,
                    NodeIcon = "ui://NextCore/icon_dao",
                    SeidID = seidId,
                };
                
                if(SeidMetas.TryGetValue(seidId,out var seidMeta))
                {
                    seidData.NodeName = $"{seidId} {seidMeta.Name}";
                }
                else
                {
                    seidData.NodeName = $"{seidId}";
                }
                
                var seidNode = new GTreeNode(false)
                {
                    data = seidData
                };
                
                seidNodes.Add(seidNode);
                listNode.AddChild(seidNode);
            }

            listNode.expanded = true;
            SeidEditor.m_list.rootNode.AddChild(listNode);
        }
    }
}