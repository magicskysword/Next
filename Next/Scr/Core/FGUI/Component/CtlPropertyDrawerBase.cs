using System;
using System.Collections.Generic;
using FairyGUI;

namespace SkySwordKill.Next.FGUI.Component;

public abstract class CtlPropertyDrawerBase : IPropertyDrawer
{
    private bool _editable = true;

    public bool Editable
    {
        get
        {
            return _editable;
        }
        set
        {
            _editable = value;
            if (Component != null)
            {
                SetDrawerEditable(value);
            }
        }
    }

    private string _tooltips;
    public string Tooltips
    {
        get => _tooltips;
        set
        {
            _tooltips = value;
            if (Component != null)
            {
                Component.tooltips = value;
            }
        }
    }

    protected Action OnChanged { get; set; }
    public UndoInstManager UndoManager { get; set; }
    
    public GComponent Component { get; set; }

    /// <summary>
    /// 绑定的绘制器，该绘制器刷新时同时也会刷新其他绘制器
    /// </summary>
    public List<IPropertyDrawer> ChainDrawers { get; set; } = new List<IPropertyDrawer>();
    protected abstract GComponent OnCreateCom();
    protected virtual void OnRemoveCom(GComponent component) { }
    protected virtual void OnRefresh() { }
    protected abstract void SetDrawerEditable(bool value);
        
    public GComponent CreateCom()
    {
        if(Component == null)
        {
            Component = OnCreateCom();
        }
        Component.tooltips = Tooltips;
        SetDrawerEditable(Editable);
        return Component;
    }

    public void RemoveCom()
    {
        if(Component != null)
        {
            OnRemoveCom(Component);
            Component.Dispose();
        }

        Component = null;
    }

    /// <summary>
    /// 刷新绘制器，并刷新其他绘制器
    /// </summary>
    /// <param name="chainRefresh"></param>
    public void Refresh(bool chainRefresh = true)
    {
        OnRefresh();

        if (chainRefresh && ChainDrawers.Count > 0)
        {
            var hashSet = new HashSet<IPropertyDrawer>();
            var queue = new Queue<IPropertyDrawer>();
            foreach (var drawer in ChainDrawers)
            {
                queue.Enqueue(drawer);
                hashSet.Add(drawer);
            }
            
            while (queue.Count > 0)
            {
                var currentDrawer = queue.Dequeue();
                currentDrawer.Refresh(false);
                if (currentDrawer.ChainDrawers.Count > 0)
                {
                    foreach (var chainDrawer in currentDrawer.ChainDrawers)
                    {
                        if (hashSet.Add(chainDrawer))
                        {
                            queue.Enqueue(chainDrawer);
                        }
                    }
                }
            }
        }
    }

    public IPropertyDrawer AddChangeListener(Action OnDrawerChanged)
    {
        OnChanged += OnDrawerChanged;
        return this;
    }
    
    public IPropertyDrawer RemoveChangeListener(Action OnDrawerChanged)
    {
        OnChanged -= OnDrawerChanged;
        return this;
    }
    
    public IPropertyDrawer ClearChangeListener()
    {
        OnChanged = () => { };
        return this;
    }

    /// <summary>
    /// 链接一个绘制器，当自身刷新时，也会刷新该绘制器
    /// </summary>
    /// <param name="iconDrawer"></param>
    public IPropertyDrawer AddChainDrawer(IPropertyDrawer iconDrawer)
    {
        ChainDrawers.Add(iconDrawer);
        return this;
    }
    
    public IPropertyDrawer RemoveChainDrawer(IPropertyDrawer iconDrawer)
    {
        ChainDrawers.Remove(iconDrawer);
        return this;
    }
}