using System;
using SkySwordKill.Next.FGUI.Component;

namespace SkySwordKill.Next.FGUI.ComponentBuilder;

public class CtlStringPropertyDrawerBuilder : CtlPropertyDrawerBuilderBase
{
    protected string _drawerName;
    protected Action<string> _setter;
    protected Func<string> _getter;
    
    public CtlStringPropertyDrawerBuilder SetDrawerName(string value)
    {
        _drawerName = value;
        
        return this;
    }
    
    public CtlStringPropertyDrawerBuilder SetSetter(Action<string> value)
    {
        _setter = value;
        
        return this;
    }
    
    public CtlStringPropertyDrawerBuilder SetGetter(Func<string> value)
    {
        _getter = value;
        
        return this;
    }
    
    public override CtlPropertyDrawerBase Build()
    {
        if (_drawerName == null)
        {
            throw new Exception("drawerName is null");
        }
        
        if (_setter == null)
        {
            throw new Exception("setter is null");
        }
        
        if (_getter == null)
        {
            throw new Exception("getter is null");
        }
        
        var drawer = new CtlStringPropertyDrawer(_drawerName, _setter, _getter);
        BuildBase(drawer);
        
        return drawer;
    }
}