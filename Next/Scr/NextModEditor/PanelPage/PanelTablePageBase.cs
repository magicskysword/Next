using System.Collections.Generic;
using FairyGUI;
using SkySwordKill.Next.FGUI.Component;
using SkySwordKill.NextEditor.Mod;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.NextEditor.PanelPage
{
    public abstract class PanelTablePageBase : PanelPageBase
    {
        public ModProject Project { get; set; }
        public List<TableInfo> TableInfos { get; } = new List<TableInfo>();
        public CtlTableEditor TableEditor { get; set; }
        public int CurInspectIndex { get; set; }
        public CtlTableList TableList => TableEditor.TableList;
        public CtlPropertyInspector Inspector => TableEditor.Inspector;

        protected override GObject OnAdd()
        {
            TableEditor = new CtlTableEditor(UI_ComTableEditor.CreateInstance());
            TableList.BindTable(TableInfos,GetData,GetDataCount,
                TableItemRenderer,OnClickTableItem);
            CurInspectIndex = -1;
            OnInit();
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
        
        private void TableItemRenderer(int index, UI_ComTableRow row)
        {
            if (index != TableList.SelectedIndex)
            {
                row.GetController("button").selectedIndex = 0;
            }
        }
    
        private void OnClickTableItem(EventContext context)
        {
            if(!context.inputEvent.isDoubleClick)
                return;
    
            CurInspectIndex = TableList.SelectedIndex;
            InspectItem(CurInspectIndex);
        }
    
        private void InspectItem(int index)
        {
            var data = index >= 0 ? GetData(index) : null;
            OnInspectItem(data);
        }
    
        protected abstract void OnInspectItem(object data);
    
        protected abstract object GetData(int index);
    
        protected abstract int GetDataCount();
    }
}