using System.Collections.Generic;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.Next.FGUI.ComponentCtl
{
    public class CtlPropertyInspector
    {
        public CtlPropertyInspector(UI_ComMainInspector uiCom)
        {
            MainView = uiCom;
        }
        
        public UI_ComMainInspector MainView;
            
        private List<IPropertyDrawer> _drawers = new List<IPropertyDrawer>();

        public void AddDrawer(IPropertyDrawer drawer)
        {
            _drawers.Add(drawer);
            MainView.m_list.AddChild(drawer.CreateCom());
        }

        public void Clear()
        {
            MainView.m_list.RemoveChildren();
            foreach (var drawer in _drawers)
            {
                drawer.RemoveCom();
            }
            _drawers.Clear();
        }

        public void Refresh()
        {
            foreach (var drawer in _drawers)
            {
                drawer.Refresh();
            }
        }
    }
}