using System;
using System.Collections.Generic;
using FairyGUI;
using SkySwordKill.Next.Extension;
using SkySwordKill.NextEditor.Mod;
using SkySwordKill.NextEditor.PanelProject;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.Next.FGUI.Component
{
    public delegate void OnClickTreeProjectItem(EventContext context, ProjectTreeNodeBase node);
    
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

        private List<GTreeNode> _projectNodes = new List<GTreeNode>();
        
        public UI_ComTreeProject ProjectTree { get; }
        public CtlToolsBar ToolsBar { get; }
        
        private OnClickTreeProjectItem OnClickItem { get; set; }
        private OnClickTreeProjectItem OnRightClickItem { get; set; }

        public void AddProject(ProjectTreeNodeBase nodeData, GTreeNode parent = null)
        {
            var node = new GTreeNode(!nodeData.IsLeaf);
            node.data = nodeData;
            node.expanded = true;

            if(parent == null)
                _projectNodes.Add(node);
            else
                parent.AddChild(node);
            
            if (nodeData.Children != null)
            {
                foreach (var childNode in nodeData.Children)
                {
                    AddProject(childNode, node);
                }
            }
        }
        
        public void RemoveProject(Func<ProjectTreeNodeBase, bool> comparator)
        {
            foreach (var projectNode in _projectNodes)
            {
                var nodeData = projectNode.data as ProjectTreeNodeBase;
                if (comparator(nodeData))
                {
                    _projectNodes.Remove(projectNode);
                    return;
                }
            }
        }

        public void RemoveAllProjects()
        {
            _projectNodes.Clear();
            
            ProjectTree.m_treeView.rootNode.RemoveChildren();
        }

        public void Refresh()
        {
            ProjectTree.m_treeView.rootNode.RemoveChildren();
            foreach (var node in _projectNodes)
            {
                ProjectTree.m_treeView.rootNode.AddChild(node);
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
            var uiProjectBase = (ProjectTreeNodeBase)node.data;
            var treeItem = obj.asButton;

            treeItem.title = uiProjectBase.Name;
            if (uiProjectBase is ProjectTreeNodeBase nodeBase)
            {
                if (!string.IsNullOrEmpty(nodeBase.Icon))
                {
                    treeItem.icon = nodeBase.Icon;
                }
                else
                {
                    if (nodeBase.IsLeaf)
                    {
                        treeItem.icon = "ui://NextCore/icon_file1";
                    }
                    else
                    {
                        treeItem.icon = "ui://NextCore/icon_folder1";
                    }
                }
            }
        }
    }
}