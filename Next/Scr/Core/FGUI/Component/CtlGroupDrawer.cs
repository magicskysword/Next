using System.Collections.Generic;
using FairyGUI;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.Next.FGUI.Component
{
    public class CtlGroupDrawer : CtlPropertyDrawerBase
    {
        public CtlGroupDrawer(string title,bool isExpend ,params CtlPropertyDrawerBase[] drawers)
        {
            DrawerName = title;
            IsExpand = isExpend;
            Drawers.AddRange(drawers);
        }

        protected override GComponent OnCreateCom()
        {
            var drawer = UI_ComGroupDrawer.CreateInstance();
            drawer.title = DrawerName;
            drawer.onSizeChanged.Add(OnSizeChanged);
            Inspector = new CtlGroupInspector(drawer.m_list);
            foreach (var propertyDrawer in Drawers)
            {
                propertyDrawer.UndoManager = UndoManager;
                Inspector.AddDrawer(propertyDrawer);
            }
            var btnExpand = drawer.m_btnExpand;
            btnExpand.selected = IsExpand;
            btnExpand.onClick.Add(() =>
            {
                SetExpand(btnExpand.selected);
            });
            return drawer;
        }

        private List<CtlPropertyDrawerBase> Drawers { get; } = new List<CtlPropertyDrawerBase>();
        private string DrawerName { get; set; }
        private bool IsExpand { get; set; }
        private UI_ComGroupDrawer Drawer => (UI_ComGroupDrawer)Component;
        private CtlGroupInspector Inspector { get; set; }

        protected override void SetDrawerEditable(bool value)
        {
            Inspector.Editable = value;
            Inspector.Refresh();
        }

        protected override void OnRefresh()
        {
            base.OnRefresh();
            if (IsExpand)
            {
                Inspector.Show();
            }
            else
            {
                Inspector.Hide();
            }
        }
        
        private void OnSizeChanged()
        {
            Refresh();
        }
        
        private void SetExpand(bool isExpand)
        {
            IsExpand = isExpand;
            Refresh();
        }
    }
}