using System.Collections.Generic;
using SkySwordKill.Next.FGUI.Component;

namespace SkySwordKill.Next.FGUI.ComponentBuilder;

public class CtlGroupDrawerBuilder : CtlPropertyDrawerBuilderBase
{
    protected string _title;
    protected bool _isExpand = true;
    protected List<IPropertyDrawer> _drawers = new List<IPropertyDrawer>();
    
    public CtlGroupDrawerBuilder SetTitle(string title)
    {
        _title = title;
        return this;
    }
    
    public CtlGroupDrawerBuilder SetExpand(bool isExpand)
    {
        _isExpand = isExpand;
        return this;
    }
    
    public CtlGroupDrawerBuilder AddDrawer(IPropertyDrawer drawer)
    {
        _drawers.Add(drawer);
        return this;
    }
    
    public override CtlPropertyDrawerBase Build()
    {
        var drawer = new CtlGroupDrawer(_title, _isExpand, _drawers.ToArray());
        BuildBase(drawer);
        
        return drawer;
    }
}