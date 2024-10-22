using System;
using System.Collections.Generic;
using FairyGUI;

namespace SkySwordKill.Next.FGUI.Component;

public abstract class CtlInspectorBase : IInspector
{
    protected abstract GList _drawerGList { get; }
    protected List<IPropertyDrawer> _drawers = new List<IPropertyDrawer>();
    public event Action OnPropertyChanged;
        
    public bool Editable { get; set; } = true;

    public void AddDrawer(IPropertyDrawer drawer)
    {
        drawer.AddChangeListener(OnDrawerChanged);
        _drawers.Add(drawer);
        _drawerGList.AddChild(drawer.CreateCom());
        OnAddDrawer(drawer);
    }

    private void OnDrawerChanged()
    {
        OnPropertyChanged?.Invoke();
    }

    public void Clear()
    {
        _drawerGList.RemoveChildren();
        foreach (var drawer in _drawers)
        {
            drawer.RemoveCom();
        }
        _drawers.Clear();
        OnClear();
    }

    public void Refresh()
    {
        foreach (var drawer in _drawers)
        {
            drawer.Editable = Editable;
        }
        _drawers.RefreshWithChain();
        
        OnRefresh();
    }

    protected virtual void OnAddDrawer(IPropertyDrawer drawer)
    {
            
    }
        
    protected virtual void OnClear()
    {
            
    }

    protected virtual void OnRefresh()
    {
            
    }
}