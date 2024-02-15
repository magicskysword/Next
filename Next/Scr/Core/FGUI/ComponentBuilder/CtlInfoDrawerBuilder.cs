using SkySwordKill.Next.FGUI.Component;

namespace SkySwordKill.Next.FGUI.ComponentBuilder;

public class CtlInfoDrawerBuilder : CtlPropertyDrawerBuilderBase
{
    protected string _title = string.Empty;
    protected string _content = string.Empty;
    protected int _fontSize = 14;
    
    public CtlInfoDrawerBuilder SetTitle(string title)
    {
        _title = title;
        return this;
    }
    
    public CtlInfoDrawerBuilder SetContent(string content)
    {
        _content = content;
        return this;
    }
    
    public CtlInfoDrawerBuilder SetFontSize(int fontSize)
    {
        _fontSize = fontSize;
        return this;
    }
    
    public override CtlPropertyDrawerBase Build()
    {
        var drawer = new CtlInfoDrawer(_title, _content, _fontSize);
        
        BuildBase(drawer);
        
        return drawer;
    }
}