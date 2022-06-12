using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FairyGUI;
using SkySwordKill.NextModEditor.Mod.Data;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.Next.FGUI.ComponentCtl
{
    public class CtlBindDataCtlPropertyDrawer : CtlPropertyDrawerBase
{
    private string _drawerName;
    private UI_ComIntBindDataDrawer Drawer => (UI_ComIntBindDataDrawer)Component;
    private Action<int> _setter;
    private Func<int> _getter;
    private Func<IModData, string> _descGetter;
    private List<TableInfo> _tableInfos;
    private List<IModData> _dataList;

    public CtlBindDataCtlPropertyDrawer(string drawerName,Action<int> setter,Func<int> getter,
        Func<IModData,string> descGetter,List<TableInfo> tableInfos,IList dataList)
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
        drawer.BindEndEdit(OnSetProperty);
        drawer.title = _drawerName;
        return drawer;
    }

    protected override void OnRefresh()
    {
        Drawer.m_inContent.text = OnGetProperty().ToString();
        Drawer.m_txtDesc.text = OnGetDesc();
    }

    private void OnSetProperty(int value)
    {
        _setter.Invoke(value);
        OnChanged?.Invoke();
    }

    private int OnGetProperty()
    {
        return _getter.Invoke();
    }

    private string OnGetDesc()
    {
        var id = _getter.Invoke();
        var data = _dataList.FirstOrDefault(modData => modData.ID == id);

        if (data == null)
        {
            return "";
        }
        
        return _descGetter.Invoke(data);
    }
    
    private void OnClickEdit()
    {
        WindowSelectorDialog.CreateDialog(_drawerName,_tableInfos,
            new []{OnGetProperty()},false,_dataList,false,
            list =>
            {
                OnSetProperty(list.FirstOrDefault());
                Refresh();
            });
    }
}
}