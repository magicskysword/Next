using System;
using System.Collections.Generic;
using FairyGUI;
using UnityEngine;

namespace SkySwordKill.NextFGUI.NextCore
{
    public partial class UI_ComTableList
    {
        public delegate void OnItemRenderer(int index,UI_ComTableRow row);

        private OnItemRenderer _itemRenderer;
        private List<TableInfo> _tableInfos;
        private Func<int, object> _getter;
        private Func<int> _counter;
        private EventCallback1 _onClickListItem;

        public void BindTable(List<TableInfo> tableInfos,Func<int,object> getter,Func<int> counter,OnItemRenderer itemRenderer,
            EventCallback1 onClickListItem)
        {
            _itemRenderer = itemRenderer;
            _getter = getter;
            _counter = counter;
            _tableInfos = tableInfos;
            _onClickListItem = onClickListItem;

            // 设置虚拟列表
            m_list.itemRenderer = ItemRenderer;
            m_list.onClickItem.Set(_onClickListItem);
            m_list.SetVirtual();
            
            // 绑定滚动
            m_list.scrollPane.onScroll.Add(OnListScroll);
            m_listHeader.scrollPane.onScroll.Add(OnListHeaderScroll);

            Refresh();
        }

        private void OnListHeaderScroll()
        {
            m_list.scrollPane.posX = m_listHeader.scrollPane.posX;
        }

        private void OnListScroll()
        {
            m_listHeader.scrollPane.posX = m_list.scrollPane.posX;
        }

        public void Refresh()
        {
            RefreshHeader(_tableInfos);
            RefreshRow();
            m_listHeader.scrollPane.posX = m_list.scrollPane.posX;
        }

        public void RefreshRow()
        {
            m_list.numItems = GetDataCount();
            m_list.scrollPane.SetContentSize(Mathf.Max(m_listHeader.width, m_list.scrollPane.viewWidth),
                m_list.scrollPane.contentHeight);
        }
        
        public void RefreshHeader(IEnumerable<TableInfo> infos)
        {
            m_listHeader.RemoveChildrenToPool();
            foreach (var info in infos)
            {
                var header = (UI_LabelTableGridHeader)m_listHeader.AddItemFromPool().asLabel;
                header.title = info.Name;
                header.width = info.Width;
                header.BindTableInfo(info,RefreshRow);
            }
        }
        
        public void RefreshRowAt(int index)
        {
            var childIndex = m_list.ItemIndexToChildIndex(index);
            if (childIndex >= 0 && childIndex < GetDataCount())
            {
                var row = (UI_ComTableRow)m_list.GetChildAt(childIndex);
                row.RefreshItem(_tableInfos, GetData(index), m_listHeader.width);
            }
        }
        
        private void ItemRenderer(int index, GObject item)
        {
            var row = (UI_ComTableRow)item;
            row.RefreshItem(_tableInfos, GetData(index), m_listHeader.width);
            _itemRenderer.Invoke(index, row);
        }

        public object GetData(int index)
        {
            return _getter.Invoke(index);
        }

        public int GetDataCount()
        {
            return _counter.Invoke();
        }
    }
}