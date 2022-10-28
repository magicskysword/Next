using System;
using FairyGUI;
using SkySwordKill.Next.Utils;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.Next.FGUI.Component;

public class CtlDoubleSliderDrawer : CtlPropertyDrawerBase
{
    public UI_ComSliderDrawer SliderDrawer => (UI_ComSliderDrawer)Component;

    public CtlDoubleSliderDrawer(string title, double min, double max, Action<double> setter, Func<double> getter)
    {
        _title = title;
        _min = min;
        _max = max;
        _setter = setter;
        _getter = getter;
    }

    private string _title;
    private double _min;
    private double _max;
    private Action<double> _setter;
    private Func<double> _getter;

    protected override GComponent OnCreateCom()
    {
        var drawer = UI_ComSliderDrawer.CreateInstance();
        drawer.m_txt_title.text = _title;
        drawer.m_txt_input.BindDoubleEndEdit(OnInputEndEdit, OnInputEndEditFail);
        var slider = drawer.m_slider;
        slider.min = _min;
        slider.max = _max;
        slider.onGripTouchEnd.Add(OnSliderChanged);
        slider.wholeNumbers = false;
        slider.onChanged.Add(OnSliderValueChanged);
        return drawer;
    }

    private void OnInputEndEdit(double obj)
    {
        OnSetProperty(obj);
        Refresh();
    }

    private void OnInputEndEditFail(string obj)
    {
        Refresh();
    }

    private void OnSliderChanged(EventContext context)
    {
        var value = SliderDrawer.m_slider.value;
        OnSetProperty(value);
    }
    
    private void OnSliderValueChanged(EventContext context)
    {
        var value = SliderDrawer.m_slider.value;
        SliderDrawer.m_txt_input.text = value.ToString("F2");
    }

    protected override void OnRefresh()
    {
        var value = OnGetProperty();
        SliderDrawer.m_slider.value = value;
        SliderDrawer.m_txt_input.text = value.ToString("F2");
    }

    protected double OnGetProperty()
    {
        return _getter();
    }
    
    protected void OnSetProperty(double value)
    {
        value = MathTools.Clamp(value, _min, _max);
        this.Record(new ValueChangedCommand<double>(OnGetProperty(), value, _setter));
        OnChanged?.Invoke();
    }

    protected override void SetDrawerEditable(bool value)
    {
        SliderDrawer.enabled = value;
    }
}