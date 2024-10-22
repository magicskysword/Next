using System;
using SkySwordKill.Next.FGUI.Component;

namespace SkySwordKill.Next.FGUI.ComponentBuilder;

public class CtlButtonDrawerBuilder : CtlTextDrawerBuilder
{
    protected Action _onClick;
    protected string _buttonText;
    
    public override CtlPropertyDrawerBase Build()
    {
        var drawer = new CtlButtonDrawer(_text, _fontSize);
        BuildBase(drawer);
        drawer.SetOnClick(_onClick);
        drawer.SetButtonText(_buttonText);
        
        return drawer;
    }
    
    public CtlButtonDrawerBuilder SetOnClick(Action onClick)
    {
        _onClick = onClick;
        return this;
    }
    
    public CtlButtonDrawerBuilder SetButtonText(string buttonText)
    {
        _buttonText = buttonText;
        return this;
    }
    
    public CtlButtonDrawerBuilder SetTitle(string text)
    {
        _text = text;
        return this;
    }
}