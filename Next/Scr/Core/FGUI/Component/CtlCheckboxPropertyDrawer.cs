using System;
using FairyGUI;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.Next.FGUI.Component
{
    public class CtlCheckboxPropertyDrawer : CtlPropertyDrawerBase
    {
        public UI_ComCheckboxDrawer Drawer => (UI_ComCheckboxDrawer)Component;
        
        private string _drawerName;
        private Action<bool> _setter;
        private Func<bool> _getter;
        
        public CtlCheckboxPropertyDrawer(string drawerName,Action<bool> setter,Func<bool> getter)
        {
            _drawerName = drawerName;
            _setter = setter;
            _getter = getter;
        }
        
        protected override GComponent OnCreateCom()
        {
            var drawer = UI_ComCheckboxDrawer.CreateInstance();
            drawer.title = _drawerName;
            drawer.m_checkbox.onChanged.Add(OnClickCheckbox);
            return drawer;
        }

        private void OnClickCheckbox(EventContext context)
        {
            OnSetProperty(Drawer.m_checkbox.selected);
        }

        private void OnSetProperty(bool b)
        {
            _setter.Invoke(b);
            OnChanged?.Invoke();
        }

        private bool OnGetProperty()
        {
            return _getter.Invoke();
        }

        protected override void OnRefresh()
        {
            Drawer.m_checkbox.selected = OnGetProperty();
        }
    }
}