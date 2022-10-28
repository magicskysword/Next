using System;
using System.Collections.Generic;
using SkySwordKill.Next.FGUI;
using SkySwordKill.NextModEditor.Mod;

namespace SkySwordKill.NextFGUI.NextCore;

public partial class UI_ComNumberDrawer
{
    public bool Warning
    {
        get => GetController("warning").selectedIndex == 1;
        set => GetController("warning").selectedIndex = value ? 1 : 0;
    }

    public void SetEditable(bool value)
    {
        grayed = !value;
        m_inContent.editable = value;
        m_inContent.cursor = value ? "text" : string.Empty;
    }

    public void BindLongEndEdit(Action<long> onSetProperty)
    {
        m_inContent.BindLongEndEdit(value =>
        {
            onSetProperty?.Invoke(value);
            Warning = false;
        }, value =>
        {
            Warning = true;
        });
    }
    
    public void BindFloatEndEdit(Action<float> onSetProperty)
    {
        m_inContent.BindFloatEndEdit(value =>
        {
            onSetProperty?.Invoke(value);
            Warning = false;
        }, value =>
        {
            Warning = true;
        });
    }
    
    public void BindDoubleEndEdit(Action<double> onSetProperty)
    {
        m_inContent.BindDoubleEndEdit(value =>
        {
            onSetProperty?.Invoke(value);
            Warning = false;
        }, value =>
        {
            Warning = true;
        });
    }
    
    public void BindIntEndEdit(Action<int> onSetProperty)
    {
        m_inContent.BindIntEndEdit(value =>
        {
            onSetProperty?.Invoke(value);
            Warning = false;
        }, value =>
        {
            Warning = true;
        });
    }

    public void BindIntArrayEndEdit(Action<List<int>> onSetProperty)
    {
        m_inContent.BindIntArrayEndEdit(value =>
        {
            onSetProperty?.Invoke(value);
            Warning = false;
        }, value =>
        {
            Warning = true;
        });
    }
}