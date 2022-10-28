using System;
using System.Globalization;
using FairyGUI;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.Next.FGUI.Component;

public class CtlDoublePropertyDrawer : CtlPropertyDrawerBase
{
    private string _drawerName;
    private UI_ComNumberDrawer Drawer => (UI_ComNumberDrawer)Component;
    private Action<double> _setter;
    private Func<double> _getter;

    public CtlDoublePropertyDrawer(string drawerName,Action<double> setter,Func<double> getter)
    {
        _drawerName = drawerName;
        _setter = setter;
        _getter = getter;
    }
    
    protected override GComponent OnCreateCom()
    {
        var drawer = UI_ComNumberDrawer.CreateInstance();
        drawer.BindDoubleEndEdit(OnSetProperty);
        drawer.title = _drawerName;
        return drawer;
    }

    protected override void OnRefresh()
    {
        Drawer.m_inContent.text = OnGetProperty().ToString(CultureInfo.InvariantCulture);
    }

    protected override void SetDrawerEditable(bool value)
    {
        Drawer.SetEditable(value);
    }

    private void OnSetProperty(double value)
    {
        this.Record(new ValueChangedCommand<double>(OnGetProperty(),value, _setter));
        OnChanged?.Invoke();
    }

    private double OnGetProperty()
    {
        return _getter.Invoke();
    }
}