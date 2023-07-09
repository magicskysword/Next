using System;
using System.Collections.Generic;
using System.Linq;
using FairyGUI;
using SkySwordKill.Next.FGUI;
using SkySwordKill.Next.FGUI.Component;
using SkySwordKill.Next.FGUI.Dialog;
using SkySwordKill.NextFGUI.NextCore;
using SkySwordKill.NextModEditor.Mod;
using SkySwordKill.NextModEditor.Mod.Data;
using UnityEngine;

namespace SkySwordKill.NextModEditor.PanelPage;

public class ModDataTableDataList<T> : TableDataList<T> where T : IModData, new()
{
    public ModDataTableDataList(List<T> list) : base(list)
    {
            
    }

    /// <summary>
    /// 移除前触发
    /// </summary>
    public Action<T> OnRemoveItem { get; set; } = data => { };
    /// <summary>
    /// 添加后触发
    /// </summary>
    public Action<T> OnAddItem { get; set; } = data => { };

    public T AddItem(int id)
    {
        var modData = new T()
        {
            Id = id
        };
        return AddItem(modData);
    }

    public T AddItem(T modData)
    {
        if (TryGetItem(modData.Id, out var data))
            return data;
        
        _list.Add(modData);
        OnAddItem(modData);
        _list.ModSort();
        return modData;
    }

    public void RemoveItem(int id)
    {
        var index = _list.FindIndex(data => data.Id == id);
        if (index >= 0)
        {
            var data = _list[index];
            OnRemoveItem(data);
            _list.RemoveAt(index);
        }
        _list.ModSort();
    }

    public bool HasId(int id)
    {
        return _list.HasId(id);
    }
        
    public bool TryGetItem(int id, out T item)
    {
        return _list.TryFindData(id, out item);
    }
    
    public T GetItemById(int id)
    {
        _list.TryFindData(id, out var item);
        return item;
    }
}

public class AddDataUndoCommand : IUndoCommand
{
    private Action<IModData> _onAdd;
    private Action<IModData> _onRemove;
    private Func<IModData> _onFirstAdd;
        
    private IModData _data;
        
    public AddDataUndoCommand(Func<IModData> onFirstAdd, Action<IModData> onAdd,Action<IModData> onRemove)
    {
        _onFirstAdd = onFirstAdd;
        _onRemove = onRemove;
        _onAdd = onAdd;
    }

    public void Execute()
    {
        if (_data == null)
        {
            _data = _onFirstAdd();
        }
        else
        {
            _onAdd.Invoke(_data);
        }
    }

    public void Undo()
    {
        _onRemove.Invoke(_data);
    }
}

public class RemoveDataUndoCommand : IUndoCommand
{
    private Action<IModData> _onAdd;
    private Action<IModData> _onRemove;
    private IModData _data;
        
    public RemoveDataUndoCommand(IModData data, Action<IModData> onAdd,Action<IModData> onRemove)
    {
        _data = data;
        _onRemove = onRemove;
        _onAdd = onAdd;
    }
        
    public void Execute()
    {
        _onRemove(_data);
    }

    public void Undo()
    {
        _onAdd(_data);
    }
}
    
public abstract class PanelTablePageBase<T> : PanelPageBase, IModDataClipboardPage ,IUndoInst
    where T : class, IModData, new()
{
    protected PanelTablePageBase(string name, ModWorkshop mod, ModProject project) : base(name)
    {
        Mod = mod;
        Project = project;
    }
        
    private bool _editable = true;
    public bool Editable
    {
        get => _editable;
        set
        {
            _editable = value;
            if (IsInit)
            {
                RefreshTable();
                ReloadInspector();
            }
        }
    }

    public ModWorkshop Mod { get; set; }
    public ModProject Project { get; set; }
    public ModDataClipboard DataClipboard { get; set; }
    public List<TableInfo> TableInfos { get; } = new List<TableInfo>();
    public CtlTableEditor TableEditor { get; set; }
    public int CurInspectIndex { get; set; } = -1;
    public CtlTableList TableList => TableEditor.TableList;
    public CtlPropertyInspector Inspector => TableEditor.Inspector;
    public GButton BtnToolsAdd { get; set; }
    public GButton BtnToolsRemove { get; set; }
    public GButton BtnToolsUndo { get; set; }
    public GButton BtnToolsRedo { get; set; }
    public GButton BtnToolsCopy { get; set; }
    public GButton BtnToolsPaste { get; set; }
    public GButton BtnToolsPasteAsNew { get; set; }
    public GButton BtnToolsDelete { get; set; }
    /// <summary>
    /// Mod数据表封装
    /// </summary>
    public abstract ModDataTableDataList<T> ModDataTableDataList { get; set; }
    public UndoInstManager UndoManager { get; set; } = new UndoInstManager();
    public bool IsInit { get; private set; } = false;
    public virtual bool NeedRemoveSeidData { get; set; } = false;

    protected override GObject OnAdd()
    {
        IsInit = true;
        OnInit();
        TableEditor = new CtlTableEditor(UI_ComTableEditor.CreateInstance());
        TableList.SetItemRenderer(TableItemRenderer);
        TableList.SetClickItem(OnClickTableItem);
        TableList.SetRightClickItem(OnRightClickTableItem);
        TableList.SetTableRightClick(() => OnPopupMenu(null));
        TableList.AllowClickToSelect = false;
        TableList.MultiSelect = true;
        TableList.BindTable(TableInfos, ModDataTableDataList);

        UndoManager.OnChanged += RefreshToolsBar;
            
        BtnToolsAdd = TableEditor.ToolsBar.AddToolBtn("ui://NextCore/icon_add", "新建数据(Ctrl + N)", OnClickAdd);
        BtnToolsRemove = TableEditor.ToolsBar.AddToolBtn("ui://NextCore/icon_trash", "移除数据(Delete)", OnClickRemove);
        TableEditor.ToolsBar.AddToolSep();
        BtnToolsUndo = TableEditor.ToolsBar.AddToolBtn("ui://NextCore/icon_undo", "撤销(Ctrl + Z)", OnClickUndo);
        BtnToolsRedo = TableEditor.ToolsBar.AddToolBtn("ui://NextCore/icon_redo", "重做(Ctrl + Y)", OnClickRedo);
        TableEditor.ToolsBar.AddToolSep();
        BtnToolsCopy = TableEditor.ToolsBar.AddToolBtn("ui://NextCore/icon_copy", "复制(Ctrl + C)", OnClickCopy);
        BtnToolsPaste = TableEditor.ToolsBar.AddToolBtn("ui://NextCore/icon_paste", "粘贴(Ctrl + V)", OnClickPaste);
        BtnToolsPasteAsNew = TableEditor.ToolsBar.AddToolBtn("ui://NextCore/icon_paste_as_new", "粘贴为新数据(Ctrl + Shift + V)", OnClickPasteAsNew);
        TableEditor.ToolsBar.AddToolSep();
        BtnToolsDelete = TableEditor.ToolsBar.AddToolBtn("ui://NextCore/icon_delete", "删除数据(Delete)", OnClickDelete);
        TableEditor.ToolsBar.AddToolSearch(OnSearchData);
            
        CurInspectIndex = -1;
        RefreshToolsBar();
        return TableEditor.MainView;
    }
    

    protected abstract void OnInit();

    protected override void OnOpen()
    {
        Refresh();
    }
        
    protected override void OnRemove()
    {
        TableInfos.Clear();
    }

    public override bool OnHandleKey(InputEvent evt)
    {
        switch (evt.keyCode)
        {
            case KeyCode.C:
                if (evt.ctrlOrCmd)
                {
                    OnClickCopy();
                    return true;
                }
                break;
            case KeyCode.V:
                if (evt.ctrlOrCmd)
                {
                    if (evt.shift)
                    {
                        OnClickPasteAsNew();
                    }
                    else
                    {
                        OnClickPaste();
                    }
                    return true;
                }
                break;
            case KeyCode.Z:
                if (evt.ctrlOrCmd)
                {
                    if (evt.shift)
                    {
                        OnClickRedo();
                    }
                    else
                    {
                        OnClickUndo();
                    }
                        
                    return true;
                }
                break;
            case KeyCode.Y:
                if (evt.ctrl)
                {
                    OnClickRedo();
                    return true;
                }
                break;
            case KeyCode.Delete:
                OnClickRemove();
                return true;
            case KeyCode.N:
                if (evt.ctrlOrCmd)
                {
                    OnClickAdd();
                    return true;
                }
                break;
        }
        return false;
    }

    public void AddTableHeader(TableInfo info)
    {
        TableInfos.Add(info);
    }
        
    public virtual void Refresh()
    {
        RefreshTable();
        RefreshInspector();
    }

    private void RefreshInspector()
    {
        if(CurInspectIndex >= 0 && CurInspectIndex < GetDataCount())
            TableList.ScrollToView(CurInspectIndex);
        InspectItem(CurInspectIndex);
    }

    public void RefreshTable()
    {
        TableList.Refresh();
    }

    protected virtual void TableItemRenderer(int index, UI_ComTableRow row, object data)
    {
            
    }
    
    protected virtual void OnClickTableItem()
    {
        Focus(TableList.SelectedIndex);
    }

    /// <summary>
    /// 聚焦到选中的数据
    /// </summary>
    /// <param name="indexArray"></param>
    protected virtual void Focus(params int[] indexArray)
    {
        if(indexArray.Length == 0)
            return;
        
        var toIndex = indexArray.Min();
        var curIndex = CurInspectIndex;
        var curIndexArray = TableList.SelectionArea.ToArray();
        this.Record(new CommonUndoCommand(
            () =>
            {
                CurInspectIndex = toIndex;
                TableList.SelectionArea = indexArray;
                RefreshInspector();
            }, 
            () =>
            {
                CurInspectIndex = curIndex;
                TableList.SelectionArea = curIndexArray;
                RefreshInspector();
            }));
    }

    /// <summary>
    /// 聚焦到选中的数据组
    /// </summary>
    /// <param name="indexArray"></param>
    protected virtual void FocusWithoutRecord(params int[] indexArray)
    {
        if(indexArray.Length == 0)
            return;
        
        var toIndex = indexArray.Min();
        CurInspectIndex = toIndex;
        TableList.SelectionArea = indexArray;
        RefreshInspector();
    }
        
    protected virtual void OnRightClickTableItem()
    {
        OnPopupMenu(TableList.SelectedItems.Select(data => (T) data).ToArray());
    }

    protected void OnPopupMenu(T[] selectedItems)
    {
        var popupMenu = BuildTableItemPopupMenu(selectedItems);
        popupMenu.Show(null, PopupDirection.Down);
    }

    protected virtual void OnClickPasteAsNew()
    {
        if (CanPaste(DataClipboard.CopyDatas))
        {
            PasteCopyDataAsNew(DataClipboard.CurCopyData);
            RefreshTable();
        }
    }
        
    protected virtual PopupMenu BuildTableItemPopupMenu(T[] modDataArray)
    {
        var menu = new PopupMenu();
        menu.AddItem("新建", TryAddData).enabled = Editable;
        menu.AddItem("复制", () => OnCopy(modDataArray)).enabled = modDataArray != null && CanCopy(modDataArray);
        menu.AddItem("粘贴", OnClickPaste).enabled = Editable && CanPaste(DataClipboard.CopyDatas);
        menu.AddItem("另存为", OnClickPasteAsNew).enabled = Editable && CanPaste(DataClipboard.CopyDatas);
        OnBuildCustomPopupMenu(menu, modDataArray);
        menu.AddItem("删除", () => TryRemoveData(modDataArray)).enabled = Editable && CanRemove(modDataArray);
        return menu;
    }

    protected virtual void OnBuildCustomPopupMenu(PopupMenu menu, T[] modDataArray)
    {
            
    }

    private void InspectItem(int index)
    {
        Inspector.Editable = Editable;
        if (index >= 0 && index < TableList.GetDataCount())
        {
            CurInspectIndex = index;
            var data = (T)TableList.GetData(index);
            Inspector.Clear();
            OnInspectItem(data);
            Inspector.Refresh();
        }
        else
        {
            CurInspectIndex = -1;
            Inspector.Clear();
        }
        RefreshToolsBar();
    }

    private void RefreshToolsBar()
    {
        if (Editable)
        {
            BtnToolsAdd.enabled = true;
            var curIndexArray = TableList.SelectionArea.ToArray();
            var modDataArray = curIndexArray.Select(GetItem).ToArray();
            if(curIndexArray.Length > 0)
            {
                BtnToolsRemove.enabled = CanRemove(modDataArray);
                BtnToolsCopy.enabled = CanCopy(modDataArray);
            }
            else
            {
                BtnToolsRemove.enabled = false;
                BtnToolsCopy.enabled = false;
            }
            BtnToolsPaste.enabled = Editable && CanPaste(DataClipboard?.CopyDatas);
            BtnToolsPasteAsNew.enabled = Editable && CanPaste(DataClipboard?.CopyDatas);
        }
        else
        {
            BtnToolsAdd.enabled = false;
            BtnToolsRemove.enabled = false;
            BtnToolsCopy.enabled = true;
            BtnToolsPaste.enabled = false;
            BtnToolsPasteAsNew.enabled = false;
        }
            
        BtnToolsUndo.enabled = CanUndo();
        BtnToolsRedo.enabled = CanRedo();
    }

    private void OnClickAdd()
    {
        TryAddData();
    }

    protected void TryAddData()
    {
        WindowIntInputDialog.CreateDialog("新建数据ID", true, id =>
        {
            if (HasId(id))
            {
                WindowConfirmDialog.CreateDialog("提示", "创建数据失败，对应ID已存在。", false);
            } 
            else
            {
                this.Record(new AddDataUndoCommand(
                    () =>
                    {
                        var data = new T()
                        {
                            Id = id
                        };
                        AddData(data);
                        return data;
                    },
                    modData => AddData((T)modData),
                    modData => RemoveData(modData.Id)));
            }
        });
    }
        
    private void OnClickRemove()
    {
        var dataArray = TableList.SelectedItems.Select(d => (T)d).ToArray();
        if(dataArray.Length == 0)
            return;
        
        if(dataArray.Length == 1)
        {
            TryRemoveData(dataArray[0]);
        }
        else
        {
            TryRemoveData(dataArray);
        }
    }

    protected void TryRemoveData(T modData)
    {
        if (NeedRemoveSeidData)
        {
            WindowConfirmDialogExtra.CreateDialog("提示", $"即将删除数据【{OnGetDataName(modData)}】，是否确认？", "同时删除所有引用该数据的数据", true,false, (removeSeidData) =>
            {
                OnRemoveData(modData, removeSeidData);
            });
        }
        else
        {
            WindowConfirmDialog.CreateDialog("提示", $"即将删除数据【{OnGetDataName(modData)}】，是否确认？", true, () =>
            {
                OnRemoveData(modData);
            });
        }
        
        
    }
    
    protected void TryRemoveData(T[] modDataArray)
    {
        WindowConfirmDialog.CreateDialog("提示", $"即将删除【{OnGetDataName(modDataArray[0])}】等{modDataArray.Length}项数据，是否确认？", true, () =>
        {
            for (var index = 0; index < modDataArray.Length; index++)
            {
                var modData = modDataArray[index];
                OnRemoveData(modData);
            }
        });
    }
    
    /// <summary>
    /// 移除数据，基类只移除数据，不移除seid数据
    /// </summary>
    /// <param name="modData"></param>
    /// <param name="removeSeidData"></param>
    protected virtual void OnRemoveData(T modData, bool removeSeidData = false)
    {
        this.Record(new RemoveDataUndoCommand(
                modData,
                data => AddData((T)data),
                data => RemoveData(data.Id)
            )
        );
    }

    protected virtual void OnClickCopy()
    {
        OnCopy(TableList.SelectedItems.Select(d => (T)d).ToArray());
    }
        
    protected virtual void OnCopy(T[] modDataArray)
    {
        if(CanCopy(modDataArray))
        {
            DataClipboard.SetCopyData(modDataArray.Select(GetCopyData));
            RefreshToolsBar();
        }
    }
        
    protected virtual void OnClickPaste()
    {
        if (CanPaste(DataClipboard.CopyDatas))
        {
            PasteCopyData(DataClipboard.CopyDatas);
            RefreshTable();
        }
    }
    
    private void OnClickDelete()
    {
        TryRemoveData(TableList.SelectedItems.Select(d => (T)d).ToArray());
    }
        
    protected virtual void OnClickUndo()
    {
        if (CanUndo())
        {
            UndoManager.Undo();
            Refresh();
        }
    }

    protected virtual void OnClickRedo()
    {
        if (CanRedo())
        {
            UndoManager.Redo();
            Refresh();
        }
    }
        
    protected virtual void AddData(T data)
    {
        ModDataTableDataList.AddItem(data);
        RefreshTable();
        var index = GetIndexById(data.Id);
        TableList.SelectedIndex = index;
        TableList.MainView.m_list.ScrollToView(index, false);
        InspectItem(TableList.SelectedIndex);
    }
        
    protected virtual void RemoveData(int id)
    {
        ModDataTableDataList.RemoveItem(id);
        RefreshTable();
        InspectItem(Mathf.Min(CurInspectIndex, TableEditor.TableList.GetDataCount()));
    }
    
    protected virtual void OnSearchData(string input) 
    {
        TableEditor.TableList.SearchItems(input);
        TableList.SelectedIndex = 0;
        InspectItem(TableList.SelectedIndex);
    }

    protected virtual TDrawer AddDrawer<TDrawer>(TDrawer drawer) where TDrawer : IPropertyDrawer
    {
        drawer.AddChangeListener(RefreshTable);
        drawer.UndoManager = UndoManager;
        Inspector.AddDrawer(drawer);
        return drawer;
    }
        
    protected abstract void OnInspectItem(T data);
        
    public void ReloadInspector()
    {
        InspectItem(CurInspectIndex);
    }

    public virtual bool HasId(int id)
    {
        return GetIndexById(id) != -1;
    }
        
    public virtual T GetItemById(int id)
    {
        ModDataTableDataList.TryGetItem(id, out var item);
        return item;
    }
        
    public virtual int GetIndexById(int id)
    {
        for (var i = 0; i < GetDataCount(); i++)
        {
            var data = GetItem(i);
            if(data.Id == id)
                return i;
        }
        return -1;
    }
        
    protected virtual bool CanUndo()
    {
        return Editable && UndoManager.CanUndo;
    }
        
    protected virtual bool CanRedo()
    {
        return Editable && UndoManager.CanRedo;
    }

    protected virtual bool CanCopy(T[] data)
    {
        return data != null;
    }
        
    protected CopyData GetCopyData(T data)
    {
        return new CopyData()
        {
            Data = data,
            Project = Project,
            Name = OnGetDataName(data)
        };
    }

    public abstract string OnGetDataName(T data);
    
    private bool CanRemove(T[] modDataArray)
    {
        return modDataArray != null &&  modDataArray.Length > 0;
    }

    public virtual bool CanPaste(List<CopyData> dataArray)
    {
        if(dataArray == null || dataArray.Count == 0)
            return false;
            
        return dataArray.All(copyData => copyData.Data is T);
    }
        
    public void PasteCopyData(List<CopyData> copyDataArray)
    {
        if (copyDataArray.Count == 1)
        {
            var copyData = copyDataArray[0];
            if (ModDataTableDataList.TryGetItem(copyData.Data.Id, out var data))
            {
                if(copyData.Data == data)
                {
                    PasteCopyDataAsNew(copyData);
                    return;
                }
                
                WindowConfirmDialog.CreateDialog("提示", $"目标ID:【{copyData.Data.Id}】已经存在数据，是否覆盖？", true, () =>
                {
                    ModDataTableDataList.RemoveItem(copyData.Data.Id);
                    this.Record(new AddDataUndoCommand(
                            () => OnPasteData(copyData, copyData.Data.Id),
                            data => AddData((T)data),
                            data => RemoveData(data.Id)
                        )
                    );
                    
                    var index = GetIndexById(copyData.Data.Id);
                    Focus(index);
                    RefreshTable();
                });
            }
            else
            {
                this.Record(new AddDataUndoCommand(
                        () => OnPasteData(copyData, copyData.Data.Id),
                        data => AddData((T)data),
                        data => RemoveData(data.Id)
                    )
                );

                var index = GetIndexById(copyData.Data.Id);
                Focus(index);
                RefreshTable();
            }
        }
        else
        {
            if (copyDataArray.Any(copyData => ModDataTableDataList.HasId(copyData.Data.Id)))
            {
                var conflictDataArray = copyDataArray.Where(copyData => ModDataTableDataList.HasId(copyData.Data.Id)).ToArray();
                WindowConfirmDialog.CreateDialog("提示", $"目标ID:【{string.Join("," ,conflictDataArray.Select(d => d.Data.Id.ToString()))}】已经存在数据，是否覆盖？", true, () =>
                {
                    var commandSequence = new SequenceCommand();
                    foreach (var copyData in copyDataArray)
                    {
                        if (ModDataTableDataList.HasId(copyData.Data.Id))
                        {
                            var removeData = ModDataTableDataList.GetItemById(copyData.Data.Id);
                            commandSequence.AddCommand(new RemoveDataUndoCommand(removeData, 
                                data => AddData((T)data), 
                                data => RemoveData(data.Id)));
                        }
                        commandSequence.AddCommand(new AddDataUndoCommand(
                            () => OnPasteData(copyData, copyData.Data.Id),
                            data => AddData((T)data),
                            data => RemoveData(data.Id)));
                    }
                    this.Record(commandSequence);

                    var indexArray = copyDataArray.Select(copyData => GetIndexById(copyData.Data.Id)).ToArray();
                    Focus(indexArray);
                    RefreshTable();
                });
            }
            else
            {
                var commandSequence = new SequenceCommand();
                foreach (var copyData in copyDataArray)
                {
                    commandSequence.AddCommand(new AddDataUndoCommand(
                        () => OnPasteData(copyData, copyData.Data.Id),
                        data => AddData((T)data),
                        data => RemoveData(data.Id)));
                }
                this.Record(commandSequence);
                
                var indexArray = copyDataArray.Select(copyData => GetIndexById(copyData.Data.Id)).ToArray();
                Focus(indexArray);
                RefreshTable();
            }
        }
    }
        
    private void PasteCopyDataAsNew(CopyData copyData)
    {
        WindowIntInputDialog.CreateDialog("请输入新的数据ID", true, id =>
        {
            if (HasId(id))
            {
                WindowConfirmDialog.CreateDialog("提示", "粘贴数据失败，对应ID已存在。", false);
            }
            else
            {
                this.Record(new AddDataUndoCommand(
                        () => OnPasteData(copyData, id),
                        data => AddData((T)data),
                        data => RemoveData(data.Id)
                    )
                );
                RefreshTable();
            }
        });
    }

    protected abstract T OnPasteData(CopyData copyData, int targetId);

    public int GetDataCount()
    {
        return TableList.GetDataCount();
    }
        
    public T GetItem(int index)
    {
        if(index < 0 || index >= GetDataCount())
            return default;
        return (T)TableList.GetData(index);
    }
}