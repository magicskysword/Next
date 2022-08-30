using System.Collections.Generic;
using FairyGUI;

namespace SkySwordKill.Next.FGUI.Component
{
    public abstract class CtlInspectorBase
    {
        protected abstract GList _drawerGList { get; }
        protected List<IPropertyDrawer> _drawers = new List<IPropertyDrawer>();
        
        public bool Editable { get; set; } = true;

        public void AddDrawer(IPropertyDrawer drawer)
        {
            _drawers.Add(drawer);
            _drawerGList.AddChild(drawer.CreateCom());
            OnAddDrawer(drawer);
        }

        public void Clear()
        {
            _drawerGList.RemoveChildren();
            foreach (var drawer in _drawers)
            {
                drawer.RemoveCom();
            }
            _drawers.Clear();
            OnClear();
        }

        public void Refresh()
        {
            foreach (var drawer in _drawers)
            {
                drawer.Refresh();
                drawer.Editable = Editable;
            }
            OnRefresh();
        }
        
        protected virtual void OnAddDrawer(IPropertyDrawer drawer)
        {
            
        }
        
        protected virtual void OnClear()
        {
            
        }

        protected virtual void OnRefresh()
        {
            
        }
    }
}