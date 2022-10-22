using System;
using System.Collections.Generic;
using FairyGUI;
using SkySwordKill.Next.FGUI;
using SkySwordKill.NextFGUI.NextCore;
using UnityEngine;

namespace SkySwordKill.Next.FGUI.Component;

public class CtlDocumentView
{
    public CtlDocumentView(UI_ComMainDocumentView documentView)
    {
        _onRemoveTabItem = OnRemoveTabItem;
        DocumentView = documentView;
            
        DocumentView.onKeyDown.Add(OnKeyDown);
        var lstTab = DocumentView.m_lstTab;
        lstTab.itemRenderer = TabItemRenderer;
        lstTab.onClickItem.Set(OnClickTabItem);
    }

    private int _curTabIndex;
    private EventCallback1 _onRemoveTabItem;

    public Action<PanelPageBase> OnTabAdd { get; set; } = _ => { };
    public Action<PanelPageBase> OnTabRemove { get; set; } = _ => { };
    public Action<PanelPageBase> OnTabOpen { get; set; } = _ => { };
    public Action<PanelPageBase> OnTabClose { get; set; } = _ => { };
        
        
    private List<PanelPageBase> Tabs { get; } = new List<PanelPageBase>();
        
    public UI_ComMainDocumentView DocumentView { get; }
    public int CurTabIndex => _curTabIndex;
        
    private void OnClickTabItem(EventContext context)
    {
        var item = (UI_BtnTab)context.data;
        var index = DocumentView.m_lstTab.GetChildIndex(item);
        OnSwitchTab(index);
    }

    private void OnSwitchTab(int index, bool forceRefresh = false)
    {
        if (_curTabIndex == index && !forceRefresh)
        {
            return;
        }
        Debug.Log($"切换Tab");
        var oldPage = index > 0 && index < Tabs.Count ? Tabs[index] : null;
        OnTabClose?.Invoke(oldPage);
            
            
        _curTabIndex = index;
        DocumentView.m_lstTab.selectedIndex = _curTabIndex;
        DocumentView.m_content.RemoveChildren();
        if (index >= 0 && index < Tabs.Count)
        {
            var page = Tabs[index];
            if(page.Content != null)
            {
                DocumentView.m_content.AddChild(page.Content);
            }
            page.Open();
            OnTabOpen?.Invoke(page);
        }
    }

    private void OnRemoveTabItem(EventContext context)
    {
        var btn = (GButton)context.sender;
        var tabItem = (UI_BtnTab)btn.parent;
        var tab = (PanelPageBase)tabItem.data;
        OnTabRemove?.Invoke(tab);
        RemoveTab(tab);
    }
        
    private void OnKeyDown(EventContext context)
    {
        if(CurTabIndex >= 0 && CurTabIndex < Tabs.Count)
        {
            var tab = Tabs[CurTabIndex];
            if(tab.OnHandleKey(context.inputEvent))
            {
                context.StopPropagation();
            }
        }
    }

    private void TabItemRenderer(int index, GObject item)
    {
        var tabItem = (UI_BtnTab)item;
        var tabData = Tabs[index];

        tabItem.title = tabData.Name;
        tabItem.data = tabData;
        tabItem.m_closeButton.onClick.Set(_onRemoveTabItem);
    }

    private void RefreshTabs()
    {
        DocumentView.m_lstTab.numItems = Tabs.Count;
    }

    public void TryAddAndSelectTab(IDocumentItem item)
    {
        if (!TryGetTab(item.ID,out var tab))
        {
            tab = item.CreatePage();
            tab.ID = item.ID;
            AddTab(tab);
        }
        SelectTab(tab);
    }
        
    public bool TryGetTab(string tabID,out PanelPageBase page)
    {
        page = Tabs.Find(t => t.ID == tabID);
        return page != null;
    }
        
    public bool HasTab(string tabID)
    {
        return Tabs.Find(tab => tab.ID == tabID) != null;
    }
        
    public void AddTab(PanelPageBase page)
    {
        Tabs.Add(page);
            
        page.Create();
        RefreshTabs();
            
        OnTabAdd?.Invoke(page);
    }

    public void RemoveTab(PanelPageBase page)
    {
        var lstTab = DocumentView.As<UI_ComMainDocumentView>().m_lstTab;
        var oldIndex = lstTab.selectedIndex;
        var newIndex = 0;
        Tabs.Remove(page);
        page.Close();
        RefreshTabs();
        if (Tabs.Count > 0)
        {
            if (oldIndex > 0)
                newIndex = oldIndex - 1;
            else
                newIndex = 0;
        }
        else
        {
            newIndex = -1;
        }

        OnSwitchTab(newIndex);
    }

    public void SelectTab(PanelPageBase page)
    {
        var index = Tabs.IndexOf(page);
        OnSwitchTab(index,true);
    }

    public void RemoveAllTab()
    {
        foreach (var tab in Tabs)
        {
            tab.Close();
        }
        Tabs.Clear();
        RefreshTabs();
    }
}