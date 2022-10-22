using System;
using System.Collections.Generic;
using FairyGUI;
using SkySwordKill.Next.Extension;
using SkySwordKill.Next.ModGUI;
using SkySwordKill.NextModEditor.Mod;
using SkySwordKill.NextModEditor.PanelProject;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.Next.FGUI.Component;

public delegate void OnClickTreeProjectItem(EventContext context, ProjectTreeNodeBase node);
public delegate void OnRenderTreeProjectItem(GTreeNode treeNode, ProjectTreeNodeBase node);

public class CtlTreeProject
{
    public CtlTreeProject(UI_ComTreeProject projectTree)
    {
        ProjectTree = projectTree;
            
        var treeView = ProjectTree.m_treeView;
        treeView.treeNodeRender = ProjectTreeItemRenderer;
        treeView.onClickItem.Set(OnClickProjectTreeItem);
        treeView.onRightClickItem.Set(OnRightClickProjectTreeItem);
            
        ToolsBar = new CtlToolsBar(ProjectTree.m_toolsBar);
    }

    protected List<ProjectTreeNodeBase> Nodes { get; } = new List<ProjectTreeNodeBase>();

    public UI_ComTreeProject ProjectTree { get; }
    public CtlToolsBar ToolsBar { get; }
        
    protected OnClickTreeProjectItem OnClickItem { get; set; }
    protected OnClickTreeProjectItem OnRightClickItem { get; set; }
    protected OnRenderTreeProjectItem OnRenderItem { get; set; }

    public void AddProject(ProjectTreeNodeBase nodeData)
    {
        Nodes.Add(nodeData);
        ProjectTree.m_treeView.rootNode.AddChild(nodeData.BindNode);
    }
        
    public void RemoveProject(Func<ProjectTreeNodeBase, bool> comparator)
    {
        foreach (var node in Nodes)
        {
            if (comparator(node))
            {
                Nodes.Remove(node);
                ProjectTree.m_treeView.rootNode.RemoveChild(node.BindNode);
                return;
            }
        }
    }
    
    public void RemoveProject(ProjectTreeNodeBase nodeData)
    {
        RemoveProject(node => node == nodeData);
    }

    public void RemoveAllProjects()
    {
        Nodes.Clear();
            
        ProjectTree.m_treeView.rootNode.RemoveChildren();
    }
    
    public void MoveProjectUp(ProjectTreeNodeBase modGroup)
    {
        var index = Nodes.IndexOf(modGroup);
        if (index == 0)
        {
            return;
        }
            
        Nodes.Remove(modGroup);
        Nodes.Insert(index - 1, modGroup);
            
        ProjectTree.m_treeView.rootNode.RemoveChild(modGroup.BindNode);
        ProjectTree.m_treeView.rootNode.AddChildAt(modGroup.BindNode, index - 1);
    }
    
    public void MoveProjectDown(ProjectTreeNodeBase modGroup)
    {
        var index = Nodes.IndexOf(modGroup);
        if (index == Nodes.Count - 1)
        {
            return;
        }
            
        Nodes.Remove(modGroup);
        Nodes.Insert(index + 1, modGroup);
            
        ProjectTree.m_treeView.rootNode.RemoveChild(modGroup.BindNode);
        ProjectTree.m_treeView.rootNode.AddChildAt(modGroup.BindNode, index + 1);
    }

    public void Refresh()
    {
        ProjectTree.m_treeView.rootNode.RemoveChildren();
        foreach (var node in Nodes)
        {
            ProjectTree.m_treeView.rootNode.AddChild(node.BindNode);
        }
    }

    public void SetClickItem(OnClickTreeProjectItem action)
    {
        OnClickItem = action;
    }

    public void SetRightClickItem(OnClickTreeProjectItem action)
    {
        OnRightClickItem = action;
    }
    
    public void SetRenderItem(OnRenderTreeProjectItem action)
    {
        OnRenderItem = action;
    }

    private void OnClickProjectTreeItem(EventContext context)
    {
        var obj = (GObject)context.data;
        var node = obj.treeNode;
            
        var uiProjectBase = (ProjectTreeNodeBase)node.data;

        OnClickItem?.Invoke(context ,uiProjectBase);
    }
        
    private void OnRightClickProjectTreeItem(EventContext context)
    {
        var obj = (GObject)context.data;
        var node = obj.treeNode;
            
        var uiProjectBase = (ProjectTreeNodeBase)node.data;

        OnRightClickItem?.Invoke(context ,uiProjectBase);
    }
        
    private void ProjectTreeItemRenderer(GTreeNode node, GComponent obj)
    {
        var nodeBase = (ProjectTreeNodeBase)node.data;

        obj.text = nodeBase.Name;
        if (!string.IsNullOrEmpty(nodeBase.Icon))
        {
            obj.icon = nodeBase.Icon;
        }
        else
        {
            if (!node.isFolder)
            {
                obj.icon = "ui://NextCore/icon_file1";
            }
            else
            {
                obj.icon = "ui://NextCore/icon_folder1";
            }
        }

        OnRenderItem?.Invoke(node, nodeBase);
    }

    public GTreeNode GetSelectedNode()
    {
        return ProjectTree.m_treeView.GetSelectedNode();
    }

    public void SelectNode(ProjectTreeNodeBase node)
    {
        ProjectTree.m_treeView.ClearSelection();
        ProjectTree.m_treeView.SelectNode(node.BindNode);
    }
}