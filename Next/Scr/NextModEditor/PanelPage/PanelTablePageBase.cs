using System.Collections.Generic;
using FairyGUI;
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

        public void AddItem(int id)
        {
            _list.Add(new T()
            {
                Id = id
            });
            _list.ModSort();
        }

        public void RemoveItem(int id)
        {
            var index = _list.FindIndex(data => data.Id == id);
            if (index >= 0)
                _list.RemoveAt(index);
            _list.ModSort();
        }
    }
    
    public abstract class PanelTablePageBase<T> : PanelPageBase where T : IModData, new()
    {
        protected PanelTablePageBase(string name, ModWorkshop mod, ModProject project) : base(name)
        {
            Mod = mod;
            Project = project;
        }
        
        public ModWorkshop Mod { get; set; }
        public ModProject Project { get; set; }
        public List<TableInfo> TableInfos { get; } = new List<TableInfo>();
        public CtlTableEditor TableEditor { get; set; }
        public int CurInspectIndex { get; set; }
        public CtlTableList TableList => TableEditor.TableList;
        public CtlPropertyInspector Inspector => TableEditor.Inspector;
        public GButton BtnRemove { get; set; }
        public abstract ModDataTableDataList<T> ModDataTableDataList { get; set; }
        public bool IsInit { get; private set; }

        protected override GObject OnAdd()
        {
            if(!IsInit)
                OnInit();
            TableEditor = new CtlTableEditor(UI_ComTableEditor.CreateInstance());
            TableList.SetItemRenderer(TableItemRenderer);
            TableList.SetClickItem(OnClickTableItem);
            TableList.SetRightClickItem(OnRightClickTableItem);
            TableList.SetTableRightClick(() => OnPopupMenu(null));
            TableList.BindTable(TableInfos, ModDataTableDataList);
            
            TableEditor.ToolsBar.AddToolBtn("ui://NextCore/icon_add", "新建数据", OnClickAdd);
            BtnRemove = TableEditor.ToolsBar.AddToolBtn("ui://NextCore/icon_minus", "移除数据", OnClickRemove);
            TableEditor.ToolsBar.AddToolSearch(OnSearchData);
            
            CurInspectIndex = -1;
            BtnRemove.enabled = false;
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
            OnPopupMenu((IModData)data);
        }

        protected void OnPopupMenu(IModData modData)
        {
            var popupMenu = BuildTableItemPopupMenu(modData);
            popupMenu.Show(null, PopupDirection.Down);
        }

        protected virtual PopupMenu BuildTableItemPopupMenu(IModData modData)
        {
            var menu = new PopupMenu();
            menu.AddItem("新建数据", TryAddData);
            menu.AddItem("移除数据", () => TryRemoveData(modData.Id)).enabled = modData != null;
            return menu;
        }
    
        private void InspectItem(int index)
        {
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