using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FairyGUI;
using SkySwordKill.Next.FGUI.Dialog;
using SkySwordKill.Next.FGUI;
using SkySwordKill.NextModEditor.Mod;
using SkySwordKill.NextFGUI.NextCore;
using SkySwordKill.NextModEditor.Mod.Data;

namespace SkySwordKill.Next.FGUI.Component;

/// <summary>
/// 与CtlIntArrayBindTablePropertyDrawer相比
/// 多了一个bool函数判断是否是从表里获取和读取数据
/// </summary>
public class CtlIntArrayBindTableExPropertyDrawer : CtlPropertyDrawerBase
{
    private string _drawerName;
    private UI_ComNumberBindDataDrawer Drawer => (UI_ComNumberBindDataDrawer)Component;
    private Action<List<int>, bool> _setter;
    private Func<bool, List<int>> _getter;
    private Func<List<int>, string> _descGetter;
    private List<TableInfo> _tableInfos;
    private Func<List<IModData>> _dataListGetter;
    private Func<IModData, int> _idGetter;

    public CtlIntArrayBindTableExPropertyDrawer(string drawerName, Action<List<int>, bool> setter, Func<bool, List<int>> getter,
        Func<List<int>, string> descGetter, List<TableInfo> tableInfos, Func<List<IModData>> dataListGetter, 
        Func<IModData,int> idGetter = null)
    {
        _drawerName = drawerName;
        _setter = setter;
        _getter = getter;
        _descGetter = descGetter;
        _tableInfos = tableInfos;
        _dataListGetter = dataListGetter;
        _idGetter = idGetter;
    }

    protected override GComponent OnCreateCom()
    {
        var drawer = UI_ComNumberBindDataDrawer.CreateInstance();
        drawer.m_btnEdit.onClick.Set(OnClickEdit);
        drawer.BindEndMultiEdit(OnEndMultiDataEdit);
        drawer.title = _drawerName;
        return drawer;
    }

    private void OnEndMultiDataEdit(bool success, List<int> ids)
    {
        if (success)
        {
            OnSetProperty(ids, false);
            Refresh();
        }
        else
        {
            Drawer.m_txtDesc.text = "";
        }
    }

    protected override void OnRefresh()
    {
        Drawer.m_inContent.text = OnGetProperty(false).ToFormatString();
        Drawer.m_txtDesc.text = OnGetDesc();
    }

    protected override void SetDrawerEditable(bool value)
    {
        Drawer.SetEditable(value);
    }

    private void OnSetProperty(List<int> list, bool fromTable)
    {
        this.Record(new ValueChangedCommand<List<int>>(OnGetProperty(fromTable), list, 
            (v) => _setter.Invoke(list, fromTable)));
        OnChanged?.Invoke();
    }

    private List<int> OnGetProperty(bool fromTable)
    {
        return _getter.Invoke(fromTable);
    }

    private string OnGetDesc()
    {
        return _descGetter.Invoke(OnGetProperty(false));
    }

    private void OnClickEdit()
    {
        var ids = _getter.Invoke(true);

        WindowTableSelectorDialog.CreateDialog(_drawerName, _tableInfos,
            ids, true, _dataListGetter.Invoke(), true,
            onConfirm: list =>
            {
                OnSetProperty(list.ToList(), true);
                Refresh();
            },
            idGetter: _idGetter);
    }
}