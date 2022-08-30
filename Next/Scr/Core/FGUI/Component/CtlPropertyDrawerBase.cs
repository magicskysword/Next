using System;
using FairyGUI;

namespace SkySwordKill.Next.FGUI.Component
{
    public abstract class CtlPropertyDrawerBase : IPropertyDrawer
    {
        private bool _editable = true;

        public bool Editable
        {
            get { return _editable; }
            set
            {
                _editable = value;
                SetDrawerEditable(value);
            }
        }

        public Action OnChanged { get; set; } = () => { };
        public GComponent Component { get; set; }
        protected abstract GComponent OnCreateCom();
        protected virtual void OnRemoveCom(GComponent component) { }
        protected virtual void OnRefresh() { }
        protected abstract void SetDrawerEditable(bool value);
        
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