using System;
using System.Collections.Generic;
using FairyGUI;
using SkySwordKill.NextFGUI.NextCore;
using UnityEngine;

namespace SkySwordKill.Next.FGUI.Component;

public delegate void OnTableItemRenderer(int index, UI_ComTableRow row, object data);
public delegate void OnClickTableItem(int index, object data);
    
public interface ITableDataList
{
    int Count { get; }
    object GetObj(int index);
}

public class TableDataList<T> : ITableDataList
{
    public TableDataList(List<T> list)
    {
        _list = list;
    }

    protected List<T> _list;
            
    public int Count => _list.Count;
    public object GetObj(int index)
    {
        return _list[index];
    }
        
    public T GetItem(int index)
    {
        return _list[index];
    }
}
    
public class CtlTableList
{
        

    public CtlTableList(UI_ComTableList com)
    {
        MainView = com;
    }
        
    public UI_ComTableList MainView { get; set; }

    public int SelectedIndex
    {
        get => MainView.m_list.selectedIndex;
        set => MainView.m_list.selectedIndex = value;
    }
        
    private OnTableItemRenderer _tableItemRenderer;
    private OnClickTableItem _onClickTableItem;
    private OnClickTableItem _onRightClickTableItem;
    private Action _onRightClickTableList;
    private List<TableInfo> _tableInfos;
    private ITableDataList _tableDataList;
    private string _searchText;

    private List<object> _searchList = new List<object>();
    private bool _isSearching;

    public void BindTable(List<TableInfo> tableInfos, ITableDataList tableDataList)
    {
        _tableInfos = tableInfos;
        _tableDataList = tableDataList;

        // 设置虚拟列表
        MainView.m_list.itemRenderer = ItemRenderer;
        MainView.m_list.onClickItem.Set(OnClickItem);
        MainView.m_list.onRightClickItem.Set(OnRightClickItem);
        MainView.m_list.opaque = false;
        MainView.m_list.SetVirtual();
            
        // 绑定背景
        MainView.m_bgList.onRightClick.Set(OnRightClickTableList);

        // 绑定滚动
        MainView.m_list.scrollPane.onScroll.Add(OnListScroll);
        MainView.m_listHeader.scrollPane.onScroll.Add(OnListHeaderScroll);
            
        // 绑定大小变动
        MainView.onSizeChanged.Add(OnSizeChanged);

        _searchText = "";
        _isSearching = false;

        Refresh();
    }

    private void OnSizeChanged()
    {
        Refresh();
    }

    public void SetItemRenderer(OnTableItemRenderer renderer)
    {
        _tableItemRenderer = renderer;
    }
        
    public void SetClickItem(OnClickTableItem clickItem)
    {
        _onClickTableItem = clickItem;
    }
        
    public void SetRightClickItem(OnClickTableItem rightClickItem)
    {
        _onRightClickTableItem = rightClickItem;
    }
        
    public void SetTableRightClick(Action action)
    {
        _onRightClickTableList = action;
    }

    public void SetSelectionMode(ListSelectionMode mode)
    {
        MainView.m_list.selectionMode = mode;
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
        ResetDataList();
        RefreshHeader(_tableInfos);
        RefreshRows();
        MainView.m_listHeader.scrollPane.posX = MainView.m_list.scrollPane.posX;
    }

    public void RefreshRows()
    {
        MainView.m_list.numItems = GetDataCount();
            
        var width = Mathf.Max(MainView.m_listHeader.scrollPane.contentWidth, 
            MainView.m_list.scrollPane.viewWidth);
        var height = MainView.m_list.scrollPane.contentHeight;
        MainView.m_list.scrollPane.SetContentSize(width, height);
    }

    public void RefreshHeader(IEnumerable<TableInfo> infos)
    {
        MainView.m_listHeader.RemoveChildrenToPool();
        foreach (var info in infos)
        {
            var header = (UI_LabelTableGridHeader)MainView.m_listHeader.AddItemFromPool().asLabel;
            header.title = info.Name;
            header.width = info.Width;
            header.BindTableInfo(info, RefreshRows);
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

    public void SearchItems(string searchText)
    {
        _searchText = searchText;
        Refresh();
    }

    private void ItemRenderer(int index, GObject item)
    {
        var row = (UI_ComTableRow)item;
        row.RefreshItem(_tableInfos, GetData(index), MainView.m_listHeader.width);
        if (MainView.m_list.selectionMode == ListSelectionMode.Single && index != SelectedIndex)
        {
            row.GetController("button").selectedIndex = 0;
        }
        _tableItemRenderer?.Invoke(index, row, row.data);
    }
        
    private void OnClickItem(EventContext context)
    {
        var index = MainView.m_list.GetChildIndex((GObject)context.data);
        int itemIndex = MainView.m_list.ChildIndexToItemIndex(index);
        _onClickTableItem?.Invoke(itemIndex, GetData(itemIndex));
    }
        
    private void OnRightClickItem(EventContext context)
    {
        var index = MainView.m_list.GetChildIndex((GObject)context.data);
        int itemIndex = MainView.m_list.ChildIndexToItemIndex(index);
        _onRightClickTableItem?.Invoke(itemIndex, GetData(itemIndex));
    }

    private void OnRightClickTableList(EventContext context)
    {
        _onRightClickTableList?.Invoke();
    }

    private void ResetDataList()
    {
        _searchList.Clear();
        if (string.IsNullOrEmpty(_searchText))
        {
            _isSearching = false;
        }
        else
        {
            for (int i = 0; i < _tableDataList.Count; i++)
            {
                var data = _tableDataList.GetObj(i);
                if (CheckSearch(data, _searchText))
                {
                    _searchList.Add(data);
                }
            }
            _isSearching = true;
        }
    }

    private bool CheckSearch(object data, string tagText)
    {
        foreach (var info in _tableInfos)
        {
            var str = info.Getter(data);
            if (str.IndexOf(tagText, StringComparison.OrdinalIgnoreCase) >= 0)
                return true;
        }

        return false;
    }

    public object GetData(int index)
    {
        if(_isSearching)
        {
            return _searchList[index];
        }
        return _tableDataList.GetObj(index);
    }

    public int GetDataCount()
    {
        if (_isSearching)
        {
            return _searchList.Count;
        }
        return _tableDataList.Count;
    }
}