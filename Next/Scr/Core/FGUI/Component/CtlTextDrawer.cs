using System;
using FairyGUI;

namespace SkySwordKill.Next.FGUI.Component;

public class CtlTextDrawer : CtlPropertyDrawerBase
{
    public CtlTextDrawer(string text)
    {
        _text = text;
    }
    
    public CtlTextDrawer(string text,int fontSize) : this(text)
    {
        _fontSize = fontSize;
    }

    protected int _fontSize = Int32.MinValue;
    protected string _text;

    protected override GComponent OnCreateCom()
    {
        var label = UIPackage.CreateObject(FGUIManager.PKG_NEXT_CORE, "ComTextDrawer").asLabel;
        label.text = _text;
        if (_fontSize != Int32.MinValue)
        {
            label.titleFontSize = _fontSize;
        }
        return label;
    }

    protected override void SetDrawerEditable(bool value)
    {
        
    }

    public virtual void SetText(string text)
    {
        _text = text;
        Component.text = text;
    }
}