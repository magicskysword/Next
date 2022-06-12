using System;
using FairyGUI;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.Next.FGUI.ComponentCtl
{
    public class CtlIntPropertyDrawer : CtlPropertyDrawerBase
    {
        private string _drawerName;
        private UI_ComIntDrawer Drawer => (UI_ComIntDrawer)Component;
        private Action<int> _setter;
        private Func<int> _getter;

        public CtlIntPropertyDrawer(string drawerName,Action<int> setter,Func<int> getter)
        {
            _drawerName = drawerName;
            _setter = setter;
            _getter = getter;
        }
    
        protected override GComponent OnCreateCom()
        {
            var drawer = UI_ComIntDrawer.CreateInstance();
            drawer.BindEndEdit(OnSetProperty);
            drawer.title = _drawerName;
            return drawer;
        }

        protected override void OnRefresh()
        {
            Drawer.m_inContent.text = OnGetProperty().ToString();
        }
    
        private void OnSetProperty(int text)
        {
            _setter.Invoke(text);
            OnChanged?.Invoke();
        }

        private int OnGetProperty()
        {
            return _getter.Invoke();
        }
    }
}