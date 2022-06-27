using System;
using System.Collections.Generic;
using FairyGUI;
using SkySwordKill.NextFGUI.NextCore;
using UnityEngine;

namespace SkySwordKill.Next.FGUI.ComponentCtl
{
    public class CtlTableList
    {
        public delegate void OnItemRenderer(int index, UI_ComTableRow row);

        public CtlTableList(UI_ComTableList com)
        {
            MainView = com;
        }
        
        public UI_ComTableList MainView { get; set; }

        public int SelectedIndex => MainView.m_list.selectedIndex;
        
        private OnItemRenderer _itemRenderer;
        private List<TableInfo> _tableInfos;
        private Func<int, object> _getter;
        private Func<int> _counter;
        private EventCallback1 _onClickListItem;

        public void BindTable(List<TableInfo> tableInfos, Func<int, object> getter, Func<int> counter,
            OnItemRenderer itemRenderer,
            EventCallback1 onClickListItem)
        {
            _itemRenderer = itemRenderer;
            _getter = getter;
            _counter = counter;
            _tableInfos = tableInfos;
            _onClickListItem = onClickListItem;

            // 设置虚拟列表
            MainView.m_list.itemRenderer = ItemRenderer;
            MainView.m_list.onClickItem.Set(_onClickListItem);
            MainView.m_list.SetVirtual();

            // 绑定滚动
            MainView.m_list.scrollPane.onScroll.Add(OnListScroll);
            MainView.m_listHeader.scrollPane.onScroll.Add(OnListHeaderScroll);

            Refresh();
        }

        private void OnListHeaderScroll()
        {
            MainView.m_list.scrollPane.posX = MainView.m_listHeader.scrollPane.posX;
        }

        private void OnListScroll()
        {
            MainView.m_listHeader.scrollPane.posX = MainView.m_list.scrollPane.posX;
        }

        public void Refresh()
        {
            RefreshHeader(_tableInfos);
            RefreshRow();
            MainView.m_listHeader.scrollPane.posX = MainView.m_list.scrollPane.posX;
        }

        public void RefreshRow()
        {
            MainView.m_list.numItems = GetDataCount();
            MainView.m_list.scrollPane.SetContentSize(Mathf.Max(MainView.m_listHeader.width, 
                    MainView.m_list.scrollPane.viewWidth),
                MainView. m_list.scrollPane.contentHeight);
        }

        public void RefreshHeader(IEnumerable<TableInfo> infos)
        {
            MainView.m_listHeader.RemoveChildrenToPool();
            foreach (var info in infos)
            {
                var header = (UI_LabelTableGridHeader)MainView.m_listHeader.AddItemFromPool().asLabel;
                header.title = info.Name;
                header.width = info.Width;
                header.BindTableInfo(info, RefreshRow);
            }
        }

        public void RefreshRowAt(int index)
        {
            var childIndex = MainView.m_list.ItemIndexToChildIndex(index);
            if (childIndex >= 0 && childIndex < GetDataCount())
            {
                var row = (UI_ComTableRow)MainView.m_list.GetChildAt(childIndex);
                row.RefreshItem(_tableInfos, GetData(index), MainView.m_listHeader.width);
            }
        }

        private void ItemRenderer(int index, GObject item)
        {
            var row = (UI_ComTableRow)item;
            row.RefreshItem(_tableInfos, GetData(index), MainView.m_listHeader.width);
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