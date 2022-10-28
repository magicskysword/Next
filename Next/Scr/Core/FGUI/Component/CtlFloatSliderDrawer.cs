using System;
using FairyGUI;
using SkySwordKill.Next.Utils;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.Next.FGUI.Component;

public class CtlFloatSliderDrawer : CtlPropertyDrawerBase
{
    public UI_ComSliderDrawer SliderDrawer => (UI_ComSliderDrawer)Component;

    public CtlFloatSliderDrawer(string title, float min, float max, Action<float> setter, Func<float> getter)
    {
        _title = title;
        _min = min;
        _max = max;
        _setter = setter;
        _getter = getter;
    }

    private string _title;
    private float _min;
    private float _max;
    private Action<float> _setter;
    private Func<float> _getter;

    protected override GComponent OnCreateCom()
    {
        var drawer = UI_ComSliderDrawer.CreateInstance();
        drawer.m_txt_title.text = _title;
        drawer.m_txt_input.BindFloatEndEdit(OnInputEndEdit, OnInputEndEditFail);
        var slider = drawer.m_slider;
        slider.min = _min;
        slider.max = _max;
        slider.onGripTouchEnd.Add(OnSliderChanged);
        slider.wholeNumbers = false;
        slider.onChanged.Add(OnSliderValueChanged);
        return drawer;
    }

    private void OnInputEndEdit(float obj)
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
        OnSetProperty((float)value);
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

    protected float OnGetProperty()
    {
        return _getter();
    }
    
    protected void OnSetProperty(float value)
    {
        value = MathTools.Clamp(value, _min, _max);
        this.Record(new ValueChangedCommand<float>(OnGetProperty(), value, _setter));
        OnChanged?.Invoke();
    }

    protected override void SetDrawerEditable(bool value)
    {
        SliderDrawer.enabled = value;
    }
}