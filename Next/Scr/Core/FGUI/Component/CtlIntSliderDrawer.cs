using System;
using FairyGUI;
using SkySwordKill.Next.Utils;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.Next.FGUI.Component;

public class CtlIntSliderDrawer : CtlPropertyDrawerBase
{
    public UI_ComSliderDrawer SliderDrawer => (UI_ComSliderDrawer)Component;

    public CtlIntSliderDrawer(string title, int min, int max, Action<int> setter, Func<int> getter)
    {
        _title = title;
        _min = min;
        _max = max;
        _setter = setter;
        _getter = getter;
    }

    private string _title;
    private int _min;
    private int _max;
    private Action<int> _setter;
    private Func<int> _getter;

    protected override GComponent OnCreateCom()
    {
        var drawer = UI_ComSliderDrawer.CreateInstance();
        drawer.m_txt_title.text = _title;
        drawer.m_txt_input.BindIntEndEdit(OnInputEndEdit, OnInputEndEditFail);
        var slider = drawer.m_slider;
        slider.min = _min;
        slider.max = _max;
        slider.onGripTouchEnd.Add(OnSliderChanged);
        slider.wholeNumbers = true;
        slider.onChanged.Add(OnSliderValueChanged);
        return drawer;
    }

    private void OnInputEndEdit(int obj)
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
        OnSetProperty((int)value);
    }
    
    private void OnSliderValueChanged(EventContext context)
    {
        var value = (int)SliderDrawer.m_slider.value;
        SliderDrawer.m_txt_input.text = value.ToString();
    }

    protected override void OnRefresh()
    {
        var value = OnGetProperty();
        SliderDrawer.m_slider.value = value;
        SliderDrawer.m_txt_input.text = value.ToString();
    }

    protected int OnGetProperty()
    {
        return _getter();
    }
    
    protected void OnSetProperty(int value)
    {
        value = MathTools.Clamp(value, _min, _max);
        this.Record(new ValueChangedCommand<int>(OnGetProperty(), value, _setter));
        OnChanged?.Invoke();
    }

    protected override void SetDrawerEditable(bool value)
    {
        SliderDrawer.enabled = value;
    }
}