using System;
using System.Collections;
using System.Collections.Generic;
using FairyGUI;
using SkySwordKill.NextModEditor.Mod.Data;
using SkySwordKill.Next.FGUI;
using SkySwordKill.NextEditor.Mod;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.NextEditor.Drawer
{
    public class ModIntBindMultiDataPropertyDrawer : ModPropertyDrawer
{
    private string _drawerName;
    private UI_ComIntBindDataDrawer Drawer => (UI_ComIntBindDataDrawer)Component;
    private Action<List<int>> _setter;
    private Func<List<int>> _getter;
    private Func<List<int>, string> _descGetter;
    private List<TableInfo> _tableInfos;
    private List<IModData> _dataList;

    public ModIntBindMultiDataPropertyDrawer(string drawerName,Action<List<int>> setter,Func<List<int>> getter,
        Func<List<int>,string> descGetter,List<TableInfo> tableInfos,IList dataList)
    {
        _drawerName = drawerName;
        _setter = setter;
        _getter = getter;
        _descGetter = descGetter;
        _tableInfos = tableInfos;
        _dataList = new List<IModData>();
        foreach (var data in dataList)
        {
            _dataList.Add((IModData)data);
        }
    }
    
    protected override GComponent OnCreateCom()
    {
        var drawer = UI_ComIntBindDataDrawer.CreateInstance();
        drawer.m_btnEdit.onClick.Set(OnClickEdit);
        drawer.BindEndMultiEdit(OnEndMultiDataEdit);
        drawer.title = _drawerName;
        return drawer;
    }
    
    private void OnEndMultiDataEdit(bool success, List<int> ids)
    {
        if (success)
        {
            OnSetProperty(ids);
            Refresh();
        }
        else
        {
            Drawer.m_txtDesc.text = "";
        }
    }

    protected override void OnRefresh()
    {
        Drawer.m_inContent.text = OnGetProperty().ToFormatString();
        Drawer.m_txtDesc.text = OnGetDesc();
    }

    private void OnSetProperty(List<int> list)
    {
        _setter.Invoke(list);
        OnChanged?.Invoke();
    }

    private List<int> OnGetProperty()
    {
        return _getter.Invoke();
    }

    private string OnGetDesc()
    {
        return _descGetter.Invoke(OnGetProperty());
    }
    
    private void OnClickEdit()
    {
        var ids = _getter.Invoke();
        
        WindowSelectorDialog.CreateDialog(_drawerName,_tableInfos,
            ids,true,_dataList,true,
            list =>
            {
                OnSetProperty(list);
                Refresh();
            });
    }
}
}