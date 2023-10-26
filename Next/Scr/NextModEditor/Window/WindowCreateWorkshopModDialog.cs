using System.Collections.Generic;
using FairyGUI;
using SkySwordKill.Next.Extension;
using SkySwordKill.Next.FGUI;
using SkySwordKill.Next.FGUI.Component;
using SkySwordKill.Next.FGUI.Dialog;
using SkySwordKill.NextFGUI.NextCore;
using SkySwordKill.NextModEditor.Mod;
using SkySwordKill.NextModEditor.PanelProject;
using UnityEngine;

namespace SkySwordKill.Next.NextModEditor.Window;

public class WindowCreateWorkshopModDialog : WindowDialogBase
{
    public WindowCreateWorkshopModDialog() : base("NextCore", "WinCreateWorkshopModDialog")
    {
            
    }
    
    public UI_WinCreateWorkshopModDialog MainView => contentPane as UI_WinCreateWorkshopModDialog;
        
    public string Title { get; set; }
    public OnSelectMod OnConfirm { get; set; }
        
    public CtlListProject ListProject { get; set; }
    public CtlPropertyInspector Inspector { get; set; }
        
    private ProjectListBase CurrentItem { get; set; }
    
    public List<ProjectListBase> ProjectList { get; } = new();

    protected override void OnInit()
    {
        base.OnInit();

        ListProject = new CtlListProject(MainView.m_projectList);
        Inspector = new CtlPropertyInspector(MainView.m_inspector);
            
        ListProject.SetClickItem(OnClickProjectItem);
        foreach (var projectListItemBase in ProjectList)
        {
            ListProject.AddProject(projectListItemBase);
        }

        MainView.m_btnConfirm.onClick.Add(OnClickConfirm);
        MainView.m_btnCancel.onClick.Add(OnClickCancel);
        MainView.m_frame.m_closeButton.onClick.Add(OnClickCancel);

        ListProject.Refresh();
        ListProject.ProjectList.m_list.selectedIndex = 0;
        OnSelectWorkshopItem(ListProject.FirstProjectItem as ProjectListCreateWorkshopItem);
    }
    
    protected override void OnKeyDown(EventContext context)
    {
        base.OnKeyDown(context);
        
        if (context.inputEvent.keyCode == KeyCode.Escape)
        {
            Cancel();
        }
    }

    private void Cancel()
    {
        Hide();
    }

    private void OnClickCancel()
    {
        Cancel();
    }

    private void OnClickConfirm(EventContext context)
    {
        ModWorkshop workshop;
        try
        {
            if (CurrentItem is ProjectListCreateWorkshopItem workshopItem)
            {
                workshop = workshopItem.OnCreateWorkshop();
                ModEditorManager.I.SaveWorkshop(workshop);
            }
            else
            {
                return;
            }
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
                
        if(workshopItem != null)
        {
            workshopItem.Inspector = Inspector;
            workshopItem.OnInspect();
        }
            
        Inspector.Refresh();
    }
}