using System;
using FairyGUI;
using SkySwordKill.Next.Extension;
using SkySwordKill.Next.FGUI;
using SkySwordKill.Next.FGUI.Component;
using SkySwordKill.Next.FGUI.Dialog;
using SkySwordKill.NextFGUI.NextCore;
using SkySwordKill.NextModEditor.Mod;
using SkySwordKill.NextModEditor.PanelProject;

namespace SkySwordKill.NextModEditor.Panel
{
    public class WindowCreateWorkshopModDialog : WindowDialogBase
    {
        private WindowCreateWorkshopModDialog() : base("NextCore", "WinCreateWorkshopModDialog")
        {
            
        }

        public static WindowCreateWorkshopModDialog CreateDialog(string title, OnSelectMod confirm)
        {
            var window = new WindowCreateWorkshopModDialog();
            window.Title = title;
            window.OnConfirm = confirm;

            window.Show();
            return window;
        }
        
        public UI_WinCreateWorkshopModDialog MainView => contentPane as UI_WinCreateWorkshopModDialog;
        
        public string Title { get; set; }
        public OnSelectMod OnConfirm { get; set; }
        
        public CtlListProject ListProject { get; set; }
        public CtlPropertyInspector Inspector { get; set; }
        
        private ProjectListCreateWorkshopItem CurrentItem { get; set; }

        protected override void OnInit()
        {
            base.OnInit();

            ListProject = new CtlListProject(MainView.m_projectList);
            Inspector = new CtlPropertyInspector(MainView.m_inspector);
            
            ListProject.SetClickItem(OnClickProjectItem);

            var emptyWorkshop = new ProjectListCreateWorkshopEmpty();
            ListProject.AddProject(emptyWorkshop);
            
            MainView.m_btnConfirm.onClick.Add(OnClickConfirm);
            MainView.m_btnCancel.onClick.Add(OnClickCancel);
            MainView.m_frame.m_closeButton.onClick.Add(OnClickCancel);

            ListProject.Refresh();
            ListProject.ProjectList.m_list.selectedIndex = 0;
            OnSelectWorkshopItem(emptyWorkshop);
        }

        private void OnClickCancel()
        {
            Hide();
        }

        private void OnClickConfirm(EventContext context)
        {
            ModWorkshop workshop;
            try
            {
                workshop = CurrentItem.OnCreateWorkshop();
                ModEditorManager.I.SaveWorkshop(workshop);
            }
            catch (CreateWorkshopException e)
            {
                WindowConfirmDialog.CreateDialog("提示".I18NTodo(), e.Message, false);
                return;
            }
            
            Hide();
            OnConfirm.Invoke(workshop);
        }

        private void OnClickProjectItem(EventContext context, ProjectListBase item)
        {
            if (item is ProjectListCreateWorkshopItem workshopItem)
            {
                OnSelectWorkshopItem(workshopItem);
            }
        }

        private void OnSelectWorkshopItem(ProjectListCreateWorkshopItem workshopItem)
        {
            Inspector.Clear();
            
            CurrentItem = workshopItem;
                
            workshopItem.Inspector = Inspector;
            workshopItem.OnInspect();
            
            Inspector.Refresh();
        }
    }
}