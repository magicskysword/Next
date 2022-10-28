using System;
using FairyGUI;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.Next.FGUI.Component;

public class CtlLongPropertyDrawer : CtlPropertyDrawerBase
{
    private string _drawerName;
    private UI_ComNumberDrawer Drawer => (UI_ComNumberDrawer)Component;
    private Action<long> _setter;
    private Func<long> _getter;

    public CtlLongPropertyDrawer(string drawerName,Action<long> setter,Func<long> getter)
    {
        _drawerName = drawerName;
        _setter = setter;
        _getter = getter;
    }
    
    protected override GComponent OnCreateCom()
    {
        var drawer = UI_ComNumberDrawer.CreateInstance();
        drawer.BindLongEndEdit(OnSetProperty);
        drawer.title = _drawerName;
        return drawer;
    }

    protected override void OnRefresh()
    {
        Drawer.m_inContent.text = OnGetProperty().ToString();
    }
    
    private void OnSetProperty(long value)
    {
        this.Record(new ValueChangedCommand<long>(OnGetProperty(), value, _setter));
        OnChanged?.Invoke();
    }

    private long OnGetProperty()
    {
        return _getter.Invoke();
    }
    
    protected override void SetDrawerEditable(bool value)
    {
        Drawer.SetEditable(value);
    }
}