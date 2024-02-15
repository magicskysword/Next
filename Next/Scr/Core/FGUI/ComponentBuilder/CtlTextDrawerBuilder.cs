using SkySwordKill.Next.FGUI.Component;

namespace SkySwordKill.Next.FGUI.ComponentBuilder;

public class CtlTextDrawerBuilder : CtlPropertyDrawerBuilderBase
{
    protected string _text;
    protected int _fontSize = 14;
    
    public CtlTextDrawerBuilder SetText(string text)
    {
        _text = text;
        return this;
    }
    
    public CtlTextDrawerBuilder SetFontSize(int fontSize)
    {
        _fontSize = fontSize;
        return this;
    }
    
    public override CtlPropertyDrawerBase Build()
    {
        var drawer = new CtlTextDrawer(_text, _fontSize);
        BuildBase(drawer);
        
        return drawer;
    }
}