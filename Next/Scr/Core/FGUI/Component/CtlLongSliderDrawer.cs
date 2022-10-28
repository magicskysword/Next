using System;
using FairyGUI;
using SkySwordKill.Next.Utils;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.Next.FGUI.Component;

public class CtlLongSliderDrawer : CtlPropertyDrawerBase
{
    public UI_ComSliderDrawer SliderDrawer => (UI_ComSliderDrawer)Component;

    public CtlLongSliderDrawer(string title, long min, long max, Action<long> setter, Func<long> getter)
    {
        _title = title;
        _min = min;
        _max = max;
        _setter = setter;
        _getter = getter;
    }

    private string _title;
    private long _min;
    private long _max;
    private Action<long> _setter;
    private Func<long> _getter;

    protected override GComponent OnCreateCom()
    {
        var drawer = UI_ComSliderDrawer.CreateInstance();
        drawer.m_txt_title.text = _title;
        drawer.m_txt_input.BindLongEndEdit(OnInputEndEdit, OnInputEndEditFail);
        var slider = drawer.m_slider;
        slider.min = _min;
        slider.max = _max;
        slider.onGripTouchEnd.Add(OnSliderChanged);
        slider.wholeNumbers = true;
        slider.onChanged.Add(OnSliderValueChanged);
        return drawer;
    }

    private void OnInputEndEdit(long obj)
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
        var value = (long)SliderDrawer.m_slider.value;
        SliderDrawer.m_txt_input.text = value.ToString();
    }

    protected override void OnRefresh()
    {
        var value = OnGetProperty();
        SliderDrawer.m_slider.value = value;
        SliderDrawer.m_txt_input.text = value.ToString();
    }

    protected long OnGetProperty()
    {
        return _getter();
    }
    
    protected void OnSetProperty(long value)
    {
        value = MathTools.Clamp(value, _min, _max);
        this.Record(new ValueChangedCommand<long>(OnGetProperty(), value, _setter));
        OnChanged?.Invoke();
    }

    protected override void SetDrawerEditable(bool value)
    {
        SliderDrawer.enabled = value;
    }
}