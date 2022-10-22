using FairyGUI;
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
    
    public UI_ModMainPanel MainView => (UI_ModMainPanel)contentPane;
    public CtlTreeProject ModTree { get; set; }
    public CtlPropertyInspector Inspector { get; set; }

    public bool SettingDirty { get; set; } = false;

    protected override void OnInit()
    {
        base.OnInit();

        modal = true;
        frame.asLabel.title = $"Next  v{Main.MOD_VERSION}";
        
        SettingDirty = false;
        
        MainView.m_frame.m_closeButton.onClick.Add(OnClickClose);
        MainView.m_btnApply.onClick.Add(OnClickApply);
        InitProject();
        InitInspector();
        InitLink();
        
        FGUIUtils.BindHSeg(MainView.m_seg, () => MainView.width * 0.2f, () => MainView.width * 0.8f);
            
        MakeFullScreenAndCenter(0.8f);
        
        Refresh();
    }

    private void InitInspector()
    {
        Inspector = new CtlPropertyInspector(MainView.m_inspector);
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
            projectGroup.RefreshModConfigs();
            ModTree.AddProject(projectGroup);
        }
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
                    SettingDirty = true;
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
        Hide();
    }
    
    private void OnClickApply(EventContext context)
    {
        WindowConfirmDialog.CreateDialog("提示", "是否应用更改？", true,() =>
        {
            WindowWaitDialog.CreateDialog("提示","正在应用更改...", 1f, 
                context =>
            {
                ModManager.ReloadAllMod();
            }, 
                context=>
            {
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
            SettingDirty = true;
        }
        else if(node is ProjectTreeFolderModGroup modGroup)
        {
            ModManager.ModGroupMoveUp(modGroup.ModGroup);
            ModTree.MoveProjectUp(modGroup);
            SettingDirty = true;
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
            SettingDirty = true;
        }
        else if(node is ProjectTreeFolderModGroup modGroup)
        {
            ModManager.ModGroupMoveDown(modGroup.ModGroup);
            ModTree.MoveProjectDown(modGroup);
            SettingDirty = true;
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
        SettingDirty = true;
    }

    private void OnClickEnableAllMod()
    {
        ModManager.ModSetEnableAll(true);
        Refresh();
        SettingDirty = true;
    }
}