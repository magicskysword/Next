using System;
using FairyGUI;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.NextEditor.Drawer
{
    public abstract class ModPropertyDrawer : IPropertyDrawer
    {
        public Action OnChanged;
        public GComponent Component { get; set; }
        protected abstract GComponent OnCreateCom();
        protected virtual void OnRemoveCom(GComponent component) { }
        protected virtual void OnRefresh() { }

        public GComponent CreateCom()
        {
            if(Component == null)
            {
                Component = OnCreateCom();
            }
            return Component;
        }

        public void RemoveCom()
        {
            if(Component != null)
            {
                OnRemoveCom(Component);
                Component.Dispose();
            }

            Component = null;
        }

        public void Refresh()
        {
            OnRefresh();
        }
    }
}