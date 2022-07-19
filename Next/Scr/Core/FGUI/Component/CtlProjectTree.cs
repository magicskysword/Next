using System;
using System.Collections.Generic;
using FairyGUI;
using SkySwordKill.Next.Extension;
using SkySwordKill.NextEditor.PanelProject;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.Next.FGUI.Component
{
    public class CtlProjectTree
    {
        public CtlProjectTree(UI_ComMainProject projectTree)
        {
            ProjectTree = projectTree;
            
            var treeView = ProjectTree.m_treeView;
            treeView.treeNodeRender = ProjectTreeItemRenderer;
            treeView.onClickItem.Set(OnClickProjectTreeItem);
            
            AddProject("ModEditor.Main.project.modConfig".I18N(),0,new ProjectTreeItemModConfig());
            AddProject("ModEditor.Main.project.modCreateAvatar".I18N(),0,new ProjectTreeItemModCreateAvatar());
            AddProject("ModEditor.Main.project.modBuffInfo".I18N(),0,new ProjectTreeItemModBuffInfo());
            AddProject("剧情预览",0,new ProjectTreeItemBaseFungus());
        }

        private List<ProjectTreeNodeBase> _projectNodes = new List<ProjectTreeNodeBase>();
        
        public UI_ComMainProject ProjectTree { get; }
        
        public Action<EventContext, ProjectTreeNodeBase> OnClickItem { get; set; }

        public void AddProject(string projName, int projLayer, ProjectTreeNodeBase nodeData)
        {
            nodeData.Name = projName;
            nodeData.Layer = projLayer;
            _projectNodes.Add(nodeData);

            var node = new GTreeNode(!nodeData.IsLeaf);
            node.data = nodeData;
            node.expanded = true;

            ProjectTree.m_treeView.rootNode.AddChild(node);
        }

        public void ClearProject()
        {
            _projectNodes.Clear();
            
            ProjectTree.m_treeView.rootNode.RemoveChildren();
        }

        private void OnClickProjectTreeItem(EventContext context)
        {
            var obj = (GObject)context.data;
            var node = obj.treeNode;
            
            var uiProjectBase = (ProjectTreeNodeBase)node.data;

            OnClickItem?.Invoke(context ,uiProjectBase);
        }
        
        private void ProjectTreeItemRenderer(GTreeNode node, GComponent obj)
        {
            var uiProjectBase = (ProjectTreeNodeBase)node.data;
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
    }
}