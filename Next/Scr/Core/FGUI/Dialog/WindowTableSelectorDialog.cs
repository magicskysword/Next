using System;
using System.Collections.Generic;
using System.Linq;
using FairyGUI;
using SkySwordKill.Next.FGUI.Component;
using SkySwordKill.NextFGUI.NextCore;
using SkySwordKill.NextModEditor.Mod.Data;
using UnityEngine;

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
    private bool _result;

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
        TableList.MultiSelect = _allowMulti;
        TableList.AllowClickToSelect = true;
        TableList.SetItemRenderer(OnItemRenderer);
        TableList.SetItemSelectedChanged(OnItemSelectedChanged);
        TableList.BindTable(_tableInfos, _tableDataList);

        RefreshConfirm();
        RefreshTipText();
    }
    
    protected override void OnKeyDown(EventContext context)
    {
        base.OnKeyDown(context);
        
        if (context.inputEvent.keyCode == KeyCode.Escape)
        {
            Cancel();
        }
    }
    
    private void OnSearch(string searchStr)
    {
        TableList.SearchItems(searchStr);
    }
    
    private void OnItemSelectedChanged()
    {
        _curIds.Clear();
        _curIds.AddRange(TableList.SelectedItems.Select(item => GetId((IModData)item)));
        
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
        _result = true;
        Hide();
    }

    private void Cancel()
    {
        _result = false;
        Hide();
    }
    
    protected override void OnHide()
    {
        base.OnHide();
        if(_result)
            _onConfirm?.Invoke(_curIds);
        else
            _onCancel?.Invoke();
    }
}