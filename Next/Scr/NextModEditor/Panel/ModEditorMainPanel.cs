using System;
using System.Collections.Generic;
using FairyGUI;
using SkySwordKill.Next;
using SkySwordKill.Next.Extension;
using SkySwordKill.Next.FGUI;
using SkySwordKill.Next.FGUI.ComponentCtl;
using SkySwordKill.NextEditor.Event;
using SkySwordKill.NextEditor.Mod;
using SkySwordKill.NextEditor.PanelPage;
using SkySwordKill.NextEditor.PanelProject;
using SkySwordKill.NextFGUI.NextCore;
using SkySwordKill.NextFGUI.NextModEditor;
using UnityEngine;

namespace SkySwordKill.NextEditor.Panel
{
    public class ModEditorMainPanel : FGUIWindowBase
    {
        public ModEditorMainPanel() : base("NextModEditor", "ModEditorMainPanel")
        {
            
        }

        private UI_ModEditorMainPanel MainView => (UI_ModEditorMainPanel)contentPane;
        
        private ModProject Project { get; set; }
        
        public CtlPropertyInspector Inspector { get; set; }
        
        private EventCallback1 _onRemoveTabItem;
        private PopupMenu _filePopMenu = new PopupMenu();
        private List<PanelPageBase> Tabs { get; } = new List<PanelPageBase>();
        private List<ProjectTreeBase> ProjectItems { get; } = new List<ProjectTreeBase>();
        
        private int _curTabIndex;

        protected override void OnInit()
        {
            base.OnInit();
            InitPopMenu();
            InitHeader();
            InitProject();
            InitDocumentView();
            InitSeg();
            
            MakeFullScreen();
            Center();
            AddRelation(GRoot.inst,RelationType.Size);
        }

        private void InitPopMenu()
        {
            var btnOpen = _filePopMenu.AddItem("ModEditor.Main.Header.File.Open".I18N(), OnOpenModProject);
            btnOpen.name = "itemOpen";
            var btnSave = _filePopMenu.AddItem("ModEditor.Main.Header.File.Save".I18N(), () => { });
            btnSave.name = "itemSave";
            var btnExport = _filePopMenu.AddItem("ModEditor.Main.Header.File.Export".I18N(), () => { });
            btnExport.name = "itemExport";
            var btnExit = _filePopMenu.AddItem("ModEditor.Main.Header.File.Exit".I18N(), Hide);
            btnExit.name = "exit";
        }

        private void InitHeader()
        {
            var btnFile = MainView.m_comHeader.As<UI_ComMainHeader>().m_lstHeader.AddItemFromPool().asButton;
            btnFile.title = "ModEditor.Main.Header.File".I18N();
            btnFile.onClick.Add(()=>_filePopMenu.Show(btnFile,PopupDirection.Down));
        }
        
        private void InitProject()
        {
            var treeView = MainView.m_comProject.As<UI_ComMainProject>().m_treeView;
            treeView.treeNodeRender = ProjectTreeItemRenderer;
            treeView.onClickItem.Set(OnClickProjectTreeItem);
            
            AddProject("ModEditor.Main.project.modConfig".I18N(),0,new ProjectTreeItemModConfig());
            AddProject("ModEditor.Main.project.modCreateAvatar".I18N(),0,new ProjectTreeItemModCreateAvatar());
            AddProject("ModEditor.Main.project.modBuffInfo".I18N(),0,new ProjectTreeItemModBuffInfo());
            AddProject("剧情预览",0,new ProjectTreeItemBaseFungus());
        }

        private void InitDocumentView()
        {
            _onRemoveTabItem = OnRemoveTabItem;

            var lstTab = MainView.m_comDocument.As<UI_ComMainDocumentView>().m_lstTab;
            lstTab.itemRenderer = TabItemRenderer;
            lstTab.onClickItem.Set(OnClickTabItem);

            Inspector = new CtlPropertyInspector(MainView.m_comInspector.As<UI_ComMainInspector>());
        }
        
        private void InitSeg()
        {
            MainView.m_leftSeg.draggable = true;
            MainView.m_leftSeg.onDragStart.Set(OnDragLeftSegStart);
            MainView.m_leftSeg.cursor = "resizeH";

            MainView.m_rightSeg.draggable = true;
            MainView.m_rightSeg.onDragStart.Set(OnDragRightSegStart);
            MainView.m_rightSeg.cursor = "resizeH";
        }

        private void OnDragLeftSegStart(EventContext context)
        {
            context.PreventDefault();
            DragDropManager.inst.StartDrag(null, null,null, (int)context.data);
            DragDropManager.inst.dragAgent.onDragMove.Set(OnDragLeftSegMove);
        }
        
        private void OnDragRightSegStart(EventContext context)
        {
            context.PreventDefault();
            DragDropManager.inst.StartDrag(null, null,null, (int)context.data);
            DragDropManager.inst.dragAgent.onDragMove.Set(OnDragRightSegMove);
        }
        
        private void OnDragLeftSegMove(EventContext context)
        {
            var posX = context.inputEvent.x;
            posX = Mathf.Clamp(posX, 50, MainView.m_rightSeg.x - 50);
            MainView.m_leftSeg.x = posX;
        }
        
        private void OnDragRightSegMove(EventContext context)
        {
            var posX = context.inputEvent.x;
            posX = Mathf.Clamp(posX, MainView.m_leftSeg.x + 50, x + width - 50);
            MainView.m_rightSeg.x = posX;
        }

        #region HeadbarFunction

        private void OnOpenModProject()
        {
            try
            {
                var project = ModMgr.I.OpenProject();
                if (project != null)
                {
                    Project = project;
                    EventCenter.Send(new LoadModProjectEventArgs()
                    {
                        ModProject = project
                    });
                }
            }
            catch (Exception e)
            {
                Main.LogError(e);
            }
        }

        #endregion

        #region DocumentFunction

        private void OnClickTabItem(EventContext context)
        {
            var item = (UI_BtnTab)context.data;
            var index = MainView.m_comDocument.As<UI_ComMainDocumentView>().m_lstTab.GetChildIndex(item);
            OnSwitchTab(index);
        }

        private void OnSwitchTab(int index, bool forceRefresh = false)
        {
            if (_curTabIndex == index && !forceRefresh)
            {
                return;
            }
            Debug.Log($"切换Tab");
            _curTabIndex = index;
            MainView.m_comDocument.As<UI_ComMainDocumentView>().m_lstTab.selectedIndex = _curTabIndex;

            MainView.m_comDocument.As<UI_ComMainDocumentView>().m_content.RemoveChildren();
            Inspector.Clear();

            if (index >= 0)
            {
                var page = Tabs[index];
                if(page.Content != null)
                {
                    MainView.m_comDocument.As<UI_ComMainDocumentView>().m_content.AddChild(page.Content);
                }
                page.OnOpen();
            }
        }

        private void OnRemoveTabItem(EventContext context)
        {
            var btn = (GButton)context.sender;
            var tabItem = (UI_BtnTab)btn.parent;
            var tab = (PanelPageBase)tabItem.data;
            RemoveTab(tab);
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
            MainView.m_comDocument.As<UI_ComMainDocumentView>().m_lstTab.numItems = Tabs.Count;
        }

        private bool TryGetTab(string tabID,out PanelPageBase page)
        {
            page = Tabs.Find(t => t.ID == tabID);
            return page != null;
        }
        
        private bool HasTab(string tabID)
        {
            return Tabs.Find(tab => tab.ID == tabID) != null;
        }
        
        private void AddTab(PanelPageBase page)
        {
            Tabs.Add(page);
            
            page.Inspector = Inspector;
            page.Project = Project;
            page.OnAdd();
            
            RefreshTabs();
        }

        private void RemoveTab(PanelPageBase page)
        {
            var lstTab = MainView.m_comDocument.As<UI_ComMainDocumentView>().m_lstTab;
            var oldIndex = lstTab.selectedIndex;
            var newIndex = 0;
            Tabs.Remove(page);
            page.OnRemove();
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

        private void SelectTab(PanelPageBase page)
        {
            var index = Tabs.IndexOf(page);
            OnSwitchTab(index,true);
        }

        #endregion

        #region ProjectFunction

        private void AddProject(string projName,int projLayer,ProjectTreeBase projectTreeBase)
        {
            projectTreeBase.Name = projName;
            projectTreeBase.Layer = projLayer;
            ProjectItems.Add(projectTreeBase);

            var node = new GTreeNode(!projectTreeBase.IsLeaf);
            node.data = projectTreeBase;
            MainView.m_comProject.As<UI_ComMainProject>().m_treeView.rootNode.AddChild(node);
        }
        
        private void OnClickProjectTreeItem(EventContext context)
        {
            if(!context.inputEvent.isDoubleClick)
                return;
            
            if(Project == null)
                return;
            
            Main.LogDebug("点击TreeItem");
            
            var obj = (GObject)context.data;
            var node = obj.treeNode;
            var uiProjectBase = (ProjectTreeBase)node.data;

            if (uiProjectBase is ProjectTreeItem uiProjectItem)
            {
                if (!TryGetTab(uiProjectItem.ID,out var tab))
                {
                    tab = uiProjectItem.CreatePage();
                    tab.ID = uiProjectItem.ID;
                    AddTab(tab);
                }
                SelectTab(tab);
            }
        }
        
        private void ProjectTreeItemRenderer(GTreeNode node, GComponent obj)
        {
            var uiProjectBase = (ProjectTreeBase)node.data;
            var treeItem = (UI_BtnTreeItem)obj;

            treeItem.title = uiProjectBase.Name;
            if (uiProjectBase is ProjectTreeItem uiProjectItem)
            {
                if (!string.IsNullOrEmpty(uiProjectItem.Icon))
                {
                    treeItem.icon = uiProjectItem.Icon;
                }
                else
                {
                    treeItem.icon = "ui://NextCore/icon_tool1";
                }
            }
        }

        #endregion
    }
}