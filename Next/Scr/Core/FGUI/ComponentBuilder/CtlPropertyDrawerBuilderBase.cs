using System.Collections.Generic;
using SkySwordKill.Next.FGUI.Component;

namespace SkySwordKill.Next.FGUI.ComponentBuilder;

public abstract class CtlPropertyDrawerBuilderBase
{
    protected bool _editable = true;
    protected string _tooltips = "";
    protected UndoInstManager _undoManager;
    protected List<IPropertyDrawer> _chainDrawer;
    
    public abstract CtlPropertyDrawerBase Build();
    
    public CtlPropertyDrawerBuilderBase SetEditable(bool value)
    {
        _editable = value;
        
        return this;
    }
    
    public CtlPropertyDrawerBuilderBase SetTooltips(string value)
    {
        _tooltips = value;
        
        return this;
    }
    
    public CtlPropertyDrawerBuilderBase SetUndoManager(UndoInstManager value)
    {
        _undoManager = value;
        
        return this;
    }

    public CtlPropertyDrawerBuilderBase AddChainDrawer(IPropertyDrawer value)
    {
        if(_chainDrawer == null)
        {
            _chainDrawer = new List<IPropertyDrawer>();
        }
        
        _chainDrawer.Add(value);
        
        return this;
    }

    protected void BuildBase(CtlPropertyDrawerBase drawerBase)
    {
        drawerBase.Editable = _editable;
        drawerBase.Tooltips = _tooltips;
        drawerBase.UndoManager = _undoManager;
        if (_chainDrawer != null)
        {
            drawerBase.ChainDrawers.AddRange(_chainDrawer);
        }
        
    }
}