using System.Collections.Generic;
using FairyGUI;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.Next.FGUI.Component;

public class CtlGroupDrawer : CtlPropertyDrawerBase, IInspector
{
    private List<IPropertyDrawer> _drawers { get; } = new List<IPropertyDrawer>();
    private string DrawerName { get; set; }
    private bool IsExpand { get; set; }
    private UI_ComGroupDrawer GroupDrawer => (UI_ComGroupDrawer)Component;
    private CtlGroupInspector Inspector { get; set; }
    
    public CtlGroupDrawer(string title,bool isExpend ,params IPropertyDrawer[] drawers)
    {
        DrawerName = title;
        IsExpand = isExpend;
        _drawers.AddRange(drawers);
    }

    protected override GComponent OnCreateCom()
    {
        var drawer = UI_ComGroupDrawer.CreateInstance();
        drawer.title = DrawerName;
        drawer.onSizeChanged.Add(OnSizeChanged);
        Inspector = new CtlGroupInspector(drawer.m_list);
        Inspector.OnPropertyChanged += OnDrawerChanged;
        
        var oldDrawers = _drawers.ToArray();
        _drawers.Clear();
        foreach (var propertyDrawer in oldDrawers)
        {
            AddDrawer(propertyDrawer); 
        }
        
        var btnExpand = drawer.m_btnExpand;
        btnExpand.selected = IsExpand;
        btnExpand.onClick.Add(() =>
        {
            SetExpand(btnExpand.selected);
        });
        return drawer;
    }

    protected override void OnRemoveCom(GComponent component)
    {
        base.OnRemoveCom(component);
        Inspector = null;
    }
    
    public IReadOnlyList<IPropertyDrawer> Drawers => _drawers;

    public void AddDrawer(IPropertyDrawer drawer)
    {
        drawer.UndoManager = UndoManager;
        _drawers.Add(drawer);
        if(Inspector != null)
            Inspector.AddDrawer(drawer);
    }

    private void OnDrawerChanged()
    {
        OnChanged?.Invoke();
    }

    public void Clear()
    {
        _drawers.Clear();
        if(Inspector != null)
            Inspector.Clear();
    }

    protected override void SetDrawerEditable(bool value)
    {
        Inspector.Editable = value;
        Inspector.Refresh();
    }

    protected override void OnRefresh()
    {
        base.OnRefresh();
        if (IsExpand)
        {
            Inspector.Show();
        }
        else
        {
            Inspector.Hide();
        }
    }

    private void OnSizeChanged()
    {
        Inspector.Resize();
    }
        
    private void SetExpand(bool isExpand)
    {
        IsExpand = isExpand;
        Refresh();
    }
}