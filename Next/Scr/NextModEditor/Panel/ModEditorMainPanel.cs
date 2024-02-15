﻿using System;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using FairyGUI;
using Newtonsoft.Json;
using SkySwordKill.Next;
using SkySwordKill.Next.Extension;
using SkySwordKill.Next.FGUI;
using SkySwordKill.Next.FGUI.Component;
using SkySwordKill.Next.FGUI.Dialog;
using SkySwordKill.Next.Mod;
using SkySwordKill.Next.NextModEditor.Window;
using SkySwordKill.Next.NextModEditor.WindowBuilder;
using SkySwordKill.NextFGUI.NextCore;
using SkySwordKill.NextModEditor.Mod;
using SkySwordKill.NextModEditor.PanelProject;

namespace SkySwordKill.NextModEditor.Panel;

public class ModEditorMainPanel : FGUIPanelBase
{
    public ModEditorMainPanel() : base("NextCore", "ModEditorMainPanel")
    {
            
    }

    private UI_ModEditorMainPanel MainView => (UI_ModEditorMainPanel)contentPane;
    private CtlHeaderBar HeaderBar { get; set; }
    private CtlTreeProject TreeProject { get; set; }
    private CtlDocumentView DocumentView { get; set; }
    private ModDataClipboard DataClipboard { get; set; }
        
    private ModWorkshop CurMod { get; set; }

    protected override void OnInit()
    {
        base.OnInit();
        DataClipboard = new ModDataClipboard();
            
        InitProject();
        InitHeader();
            
        FGUIUtils.BindHSeg(MainView.m_seg, () => MainView.width * 0.1f, () => MainView.width * 0.9f);
        MakeFullScreenAndCenter();
    }

    protected override void OnShown()
    {
        base.OnShown();

        WindowWaitDialog.CreateDialogAsync("提示", "正在初始化编辑器...", 0.5f,
            (callback, context) =>
            {
                // 预加载管理器
                UniTask.Create(async () =>
                {
                    await UniTask.SwitchToThreadPool();
                    try
                    {
                        ModEditorManager.I.Init();
                        ModEditorManager.I.LoadDefaultData();
                    }
                    catch (Exception e)
                    {
                        Main.LogError(e);
                        context.Exception = e;
                    }
                    finally
                    {
                        await UniTask.SwitchToMainThread();
                        callback();
                    }
                });
            }, 
            context =>
            {
                if (context.Exception != null)
                {
                    Hide();
                    if (context.Exception is JsonException jsonException)
                    {
                        WindowConfirmDialog.CreateDialog("提示", $"初始化失败！读取导出的游戏数据时发生错误，请尝试重新导出游戏数据（使用F4 -> 导出游戏数据 进行导出）\n错误信息：\n{context.Exception}", false);
                    }
                    else
                    {
                        WindowConfirmDialog.CreateDialog("提示", $"初始化失败！错误信息：\n{context.Exception}", false);
                    }
                }
                else
                {
                    if (!Directory.Exists(Main.PathExportOutputDir.Value))
                    {
                        WindowConfirmDialog.CreateDialog("提示", "没有找到游戏数据导出目录，缺失导出数据会影响编辑器使用，是否导出游戏数据？", true,
                            () =>
                            {
                                ModManager.GenerateBaseData();
                                ModEditorManager.I.LoadDefaultData();
                            });
                    }
                }
                    
            });
    }

    public void Clear()
    {
        TreeProject.RemoveAllProjects();
        DocumentView.RemoveAllTab();
    }

    protected override void OnHide()
    {
        Clear();
        base.OnHide();
    }

    #region Project

    private void InitProject()
    {
        HeaderBar = new CtlHeaderBar(MainView.m_comHeader);
        TreeProject = new CtlTreeProject(MainView.m_comProject);
        DocumentView = new CtlDocumentView(MainView.m_comDocument);

        TreeProject.SetClickItem((context, node) =>
        {
            if(!context.inputEvent.isDoubleClick)
                return;
                
            if(!node.IsLeaf)
                return;
                
            if(CurMod == null)
                return;
                
            if (node is IDocumentItem item)
            {
                DocumentView.TryAddAndSelectTab(item);
            }
        });
            
        TreeProject.SetRightClickItem((context, node) =>
        {
            var popup = ProjectPopupBuild(node);
            popup.Show(null, PopupDirection.Down);
        });
            
        DocumentView.OnTabAdd += (page) =>
        {
            if (page is IModDataClipboardPage clipboardPage)
            {
                clipboardPage.DataClipboard = DataClipboard;
            }
        };

        //TreeProject.ToolsBar.AddToolBtn("ui://NextCore/icon_add", "新建项目", OnCreateProject);
    }

    private PopupMenu ProjectPopupBuild(ProjectTreeNodeBase node)
    {
        var project = node as ProjectTreeModProject;
        var isProject = project != null;
        var popMenu = new PopupMenu();
            
        popMenu.AddItem("新建项目".I18NTodo(), OnCreateProject);
        popMenu.AddSeperator();
        if (isProject)
        {
            popMenu.AddItem("重命名".I18NTodo(), () => OnRenameProject(project.Project));
            popMenu.AddItem("删除".I18NTodo(), () => OnRemoveProject(project.Project));
        }
        else
        {
            popMenu.AddItem("重命名".I18NTodo(), () => {}).enabled = false;
            popMenu.AddItem("删除".I18NTodo(), () => {}).enabled = false;
                
        }
            
        return popMenu;
    }

    private void OnCreateProject()
    {
        if(CurMod == null)
            return;
            
        WindowStringInputDialog.CreateDialog("新建项目", "mod Project", true, 
            (newName) =>
            {
                if (string.IsNullOrEmpty(newName))
                {
                    WindowConfirmDialog.CreateDialog("创建失败", "项目名称不能为空", false);
                    return;
                }
                var newPath = CurMod.Path;
                if(Directory.Exists(newPath))
                {
                    WindowConfirmDialog.CreateDialog("创建失败", "项目目录已存在", false);
                    return;
                }
                var project = CurMod.CreateProject(newName);
                var modProjNode = new ProjectTreeModProject(CurMod, project);
                TreeProject.AddProject(modProjNode);
                TreeProject.Refresh();
            });
    }
        
    private void OnRemoveProject(ModProject project)
    {
        if(CurMod == null)
            return;
            
        WindowConfirmDialog.CreateDialog("删除", 
            $"确定删除项目【{project.ProjectName}】？该操作无法撤回。", 
            true,
            () =>
            {
                Directory.Delete(project.ProjectPath, true);
                CurMod.RemoveProject(project);
                TreeProject.RemoveProject(node => node is ProjectTreeModProject treeProject && treeProject.Project == project);
                TreeProject.Refresh();
            });
    }
        
    private void OnRenameProject(ModProject project)
    {
        if(CurMod == null)
            return;
            
        WindowStringInputDialog.CreateDialog("重命名", 
            project.ProjectName,
            true,
            rename =>
            {
                if (string.IsNullOrEmpty(rename))
                {
                    WindowConfirmDialog.CreateDialog("重命名失败", "项目名称不能为空", false);
                    return;
                }
                if (CurMod.Projects.Exists(p => p.ProjectDirectory == rename))
                {
                    WindowConfirmDialog.CreateDialog("重命名失败", "目标目录已存在", false);
                    return;
                }
                var oldPath = project.ProjectPath;
                var newPath = $"{CurMod.ProjectDirPath}/{rename}";
                Directory.Move(oldPath, newPath);
                project.ProjectPath = newPath;
                TreeProject.Refresh();
            });
    }

    #endregion
        
    #region HeadbarFunction
        
    private void InitHeader()
    {
        // Root
        HeaderBar.AddMenu("ModEditor.Main.Header.File".I18N(), FilePopupBuild);
        HeaderBar.AddMenu("工具".I18NTodo(), ToolPopupBuild);
    }

    private PopupMenu FilePopupBuild()
    {
        var popMenu = new PopupMenu();
        popMenu.AddItem("新建工程".I18NTodo(), OnClickFileCreate);
        popMenu.AddItem("ModEditor.Main.Header.File.Open".I18N(), OnClickFileOpen);
        var btnSave = popMenu.AddItem("ModEditor.Main.Header.File.Save".I18N(), OnClickFileSave);
        var btnExport = popMenu.AddItem("ModEditor.Main.Header.File.Export".I18N(), OnClickFileExport);
        popMenu.AddItem("ModEditor.Main.Header.File.Exit".I18N(), OnClickFileExit);
            
        if (CurMod == null)
        {
            btnSave.enabled = false;
            btnExport.enabled = false;
        }
            
        return popMenu;
    }
    
    private PopupMenu ToolPopupBuild()
    {
        var popMenu = new PopupMenu();
        popMenu.AddItem("创建AB包工程".I18NTodo(), OnClickCreateABProject);

        return popMenu;
    }

    private void OnClickCreateABProject()
    {
        new WindowCreateABProjectDialogBuilder()
            .Build()
            .Show();
    }

    private void OnClickFileExit()
    {
        WindowConfirmDialog.CreateDialog("确认退出", "确定要退出编辑器吗？请确认数据是否保存。", true, Hide);
    }

    private void OnClickFileCreate()
    {
        new WindowCreateWorkshopModDialogBuilder()
            .SetTitle("新建工坊工程".I18NTodo())
            .SetItemList(new []
            {
                new ProjectListCreateWorkshopEmpty()
            })
            .SetOnSelectMod(OnOpenMod)
            .Build()
            .Show();
    }

    private void OnClickFileOpen()
    {
        WindowModSelectorDialog.CreateDialog("打开工坊工程".I18NTodo(),
            ModFilter.Local, 
            new List<TableInfo>()
            {
                new TableInfo("名称", TableInfo.DEFAULT_GRID_WIDTH * 3, o => ((ModWorkshop)o).ModInfo.Title),
                new TableInfo("位置", TableInfo.DEFAULT_GRID_WIDTH * 6, o => ((ModWorkshop)o).Path),
            },
            OnOpenMod);
    }
        
    private void OnClickFileSave()
    {
        if(CurMod == null)
            return;
            
        OnSaveMod(CurMod, CurMod.Path);
    }
        
    private void OnClickFileExport()
    {
        if(CurMod == null)
            return;
        //
        // var dialog = new FolderPicker();
        // dialog.Title = "导出工坊工程";
        // dialog.InputPath = Main.PathLocalModsDir.Value;
        // if (dialog.ShowDialog(IntPtr.Zero) == true)
        // {
        //     
        // }
            
        var folderPicker = new FolderPicker();
        folderPicker.Title = "Mod选择文件夹";
        bool? flag = folderPicker.ShowDialog(IntPtr.Zero, false);
        if (flag == null || !flag.Value)
        {
            return;
        }
        string resultPath = folderPicker.ResultPath;
        OnSaveMod(CurMod, resultPath);
        Main.LogInfo("Mod导出路径：" + resultPath);
    }

    private void OnOpenMod(ModWorkshop mod)
    {
        Clear();

        try
        {
            CurMod = mod;
            CurMod.LoadProjects();

            TreeProject.AddProject(new ProjectTreeModWorkshop(CurMod));
            TreeProject.AddProject(new ProjectTreeModProjectReferenced(CurMod, ModEditorManager.I.ReferenceProject));
            foreach (var project in mod.Projects)
            {
                var modProjNode = new ProjectTreeModProject(CurMod, project);
                TreeProject.AddProject(modProjNode);
            }

            TreeProject.Refresh();
        }
        catch (ModOpenException e)
        {
            WindowConfirmDialog.CreateDialog("工程打开失败", $"打开工程失败，错误日志：\n{e.Message}", false);
            Clear();
        }
        catch (Exception e)
        {
            WindowConfirmDialog.CreateDialog("工程打开失败", $"打开工程失败，错误日志：\n{e}", false);
            Clear();
        }
    }

    private void OnSaveMod(ModWorkshop mod, string path)
    {
        WindowWaitDialog.CreateDialog("提示", "正在保存Mod...", 0.5f,
            context =>
            {
                try
                {
                    ModEditorManager.I.SaveWorkshop(mod, path);
                }
                catch (Exception e)
                {
                    context.Exception = e;
                }
            },
            context =>
            {
                if (context.Exception != null)
                {
                    WindowConfirmDialog.CreateDialog("提示", $"Mod保存失败！错误信息：\n{context.Exception}", false);
                }
                else
                {
                    WindowConfirmDialog.CreateDialog("提示", "保存完毕！", false);
                }
            });
    }

    #endregion
}