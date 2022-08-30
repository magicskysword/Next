using System;
using System.Collections.Generic;
using FairyGUI;
using SkySwordKill.Next;
using SkySwordKill.Next.FGUI.Component;
using SkySwordKill.Next.FGUI.Dialog;
using SkySwordKill.NextEditor.Mod;
using SkySwordKill.NextFGUI.NextCore;
using SkySwordKill.NextModEditor.Mod.Data;

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

        public void AddItem(int id)
        {
            var modData = new T()
            {
                Id = id
            };
            _list.Add(modData);
            OnAddItem(modData);
            _list.ModSort();
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
    }
    
    public abstract class PanelTablePageBase<T> : PanelPageBase, IModDataClipboardPage where T : class, IModData, new()
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
        public GButton BtnAdd { get; set; }
        public GButton BtnRemove { get; set; }
        public abstract ModDataTableDataList<T> ModDataTableDataList { get; set; }
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
            
            BtnAdd = TableEditor.ToolsBar.AddToolBtn("ui://NextCore/icon_add", "新建数据", OnClickAdd);
            BtnRemove = TableEditor.ToolsBar.AddToolBtn("ui://NextCore/icon_minus", "移除数据", OnClickRemove);
            TableEditor.ToolsBar.AddToolSearch(OnSearchData);
            
            CurInspectIndex = -1;
            RefreshToolsBar();
            return TableEditor.MainView;
        }

        protected abstract void OnInit();

        protected override void OnOpen()
        {
            RefreshTable();
            InspectItem(CurInspectIndex);
        }

        protected override void OnRemove()
        {
            TableInfos.Clear();
        }
    
        public void AddTableHeader(TableInfo info)
        {
            TableInfos.Add(info);
        }
    
        public void RefreshTable()
        {
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
            InspectItem(TableList.SelectedIndex);
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

        protected virtual PopupMenu BuildTableItemPopupMenu(T modData)
        {
            var menu = new PopupMenu();
            menu.AddItem("新建", TryAddData).enabled = Editable;
            menu.AddItem("复制", () =>
            {
                DataClipboard.CurCopyData = GetCopyData(modData);
            }).enabled = CanCopy(modData);
            menu.AddItem("粘贴", () =>
            {
                PasteCopyData(DataClipboard.CurCopyData);
                RefreshTable();
            }).enabled = Editable && CanPaste(DataClipboard.CurCopyData);
            menu.AddItem("另存为", () =>
            {
                PasteCopyDataAsNew(DataClipboard.CurCopyData);
                RefreshTable();
            }).enabled = Editable && CanPaste(DataClipboard.CurCopyData);
            menu.AddItem("删除", () => TryRemoveData(modData.Id)).enabled = Editable && modData != null;
            return menu;
        }

        private void InspectItem(int index)
        {
            Inspector.Editable = Editable;
            if (index >= 0 && index < GetDataCount())
            {
                BtnRemove.enabled = true;
                CurInspectIndex = index;
                var data = (IModData)TableList.GetData(index);
                Inspector.Clear();
                OnInspectItem(data);
                Inspector.Refresh();
            }
            else
            {
                BtnRemove.enabled = false;
                CurInspectIndex = -1;
                Inspector.Clear();
            }
            RefreshToolsBar();
        }

        private void RefreshToolsBar()
        {
            if (Editable)
            {
                BtnAdd.enabled = true;
                if(CurInspectIndex >= 0)
                    BtnRemove.enabled = true;
                else
                    BtnRemove.enabled = false;
            }
            else
            {
                BtnAdd.enabled = false;
                BtnRemove.enabled = false;
            }
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
                    ModDataTableDataList.AddItem(id);
                    RefreshTable();
                    var index = GetIndexById(id);
                    TableList.SelectedIndex = index;
                    TableList.MainView.m_list.ScrollToView(index, false);
                    InspectItem(TableList.SelectedIndex);
                }
            });
        }
        
        private void OnClickRemove()
        {
            var index = CurInspectIndex;
            if(index < 0 || index > GetDataCount())
                return;
            
            var id = ((IModData)TableList.GetData(CurInspectIndex)).Id;
            TryRemoveData(id);
        }

        protected void TryRemoveData(int id)
        {
            var index = CurInspectIndex;
            WindowConfirmDialog.CreateDialog("提示", $"即将删除ID为【{id}】的数据，是否确认删除？", true, () =>
            {
                ModDataTableDataList.RemoveItem(id);
                RefreshTable();
                TableList.SelectedIndex = index - 1;
                InspectItem(TableList.SelectedIndex);
            });
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
            Inspector.AddDrawer(drawer);
        }
        
        protected abstract void OnInspectItem(IModData data);
        
        public void ReloadInspector()
        {
            InspectItem(CurInspectIndex);
        }

        public virtual bool HasId(int id)
        {
            return GetIndexById(id) != -1;
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
        
        public virtual bool CanCopy(T data)
        {
            return data != null;
        }
        
        public CopyData GetCopyData(T data)
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
            if (ModDataTableDataList.HasId(copyData.Data.Id))
            {
                WindowConfirmDialog.CreateDialog("提示", "已经存取过该数据，是否覆盖？", true, () =>
                {
                    ModDataTableDataList.RemoveItem(copyData.Data.Id);
                    OnPaste(copyData, copyData.Data.Id);
                    RefreshTable();
                });
            }
            else
            {
                OnPaste(copyData, copyData.Data.Id);
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
                    OnPaste(copyData, id);
                    RefreshTable();
                }
            });
        }

        protected abstract void OnPaste(CopyData copyData, int targetId);

        public int GetDataCount()
        {
            return TableList.GetDataCount();
        }
        
        public IModData GetItem(int index)
        {
            return (IModData)TableList.GetData(index);
        }
    }
}