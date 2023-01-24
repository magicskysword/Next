using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using FairyGUI;
using SkySwordKill.Next.Utils;
using SkySwordKill.NextFGUI.NextCore;
using UnityEngine;
using Object = UnityEngine.Object;

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
        lstTab.scrollPane._displayOnTop = true;
        lstTab.SetSize(lstTab.width, lstTab.height);
    }

    private int _curTabIndex;
    private EventCallback1 _onRemoveTabItem;

    public Action<PanelPageBase> OnTabAdd { get; set; } = _ => { };
    public Action<PanelPageBase> OnTabRemove { get; set; } = _ => { };
    public Action<PanelPageBase> OnTabOpen { get; set; } = _ => { };
    public Action<PanelPageBase> OnTabClose { get; set; } = _ => { };
    
    private List<PanelPageBase> Tabs { get; } = new List<PanelPageBase>();
    private CancellationTokenSource _ctsDrag = new CancellationTokenSource();
    public GObject currentContent { get; set; }
        
    public UI_ComMainDocumentView DocumentView { get; }
    public int CurTabIndex => _curTabIndex;
        
    private void OnClickTabItem(EventContext context)
    {
        var item = (UI_BtnTab)context.data;
        var index = DocumentView.m_lstTab.GetChildIndex(item);
        OnSelectTab(index);
    }

    private void OnSelectTab(int index, bool forceRefresh = false)
    {
        if (_curTabIndex == index && !forceRefresh)
        {
            return;
        }
        var oldPage = index > 0 && index < Tabs.Count ? Tabs[index] : null;
        OnTabClose?.Invoke(oldPage);
        
        _curTabIndex = index;
        DocumentView.m_lstTab.selectedIndex = _curTabIndex;
        DocumentView.m_placeholder.visible = false;
        if(currentContent != null)
        {
            DocumentView.RemoveChild(currentContent);
        }
        currentContent = null;
        
        if (index >= 0 && index < Tabs.Count)
        {
            var page = Tabs[index];
            if(page.Content != null)
            {
                currentContent = page.Content;
                DocumentView.AddChild(page.Content);
                currentContent.SetSize(DocumentView.m_placeholder.width, DocumentView.m_placeholder.height);
                currentContent.SetXY(DocumentView.m_placeholder.x, DocumentView.m_placeholder.y);
                currentContent.AddRelation(DocumentView.m_placeholder, RelationType.Size);
            }
            page.Open();
            OnTabOpen?.Invoke(page);
        }
    }

    private void OnRemoveTabItem(EventContext context)
    {
        // 停止冒泡
        context.StopPropagation();
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
        tabItem.m_dragArea.draggable = true;
        tabItem.m_dragArea.onDragStart.Add(OnTabDragStart);
        tabItem.onDragStart.Add(OnTabItemDragStart);
        tabItem.onDragEnd.Add(OnTabItemDragEnd);
    }

    private void OnTabDragStart(EventContext context)
    {
        var dragArea = (GObject)context.sender;
        var tabItem = (UI_BtnTab)dragArea.parent;
        dragArea.StopDrag();
        tabItem.StartDrag((int)context.data);
    }

    private void OnTabItemDragStart(EventContext context)
    {
        context.PreventDefault();
        var tabItem = (UI_BtnTab)context.sender;
        StartTabDrag(tabItem, (int)context.data).Forget();
    }

    private void OnTabItemDragEnd(EventContext context)
    {
        _ctsDrag?.Cancel();
        
        var tabItem = (UI_BtnTab)context.sender;
        var hoverItem = Stage.inst.touchTarget?.gOwner;
        if (hoverItem == null)
        {
            return;
        }

        if (hoverItem.IsChildOf(DocumentView.m_lstTab))
        {
            var targetTabItem = hoverItem.FindParent<UI_BtnTab>();
            if(targetTabItem == tabItem)
            {
                return;
            }
            
            var curIndex = DocumentView.m_lstTab.GetChildIndex(tabItem);
            var targetIndex = DocumentView.m_lstTab.GetChildIndex(targetTabItem);
            if(curIndex == -1 || targetIndex == -1 || curIndex == targetIndex)
                return;
            
            // 如果curIndex小于targetIndex，那么targetTabItem的实际位置会减小1，tabItem插入到targetTabItem之后
            // 如果curIndex大于targetIndex，那么targetTabItem的实际位置不变，tabItem插入到targetTabItem之前
            DocumentView.m_lstTab.RemoveChildAt(curIndex, false);
            DocumentView.m_lstTab.AddChildAt(tabItem, targetIndex);
            var tab = Tabs[curIndex];
            Tabs.RemoveAt(curIndex);
            Tabs.Insert(targetIndex, tab);
            
            OnSelectTab(targetIndex);
        }
    }

    private async UniTask StartTabDrag(UI_BtnTab tabItem, int touchId = -1)
    {
        var dObject = tabItem.displayObject;
        dObject.EnterPaintingMode(1024, null);

        await UniTask.NextFrame();

        var rdTex = (RenderTexture)dObject.paintingGraphics.texture.nativeTexture;
        var texture = rdTex.ToTexture2D();
        dObject.LeavePaintingMode(1024);

        FGUIDragDropManager.inst.StartDrag(tabItem, texture.CreateTempNTexture(), tabItem);
        
        // 每帧检测
        _ctsDrag?.Cancel();
        _ctsDrag = new CancellationTokenSource();

        while (!_ctsDrag.IsCancellationRequested)
        {
            // 获取当前鼠标位置
            var pos = Stage.inst.touchPosition;
            // 转换为列表本地坐标
            var localPos = DocumentView.m_lstTab.GlobalToLocal(pos);
            // 处于列表x坐标的0~50时，向左滚动
            if (localPos.x >= 0 && localPos.x <= 50)
            {
                DocumentView.m_lstTab.scrollPane.ScrollLeft(0.5f, true);
            }
            // 处于列表x坐标最右侧50范围时，向右滚动
            else if (localPos.x <= DocumentView.m_lstTab.width && localPos.x >= DocumentView.m_lstTab.width - 50)
            {
                DocumentView.m_lstTab.scrollPane.ScrollRight(0.5f, true);
            }

            await UniTask.NextFrame();
        }
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
        if(page.Content == currentContent)
        {
            DocumentView.RemoveChild(page.Content);
            currentContent = null;
        }
        page.Close();
        RefreshTabs();
        if (Tabs.Count > 0)
        {
            newIndex = Mathf.Clamp(oldIndex, 0, Tabs.Count - 1);
        }
        else
        {
            newIndex = -1;
        }

        OnSelectTab(newIndex, true);
    }

    public void SelectTab(PanelPageBase page)
    {
        var index = Tabs.IndexOf(page);
        OnSelectTab(index,true);
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