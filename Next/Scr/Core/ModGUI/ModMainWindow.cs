using FairyGUI;
using SkySwordKill.Next.Extension;
using SkySwordKill.Next.FGUI;
using SkySwordKill.Next.FGUI.Component;
using SkySwordKill.Next.FGUI.Dialog;
using SkySwordKill.Next.Mod;
using SkySwordKill.NextFGUI.NextCore;
using UnityEngine;

namespace SkySwordKill.Next.ModGUI;

public class ModMainWindow : FGUIWindowBase
{
    public ModMainWindow() : base("NextCore", "ModMainPanel")
    {
        
    }
    
    private bool _modLoadSettingDirty;
    private bool _modDataSettingDirty;

    public UI_ModMainPanel MainView => (UI_ModMainPanel)contentPane;
    public CtlTreeProject ModTree { get; set; }
    public CtlPropertyInspector Inspector { get; set; }

    /// <summary>
    /// Mod加载设置脏标记
    /// 为True时，Mod加载设置发生了改变，需要重载所有Mod
    /// </summary>
    public bool ModLoadSettingDirty
    {
        get => _modLoadSettingDirty;
        set
        {
            _modLoadSettingDirty = value;
            RefreshTitle();
        }
    }

    public bool ModDataSettingDirty
    {
        get => _modDataSettingDirty;
        set
        {
            _modDataSettingDirty = value;
            RefreshTitle();
        }
    }
    
    public bool ModSettingDirty => ModLoadSettingDirty || ModDataSettingDirty;

    protected override void OnInit()
    {
        base.OnInit();

        modal = true;
        
        _modLoadSettingDirty = false;
        _modDataSettingDirty = false;
        RefreshTitle();
        
        MainView.m_frame.m_closeButton.onClick.Add(OnClickClose);
        MainView.m_btnApply.onClick.Add(OnClickApply);
        MainView.m_btnApply.text = "应用".I18NTodo();
        
        InitProject();
        InitInspector();
        InitLink();
        
        FGUIUtils.BindHSeg(MainView.m_seg, () => MainView.width * 0.2f, () => MainView.width * 0.8f);
            
        MakeFullScreenAndCenter(0.8f);
        
        Refresh();
    }

    protected override void OnHide()
    {
        base.OnHide();
        ModManager.LoadSetting();
    }

    private void RefreshTitle()
    {
        if(!ModSettingDirty)
            frame.asLabel.title = $"Next  v{Main.MOD_VERSION}";
        else
            frame.asLabel.title = $"* Next  v{Main.MOD_VERSION}";
    }

    private void InitInspector()
    {
        Inspector = new CtlPropertyInspector(MainView.m_inspector);
        Inspector.OnPropertyChanged += () =>
        {
            ModDataSettingDirty = true;
        };
    }

    private void InitProject()
    {
        ModTree = new CtlTreeProject(MainView.m_mods);
        ModTree.SetRenderItem(OnRenderTreeItem);
        ModTree.SetClickItem(OnInspectMod);
        ModTree.ToolsBar.AddToolBtn("ui://NextCore/icon_reset", "刷新", Refresh);
        ModTree.ToolsBar.AddToolSep();
        ModTree.ToolsBar.AddToolBtn("ui://NextCore/icon_arrow1", "上移", OnClickMoveUp);
        ModTree.ToolsBar.AddToolBtn("ui://NextCore/icon_arrow2", "下移", OnClickMoveDown);
        ModTree.ToolsBar.AddToolSep();
        ModTree.ToolsBar.AddToolBtn("ui://NextCore/icon_true", "启用全部", OnClickEnableAllMod);
        ModTree.ToolsBar.AddToolBtn("ui://NextCore/icon_false", "禁用全部", OnClickDisableAllMod);
    }

    private void InitLink()
    {
        var list = MainView.m_listLink;
        var item = list.AddItemFromPool();
        item.text = "觅长生 Wiki";
        item.onClick.Add(() => Application.OpenURL("https://wiki.biligame.com/mcs/%E9%A6%96%E9%A1%B5"));
        item.cursor = FGUIManager.MOUSE_HAND;
        
        item = list.AddItemFromPool();
        item.text = "Next Mod文档";
        item.onClick.Add(() => Application.OpenURL("https://wiki.biligame.com/mcs/Next%E9%A6%96%E9%A1%B5"));
        item.cursor = FGUIManager.MOUSE_HAND;
    }
    
    private void Refresh()
    {
        ModManager.ReloadModMeta(false);
        Inspector.Clear();
        
        ModTree.RemoveAllProjects();
        foreach (var modGroup in ModManager.modGroups)
        {
            var projectGroup = new ProjectTreeFolderModGroup(modGroup);
            projectGroup.RefreshModConfigs(true);
            ModTree.AddProject(projectGroup);
        }
        
        RefreshTitle();
    }
    
    private void OnInspectMod(EventContext context, ProjectTreeNodeBase node)
    {
        Inspector.Clear();
        if(node is ProjectTreeItemModConfig modItem)
        {
            modItem.OnInspector(Inspector);
        }
        else if(node is ProjectTreeFolderModGroup modGroup)
        {
            modGroup.OnInspector(Inspector);
        }
        Inspector.Refresh();
    }

    private void OnRenderTreeItem(GTreeNode gTreeNode, ProjectTreeNodeBase node)
    {
        if (node is ProjectTreeItemModConfig modItem)
        {
            var item = (UI_BtnTreeItemMod)gTreeNode.cell;
            var modSetting = Main.I.NextModSetting.GetOrCreateModSetting(modItem.ModConfig);
            item.m_tglEnable.selected = modSetting.enable;
            if (modItem.ModGroup.Type == ModType.Local)
            {
                item.m_tglEnable.enabled = true;
                item.m_tglEnable.onChanged.Set(() =>
                {
                    ModManager.ModSetEnable(modItem.ModConfig, item.m_tglEnable.selected);
                    ModLoadSettingDirty = true;
                });
            }
            else
            {
                item.m_tglEnable.enabled = false;
                item.m_tglEnable.onChanged.Clear();
            }
        }
    }
    
    private void OnClickClose(EventContext context)
    {
        if (ModSettingDirty)
        {
            WindowConfirmDialog.CreateDialog("提示", "检测到Mod设置已经发生变更，是否不保存并退出？", true,Hide);
        }
        else
        {
            Hide();
        }
    }
    
    private void OnClickApply(EventContext context)
    {
        WindowConfirmDialog.CreateDialog("提示", "是否应用更改？", true,() =>
        {
            WindowWaitDialog.CreateDialog("提示","正在应用更改...", 1f, 
                context =>
            {
                ModManager.SaveSetting();
                if (ModLoadSettingDirty)
                {
                    ModManager.ReloadAllMod();
                }
            }, 
                context=>
            {
                _modLoadSettingDirty = false;
                _modDataSettingDirty = false;
                Refresh();
            });
        });
    }
    
    private ProjectTreeNodeBase GetSelectedNode()
    {
        var curSelectedNode = ModTree.GetSelectedNode();
        return curSelectedNode?.data as ProjectTreeNodeBase;
    }
    
    private void OnClickMoveUp()
    {
        var node = GetSelectedNode();
        if(node is ProjectTreeItemModConfig modItem)
        {
            modItem.ModGroup.MoveModUp(modItem.ModConfig);
            var nodeParent = (ProjectTreeFolderModGroup)modItem.Parent;
            nodeParent.RefreshModConfigs();
            ModLoadSettingDirty = true;
        }
        else if(node is ProjectTreeFolderModGroup modGroup)
        {
            ModManager.ModGroupMoveUp(modGroup.ModGroup);
            ModTree.MoveProjectUp(modGroup);
            ModLoadSettingDirty = true;
        }
        
        if(node != null)
        {
            ModTree.SelectNode(node);
        }
    }

    private void OnClickMoveDown()
    {
        var node = GetSelectedNode();
        if(node is ProjectTreeItemModConfig modItem)
        {
            modItem.ModGroup.MoveModDown(modItem.ModConfig);
            var nodeParent = (ProjectTreeFolderModGroup)modItem.Parent;
            nodeParent.RefreshModConfigs();
            ModLoadSettingDirty = true;
        }
        else if(node is ProjectTreeFolderModGroup modGroup)
        {
            ModManager.ModGroupMoveDown(modGroup.ModGroup);
            ModTree.MoveProjectDown(modGroup);
            ModLoadSettingDirty = true;
        }
        
        if(node != null)
        {
            ModTree.SelectNode(node);
        }
    }
    
    private void OnClickDisableAllMod()
    {
        ModManager.ModSetEnableAll(false);
        Refresh();
        ModLoadSettingDirty = true;
    }

    private void OnClickEnableAllMod()
    {
        ModManager.ModSetEnableAll(true);
        Refresh();
        ModLoadSettingDirty = true;
    }
}