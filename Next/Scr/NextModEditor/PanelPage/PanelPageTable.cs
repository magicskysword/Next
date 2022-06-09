using System.Collections.Generic;
using FairyGUI;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.NextEditor.PanelPage
{
    public abstract class PanelPageTable : PanelPageBase
    {
        public List<TableInfo> TableInfos { get; } = new List<TableInfo>();
        public UI_ComTableList TableList { get; set; }
        public int CurInspectIndex { get; set; }
    
        public override void OnAdd()
        {
            TableList = UI_ComTableList.CreateInstance();
            TableList.BindTable(TableInfos,GetData,GetDataCount,
                TableItemRenderer,OnClickTableItem);
            Content = TableList;
            CurInspectIndex = -1;
        }
    
        public override void OnOpen()
        {
            RefreshTable();
            InspectItem(CurInspectIndex);
        }
    
        public override void OnRemove()
        {
            Content.Dispose();
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
            if (index != TableList.m_list.selectedIndex)
            {
                row.GetController("button").selectedIndex = 0;
            }
        }
    
        private void OnClickTableItem(EventContext context)
        {
            if(!context.inputEvent.isDoubleClick)
                return;
    
            CurInspectIndex = TableList.m_list.selectedIndex;
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