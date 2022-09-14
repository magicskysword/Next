using System;
using System.Collections.Generic;
using FairyGUI;
using SkySwordKill.Next;
using SkySwordKill.Next.FGUI;
using SkySwordKill.Next.FGUI.Component;
using SkySwordKill.Next.FGUI.Dialog;
using SkySwordKill.NextEditor.Mod;
using SkySwordKill.NextFGUI.NextCore;
using SkySwordKill.NextModEditor.Mod.Data;
using UnityEngine;

namespace SkySwordKill.NextEditor.PanelPage
{
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
        public abstract ModDataTableDataList<T> ModDataTableDataList { get; set; }
        public UndoInstManager UndoManager { get; set; } = new UndoInstManager();
        public bool IsInit { get; private set; } = false;

        protected override GObject OnAdd()
        {
            IsInit = true;
            OnInit();
            TableEditor = new CtlTableEditor(UI_ComTableEditor.CreateInstance());
            TableList.SetItemRenderer(TableItemRenderer);
            TableList.SetClickItem(OnClickTableItem);
            TableList.SetRightClickItem(OnRightClickTableItem);
            TableList.SetTableRightClick(() => OnPopupMenu(null));
            TableList.BindTable(TableInfos, ModDataTableDataList);

            UndoManager.OnChanged += RefreshToolsBar;
            
            BtnToolsAdd = TableEditor.ToolsBar.AddToolBtn("ui://NextCore/icon_add", "新建数据(Ctrl + N)", OnClickAdd);
            BtnToolsRemove = TableEditor.ToolsBar.AddToolBtn("ui://NextCore/icon_minus", "移除数据(Delete)", OnClickRemove);
            TableEditor.ToolsBar.AddToolSep();
            BtnToolsUndo = TableEditor.ToolsBar.AddToolBtn("ui://NextCore/icon_undo", "撤销(Ctrl + Z)", OnClickUndo);
            BtnToolsRedo = TableEditor.ToolsBar.AddToolBtn("ui://NextCore/icon_redo", "重做(Ctrl + Y)", OnClickRedo);
            TableEditor.ToolsBar.AddToolSep();
            BtnToolsCopy = TableEditor.ToolsBar.AddToolBtn("ui://NextCore/icon_copy", "复制(Ctrl + C)", OnClickCopy);
            BtnToolsPaste = TableEditor.ToolsBar.AddToolBtn("ui://NextCore/icon_paste", "粘贴(Ctrl + V)", OnClickPaste);
            BtnToolsPasteAsNew = TableEditor.ToolsBar.AddToolBtn("ui://NextCore/icon_paste_as_new", "粘贴为新数据(Ctrl + Shift + V)", OnClickPasteAsNew);
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
            InspectItem(CurInspectIndex);
        }

    
        public void RefreshTable()
        {
            TableList.SelectedIndex = CurInspectIndex;
            TableList.Refresh();
        }
    
        public void RefreshCurrentRow()
        {
            if(CurInspectIndex < 0)
                return;
    
            TableList.RefreshRowAt(CurInspectIndex);
        }
        
        protected virtual void TableItemRenderer(int index, UI_ComTableRow row, object data)
        {
            
        }
    
        protected virtual void OnClickTableItem(int index, object data)
        {
            var toIndex = index;
            var curIndex = CurInspectIndex;
            this.Record(new CommonUndoCommand(
                () =>
            {
                CurInspectIndex = toIndex;
                Refresh();
            }, 
                () =>
            {
                CurInspectIndex = curIndex;
                Refresh();
            }));
        }
        
        protected virtual void OnRightClickTableItem(int index, object data)
        {
            OnPopupMenu((T)data);
        }

        protected void OnPopupMenu(T modData)
        {
            var popupMenu = BuildTableItemPopupMenu(modData);
            popupMenu.Show(null, PopupDirection.Down);
        }

        protected virtual void OnClickPasteAsNew()
        {
            if (CanPaste(DataClipboard.CurCopyData))
            {
                PasteCopyDataAsNew(DataClipboard.CurCopyData);
                RefreshTable();
            }
        }
        
        protected virtual PopupMenu BuildTableItemPopupMenu(T modData)
        {
            var menu = new PopupMenu();
            menu.AddItem("新建", TryAddData).enabled = Editable;
            menu.AddItem("复制", () => OnCopy(modData)).enabled = CanCopy(modData);
            menu.AddItem("粘贴", OnClickPaste).enabled = Editable && CanPaste(DataClipboard.CurCopyData);
            menu.AddItem("另存为", OnClickPasteAsNew).enabled = Editable && CanPaste(DataClipboard.CurCopyData);
            menu.AddItem("删除", () => TryRemoveData(modData)).enabled = Editable && modData != null;
            return menu;
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
                if(CurInspectIndex >= 0 && CurInspectIndex < TableList.GetDataCount())
                {
                    BtnToolsRemove.enabled = true;
                    BtnToolsCopy.enabled = CanCopy((T)TableList.GetData(CurInspectIndex));
                }
                else
                {
                    BtnToolsRemove.enabled = false;
                    BtnToolsCopy.enabled = false;
                }
                BtnToolsPaste.enabled = Editable && CanPaste(DataClipboard?.CurCopyData);
                BtnToolsPasteAsNew.enabled = Editable && CanPaste(DataClipboard?.CurCopyData);
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
            var index = CurInspectIndex;
            if(index < 0 || index > TableList.GetDataCount())
                return;
            
            var data = (T)TableList.GetData(CurInspectIndex);
            TryRemoveData(data);
        }

        protected void TryRemoveData(T modData)
        {
            WindowConfirmDialog.CreateDialog("提示", $"即将删除数据【{OnGetDataName(modData)}】，删除后Seid数据无法恢复，是否确认？", true, () =>
            {
                this.Record(new RemoveDataUndoCommand(
                        modData,
                    data => AddData((T)data),
                    data => RemoveData(data.Id)
                    )
                );
            });
        }

        protected virtual void OnClickCopy()
        {
            OnCopy((T)TableList.GetData(CurInspectIndex));
        }
        
        protected virtual void OnCopy(T modData)
        {
            if(CanCopy(modData))
            {
                DataClipboard.CurCopyData = GetCopyData(modData);
                RefreshToolsBar();
            }
        }
        
        protected virtual void OnClickPaste()
        {
            if (CanPaste(DataClipboard.CurCopyData))
            {
                PasteCopyData(DataClipboard.CurCopyData);
                RefreshTable();
            }
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

        protected virtual void AddDrawer(CtlPropertyDrawerBase drawer)
        {
            drawer.OnChanged += RefreshTable;
            drawer.UndoManager = UndoManager;
            Inspector.AddDrawer(drawer);
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
            return ModDataTableDataList.GetItem(GetIndexById(id));
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

        protected virtual bool CanCopy(T data)
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

        public virtual bool CanPaste(CopyData data)
        {
            if(data == null)
                return false;
            
            return data.Data is T;
        }
        
        public void PasteCopyData(CopyData copyData)
        {
            if (ModDataTableDataList.TryGetItem(copyData.Data.Id, out var data))
            {
                if(copyData.Data == data)
                {
                    PasteCopyDataAsNew(copyData);
                    return;
                }
                
                WindowConfirmDialog.CreateDialog("提示", "目标ID已经存在数据，是否覆盖？", true, () =>
                {
                    ModDataTableDataList.RemoveItem(copyData.Data.Id);
                    OnPasteData(copyData, copyData.Data.Id);
                    RefreshTable();
                });
            }
            else
            {
                OnPasteData(copyData, copyData.Data.Id);
                RefreshTable();
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
}