using System;
using System.Collections.Generic;
using System.Linq;
using FairyGUI;
using SkySwordKill.NextFGUI.NextCore;
using SkySwordKill.NextModEditor.Mod;
using SkySwordKill.NextModEditor.Mod.Data;
using UnityEngine;

namespace SkySwordKill.Next.FGUI.Component;

public delegate void OnTableItemRenderer(int index, UI_ComTableRow row, object data);

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
    
    private SortedSet<int> _selectionArea = new SortedSet<int>();
    private OnTableItemRenderer _tableItemRenderer;
    private Action<EventContext> _onClickTableItem;
    private Action _onRightClickTableItem;
    private Action _onRightClickTableList;
    private Action _onItemSelectedChanged;
    private List<TableInfo> _tableInfos;
    private ITableDataList _tableDataList;
    private string _searchText;

    private List<object> _searchList = new List<object>();
    private bool _isSearching;
    
    public UI_ComTableList MainView { get; set; }

    /// <summary>
    /// 选中的索引
    /// </summary>
    public int SelectedIndex
    {
        get
        {
            // 过滤一遍
            var index = _selectionArea.Count > 0 ? _selectionArea.First() : -1;
            if (index >= _tableDataList.Count)
                return -1;
            
            return index;
        }
        set
        {
            _selectionArea.Clear();
            _selectionArea.Add(value);
            OnItemSelectedChanged();
        }
    }
    
    /// <summary>
    /// 选中区域，对获取的数据做筛选
    /// </summary>
    public IReadOnlyCollection<int> SelectionArea
    {
        get
        {
            var list = new List<int>();
            foreach (var index in _selectionArea)
            {
                if(index < _tableDataList.Count && index >= 0)
                    list.Add(index);
            }
            return list;
        }
        set
        {
            _selectionArea.Clear();
            foreach (var index in value)
            {
                if(index < _tableDataList.Count && index >= 0)
                    _selectionArea.Add(index);
            }
            OnItemSelectedChanged();
        }
    }

    /// <summary>
    /// 是否允许多选
    /// </summary>
    public bool MultiSelect { get; set; } = false;

    public object SelectedItem
    {
        get
        {
            var index = SelectedIndex;
            if (index == -1)
                return null;
            
            return GetData(index);
        }
    }

    public IEnumerable<object> SelectedItems
    {
        get
        {
            var indexes = SelectionArea;
            if (indexes.Count == 0)
                return Array.Empty<object>();
            
            return indexes.Select(GetData);
        }
    }

    public bool AllowClickToSelect { get; set; } = false;

    public void BindTable(List<TableInfo> tableInfos, ITableDataList tableDataList)
    {
        _tableInfos = tableInfos;
        _tableDataList = tableDataList;

        // 设置虚拟列表
        MainView.m_list.itemRenderer = ItemRenderer;
        MainView.m_list.onClickItem.Set(OnClickItem);
        MainView.m_list.onRightClickItem.Set(OnRightClickItem);
        MainView.m_list.opaque = false;
        MainView.m_list.selectionMode = ListSelectionMode.None;
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
        
    public void SetClickItem(Action<EventContext> clickItem)
    {
        _onClickTableItem = clickItem;
    }
        
    public void SetRightClickItem(Action rightClickItem)
    {
        _onRightClickTableItem = rightClickItem;
    }
        
    public void SetTableRightClick(Action callback)
    {
        _onRightClickTableList = callback;
    }
    
    public void SetItemSelectedChanged(Action callback)
    {
        _onItemSelectedChanged = callback;
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
        MainView.m_list.RefreshVirtualList();
            
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
        _selectionArea.Clear();
        Refresh();
    }

    private void ItemRenderer(int index, GObject item)
    {
        var row = (UI_ComTableRow)item;
        row.RefreshItem(_tableInfos, GetData(index), MainView.m_listHeader.width);
        if (!_selectionArea.Contains(index))
        {
            row.selected = false;
        }
        else
        {
            row.selected = true;
        }
        _tableItemRenderer?.Invoke(index, row, row.data);
    }
    
    private void OnClickItem(EventContext context)
    {
        var index = MainView.m_list.GetChildIndex((GObject)context.data);
        int itemIndex = MainView.m_list.ChildIndexToItemIndex(index);
        
        if(itemIndex == SelectedIndex)
            return;
        
        if(itemIndex == -1)
            return;

        if (MultiSelect)
        {
            if (context.inputEvent.shift)
            {
                //已选中第一个时，则选中第一个与当前的区间
                if (SelectedIndex != -1)
                {
                    var min = Mathf.Min(itemIndex, SelectedIndex);
                    var max = Mathf.Max(itemIndex, SelectedIndex);
                    for (int i = min; i <= max; i++)
                    {
                        _selectionArea.Add(i);
                    }
                }
                else
                {
                    _selectionArea.Add(itemIndex);
                }
                OnItemSelectedChanged();
            }
            else if (context.inputEvent.ctrlOrCmd || AllowClickToSelect)
            {
                if (SelectionArea.Contains(itemIndex))
                {
                    _selectionArea.Remove(itemIndex);
                }
                else
                {
                    _selectionArea.Add(itemIndex);
                }
                
                OnItemSelectedChanged();
                
                // 单击选中状态下允许点击触发事件
                if (AllowClickToSelect && !context.inputEvent.ctrlOrCmd)
                {
                    _onClickTableItem?.Invoke(context);
                }
            }
            else
            {
                SelectedIndex = itemIndex;
                _onClickTableItem?.Invoke(context);
                
                OnItemSelectedChanged();
            }
        }
        else
        {
            SelectedIndex = itemIndex;
            _onClickTableItem?.Invoke(context);
            
            OnItemSelectedChanged();
        }
    }
        
    private void OnRightClickItem(EventContext context)
    {
        var index = MainView.m_list.GetChildIndex((GObject)context.data);
        int itemIndex = MainView.m_list.ChildIndexToItemIndex(index);

        if (!SelectionArea.Contains(itemIndex))
        {
            SelectedIndex = itemIndex;
            OnItemSelectedChanged();
        }
        
        _onRightClickTableItem?.Invoke();
    }

    private void OnRightClickTableList(EventContext context)
    {
        _onRightClickTableList?.Invoke();
    }

    private void OnItemSelectedChanged()
    {
        Refresh();
        _onItemSelectedChanged?.Invoke();
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

    public void ScrollToView(int tagIndex, bool ani = false, bool setFirst = false)
    {
        MainView.m_list.ScrollToView(tagIndex, ani, setFirst);
    }
}