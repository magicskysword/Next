using System.Collections.Generic;

namespace SkySwordKill.NextFGUI.NextCore
{
    public partial class UI_ComMainInspector
    {
        private List<IPropertyDrawer> _drawers = new List<IPropertyDrawer>();

        public void AddDrawer(IPropertyDrawer drawer)
        {
            _drawers.Add(drawer);
            m_list.AddChild(drawer.CreateCom());
        }

        public void Clear()
        {
            m_list.RemoveChildren();
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