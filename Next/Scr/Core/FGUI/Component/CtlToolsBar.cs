using System;
using FairyGUI;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.Next.FGUI.Component;

public class CtlToolsBar
{
    public CtlToolsBar(UI_ComToolsBar com)
    {
        ToolsBar = com;
    }

    public UI_ComToolsBar ToolsBar { get; }
        
    public GButton AddToolBtn(string icon, string tooltips, Action onClick)
    {
        var btn = ToolsBar.m_tools.AddItemFromPool().asButton;
        btn.icon = icon;
        btn.tooltips = tooltips;
        btn.onClick.Add(() => onClick());
        return btn;
    }

    public GObject AddToolSep()
    {
        return ToolsBar.m_tools.AddItemFromPool("ui://NextCore/ComToolsSep");
    }

    public CtlToolsSearchBox AddToolSearch(Action<string> onSearch)
    {
        var searchBox = (UI_ComToolsSearchBox)ToolsBar.m_tools.AddItemFromPool("ui://NextCore/ComToolsSearchBox");
        var ctlSearch = new CtlToolsSearchBox(searchBox);
        ctlSearch.OnSearch = onSearch;
        return ctlSearch;
    }

    public void RemoveAllTools()
    {
        ToolsBar.m_tools.numItems = 0;
    }
}