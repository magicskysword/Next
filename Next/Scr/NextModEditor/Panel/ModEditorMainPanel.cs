using System;
using System.Collections.Generic;
using FairyGUI;
using SkySwordKill.Next;
using SkySwordKill.Next.Extension;
using SkySwordKill.Next.FGUI;
using SkySwordKill.Next.FGUI.Component;
using SkySwordKill.NextEditor.Event;
using SkySwordKill.NextEditor.Mod;
using SkySwordKill.NextEditor.PanelPage;
using SkySwordKill.NextEditor.PanelProject;
using SkySwordKill.NextFGUI.NextCore;
using UnityEngine;

namespace SkySwordKill.NextEditor.Panel
{
    public class ModEditorMainPanel : FGUIWindowBase
    {
        public ModEditorMainPanel() : base("NextModEditor", "ModEditorMainPanel")
        {
            
        }

        private UI_ModEditorMainPanel MainView => (UI_ModEditorMainPanel)contentPane;
        private CtlHeaderBar HeaderBar { get; set; }
        private CtlProjectTree ProjectTree { get; set; }
        private CtlDocumentView DocumentView { get; set; }
        
        private ModProject Project { get; set; }

        private PopupMenu _filePopMenu = new PopupMenu();

        protected override void OnInit()
        {
            base.OnInit();
            InitProject();
            InitHeader();
            
            FGUITools.BindHSeg(MainView.m_seg, () => MainView.width * 0.1f, () => MainView.width * 0.9f);
            
            MakeFullScreen();
            Center();
            AddRelation(GRoot.inst,RelationType.Size);
        }

        #region Project

        private void InitProject()
        {
            HeaderBar = new CtlHeaderBar(MainView.m_comHeader);
            ProjectTree = new CtlProjectTree(MainView.m_comProject);
            DocumentView = new CtlDocumentView(MainView.m_comDocument);

            ProjectTree.OnClickItem = (context, node) =>
            {
                if(!context.inputEvent.isDoubleClick)
                    return;
                
                if(!node.IsLeaf)
                    return;
                
                if(Project == null)
                    return;
                
                if (node is ProjectTreeItem item)
                {
                    if (!DocumentView.TryGetTab(item.ID,out var tab))
                    {
                        tab = item.CreatePage();
                        tab.ID = item.ID;
                        DocumentView.AddTab(tab);
                    }
                    DocumentView.SelectTab(tab);
                }
            };
        }

        #endregion
        
        #region HeadbarFunction
        
        private void InitHeader()
        {
            // File
            var btnOpen = _filePopMenu.AddItem("ModEditor.Main.Header.File.Open".I18N(), OnOpenModProject);
            btnOpen.name = "itemOpen";
            var btnSave = _filePopMenu.AddItem("ModEditor.Main.Header.File.Save".I18N(), () => { });
            btnSave.name = "itemSave";
            var btnExport = _filePopMenu.AddItem("ModEditor.Main.Header.File.Export".I18N(), () => { });
            btnExport.name = "itemExport";
            var btnExit = _filePopMenu.AddItem("ModEditor.Main.Header.File.Exit".I18N(), Hide);
            btnExit.name = "exit";
            
            // Root
            HeaderBar.AddMenu("ModEditor.Main.Header.File".I18N(), _filePopMenu);    
        }

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
    }
}