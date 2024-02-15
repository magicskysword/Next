using System;
using FairyGUI;

namespace SkySwordKill.Next.FGUI.Component;

public class CtlButtonDrawer : CtlTextDrawer
{
    protected Action _onClick;
    protected GLabel _label;
    protected GButton _button;
    protected string _buttonText;
    
    public CtlButtonDrawer(string text) : base(text)
    {
    }

    public CtlButtonDrawer(string text, int fontSize) : base(text, fontSize)
    {
    }
    
    protected override GComponent OnCreateCom()
    {
        _label = UIPackage.CreateObject(FGUIManager.PKG_NEXT_CORE, "ComButtonDrawer").asLabel;
        _label.text = _text;
        if (_fontSize != Int32.MinValue)
        {
            _label.titleFontSize = _fontSize;
        }
        
        _button = _label.GetChild("button").asButton;
        _button.onClick.Add(OnButtonClick);
        _button.title = _buttonText;
        
        return _label;
    }

    protected void OnButtonClick(EventContext context)
    {
        _onClick?.Invoke();
    }

    protected override void SetDrawerEditable(bool value)
    {
        _button.enabled = value;
    }
    
    public void SetOnClick(Action onClick)
    {
        _onClick = onClick;
    }

    public void SetButtonText(string buttonText)
    {
        _buttonText = buttonText;
        if (_button != null)
        {
            _button.title = buttonText;
        }
    }
}