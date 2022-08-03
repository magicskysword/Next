using System;
using System.Collections.Generic;
using FairyGUI;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.Next.FGUI.Component
{
    public delegate void OnClickListProjectItem(EventContext context, ProjectListBase item);
    
    public class CtlListProject
    {
        public CtlListProject(UI_ComListProject projectList)
        {
            ProjectList = projectList;
            
            var list = ProjectList.m_list;
            list.onClickItem.Set(OnClickProjectTreeItem);
            list.onRightClickItem.Set(OnRightClickProjectTreeItem);
            
            ToolsBar = new CtlToolsBar(ProjectList.m_toolsBar);
        }
        
        private List<ProjectListBase> _projectItems = new List<ProjectListBase>();

        public UI_ComListProject ProjectList { get; }
        public CtlToolsBar ToolsBar { get; }
        
        private OnClickListProjectItem OnClickItem { get; set; }
        private OnClickListProjectItem OnRightClickItem { get; set; }
        
        public void SetClickItem(OnClickListProjectItem action)
        {
            OnClickItem = action;
        }

        public void SetRightClickItem(OnClickListProjectItem action)
        {
            OnRightClickItem = action;
        }
        
        public void AddProject(ProjectListItemBase item)
        {
            _projectItems.Add(item);
        }
        
        public void Refresh()
        {
            ProjectList.m_list.RemoveChildrenToPool();
            for (var index = 0; index < _projectItems.Count; index++)
            {
                var item = _projectItems[index];
                GObject obj;
                switch (item)
                {
                    case ProjectListSeparator _:
                    {
                        obj = ProjectList.m_list.AddItemFromPool("ui://NextCore/LabGroupTitle");
                        break;
                    }
                    default:
                        obj = ProjectList.m_list.AddItemFromPool();
                        break;
                }

                obj.icon = string.Empty;

                var listBase = _projectItems[index];
                obj.text = listBase.Name;
                Main.LogInfo($"加载 {listBase.Name}");
                if (!string.IsNullOrEmpty(listBase.Icon))
                {
                    obj.icon = listBase.Icon;
                }
                else
                {
                    if (listBase.CanClick)
                    {
                        obj.icon = "ui://NextCore/icon_file1";
                    }
                }
            }
        }
        
        private void OnRightClickProjectTreeItem(EventContext context)
        {
            var obj = (GObject)context.data;
            if (obj.data is ProjectListBase listItem && listItem.CanClick)
            {
                OnClickItem?.Invoke(context, listItem);
            }
        }

        private void OnClickProjectTreeItem(EventContext context)
        {
            var obj = (GObject)context.data;
            if (obj.data is ProjectListBase listItem && listItem.CanClick)
            {
                OnRightClickItem?.Invoke(context, listItem);
            }
        }
    }
}