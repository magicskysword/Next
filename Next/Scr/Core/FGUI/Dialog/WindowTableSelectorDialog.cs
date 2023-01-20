using System;
using System.Collections.Generic;
using FairyGUI;
using SkySwordKill.Next.FGUI.Component;
using SkySwordKill.NextFGUI.NextCore;
using SkySwordKill.NextModEditor.Mod.Data;

namespace SkySwordKill.Next.FGUI.Dialog;

public class WindowTableSelectorDialog : WindowDialogBase
{

    private string _title;
    private List<TableInfo> _tableInfos = new List<TableInfo>();
    private List<int> _curIds = new List<int>();
    private TableDataList<IModData> _tableDataList;
    private bool _allowMulti;
    private Action<List<int>> _onConfirm;
    private Func<IModData, int> _idGetter;
    private Action _onCancel;
    private bool _allowEmpty;

    private WindowTableSelectorDialog() : base("NextCore", "WinTableDialog")
    {
    }

    public UI_WinTableDialog MainView => contentPane as UI_WinTableDialog;
    public CtlToolsBar ToolsBar { get; set; }
    public CtlTableList TableList { get; set; }


    public static void CreateDialog(string title, List<TableInfo> tableInfos, IEnumerable<int> curIds,
        bool allowEmpty,
        List<IModData> dataList, bool allowMulti, Action<List<int>> onConfirm = null, Action onCancel = null, 
        Func<IModData, int> idGetter = null)
    {
        var window = new WindowTableSelectorDialog();
        window._title = title;
        window._tableInfos.AddRange(tableInfos);
        window._onConfirm = onConfirm;
        window._onCancel = onCancel;
        window._idGetter = idGetter;
        window._allowEmpty = allowEmpty;
        window._allowMulti = allowMulti;
        if (curIds != null)
            window._curIds.AddRange(curIds);
        window._tableDataList = new TableDataList<IModData>(dataList);
        window.modal = true;

        window.Show();
    }

    protected override void OnInit()
    {
        base.OnInit();
        MainView.m_frame.title = _title;
        closeButton.onClick.Set(Cancel);
        MainView.m_closeButton.onClick.Set(Cancel);
        MainView.m_btnOk.onClick.Set(Confirm);

        ToolsBar = new CtlToolsBar(MainView.m_toolsBar);
        ToolsBar.AddToolSearch(OnSearch);

        TableList = new CtlTableList(MainView.m_table.As<UI_ComTableList>());
        TableList.SetClickItem(OnClickListItem);
        TableList.SetItemRenderer(OnItemRenderer);
        TableList.BindTable(_tableInfos, _tableDataList);
            
        if (_allowMulti)
        {
            TableList.SetSelectionMode(ListSelectionMode.None);
        }
        else
        {
            TableList.SetSelectionMode(ListSelectionMode.Single);
        }

        RefreshConfirm();
        RefreshTipText();
    }

    private void OnSearch(string searchStr)
    {
        TableList.SearchItems(searchStr);
    }

    private void OnClickListItem(int index, object o)
    {
        var modData = (IModData)o;
        if(_allowMulti)
        {
            if (_curIds.Contains(GetId(modData)))
            {
                _curIds.Remove(GetId(modData));
            }
            else
            {
                _curIds.Add(GetId(modData));
            }
            TableList.RefreshRows();
        }
        else
        {
            _curIds.Clear();
            _curIds.Add(GetId(modData));
        }

        RefreshTipText();
        RefreshConfirm();
    }

    public int GetId(IModData data)
    {
        if (_idGetter != null)
        {
            return _idGetter.Invoke(data);
        }

        return data.Id;
    }
        
    private void OnItemRenderer(int index, UI_ComTableRow row, object o)
    {
        var modData = (IModData)o;
        row.selected = _curIds.Contains(GetId(modData));
        row.GetController("button").selectedIndex = row.selected ? 1 : 0;
    }

    private void RefreshTipText()
    {
        if (_curIds.Count == 0)
        {
            MainView.m_txtTips.text = "未选择列表项。";
        }
        else
        {
            MainView.m_txtTips.text = $"当前已选择 {_curIds.Count}项。";
        }
    }

    private void RefreshConfirm()
    {
        if (!_allowEmpty)
        {
            MainView.m_btnOk.enabled = _curIds.Count > 0;
        }
        else
        {
            MainView.m_btnOk.enabled = true;
        }
    }

    private void Confirm()
    {
        _curIds.Sort();
        _onConfirm?.Invoke(_curIds);
        Hide();
    }

    private void Cancel()
    {
        _onCancel?.Invoke();
        Hide();
    }
}