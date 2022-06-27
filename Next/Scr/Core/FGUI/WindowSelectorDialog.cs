using System;
using System.Collections.Generic;
using FairyGUI;
using SkySwordKill.Next.FGUI.ComponentCtl;
using SkySwordKill.NextModEditor.Mod.Data;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.Next.FGUI
{
    public class WindowSelectorDialog : WindowDialogBase
{
    private string _title;
    private List<TableInfo> _tableInfos;
    private List<int> _curIds = new List<int>();
    private List<int> _selection = new List<int>();
    private List<IModData> _dataList;
    private bool _allowMulti;
    private Action<List<int>> _onConfirm;
    private Action _onCancel;
    private bool _allowEmpty;

    private WindowSelectorDialog() : base("NextCore", "WinSelectorDialog")
    {
    }

    public UI_WinSelectorDialog MainView => contentPane as UI_WinSelectorDialog;


    public static void CreateDialog(string title,List<TableInfo> tableInfos,IEnumerable<int> curIds,bool allowEmpty,
        List<IModData> dataList,bool allowMulti, Action<List<int>> onConfirm = null, Action onCancel = null)
    {
        var window = new WindowSelectorDialog();
        window._title = title;
        window._tableInfos = tableInfos;
        window._onConfirm = onConfirm;
        window._onCancel = onCancel;
        window._allowEmpty = allowEmpty;
        window._allowMulti = allowMulti;
        window._curIds.AddRange(curIds);
        window._dataList = dataList;
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

        var viewMTable = new CtlTableList(MainView.m_table.As<UI_ComTableList>());
        viewMTable.BindTable(_tableInfos, i => _dataList[i], () => _dataList.Count,
            OnItemRenderer, _ => OnClickListItem());

        if (_allowMulti)
        {
            viewMTable.MainView.m_list.selectionMode = ListSelectionMode.Multiple_SingleClick;
        }
        else
        {
            viewMTable.MainView.m_list.selectionMode = ListSelectionMode.Single;
            
        }
        
        foreach (var curId in _curIds)
        {
            int idIndex = -1;
            for (var index = 0; index < _dataList.Count; index++)
            {
                if (_dataList[index].ID != curId)
                    continue;
                idIndex = index;
                break;
            }

            if (idIndex >= 0)
                viewMTable.MainView.m_list.AddSelection(idIndex, false);
        }
        viewMTable.MainView.m_list.GetSelection(_selection);

        RefreshConfirm();
        RefreshTipText();
    }

    private void OnClickListItem()
    {
        _selection.Clear();
        MainView.m_table.As<UI_ComTableList>().m_list.GetSelection(_selection);
        RefreshTipText();
        RefreshConfirm();
    }

    private void RefreshTipText()
    {
        if (_selection.Count == 0)
        {
            MainView.m_txtTips.text = "未选择列表项。";
        }
        else
        {
            MainView.m_txtTips.text = $"当前已选择 {_selection.Count}项。";
        }
    }

    private void OnItemRenderer(int index,UI_ComTableRow row)
    {
        if (!_selection.Contains(index))
        {
            row.GetController("button").selectedIndex = 0;
        }
    }

    private void RefreshConfirm()
    {
        if (!_allowEmpty)
        {
            MainView.m_btnOk.enabled = _selection.Count > 0;
        }
        else
        {
            MainView.m_btnOk.enabled = true;
        }
    }
    
    private void Confirm()
    {
        _curIds.Clear();
        foreach (var idIndex in _selection)
        {
            _curIds.Add(_dataList[idIndex].ID);
        }
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
}