using System;
using FairyGUI;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.Next.FGUI.Component
{
    public class CtlStringPropertyDrawer : CtlPropertyDrawerBase
    {
        private string _drawerName;
        private UI_ComStringDrawer Drawer => (UI_ComStringDrawer)Component;
        private Action<string> _setter;
        private Func<string> _getter;

        public CtlStringPropertyDrawer(string drawerName,Action<string> setter,Func<string> getter)
        {
            _drawerName = drawerName;
            _setter = setter;
            _getter = getter;
        }
    
        protected override GComponent OnCreateCom()
        {
            var drawer = UI_ComStringDrawer.CreateInstance();
            drawer.m_inContent.onFocusOut.Add(() => OnSetProperty(drawer.m_inContent.text));
            drawer.title = _drawerName;
            return drawer;
        }

        protected override void OnRefresh()
        {
            Drawer.m_inContent.text = OnGetProperty();
        }
    
        private void OnSetProperty(string text)
        {
            _setter.Invoke(text);
            OnChanged?.Invoke();
        }

        private string OnGetProperty()
        {
            return _getter.Invoke();
        }
    }
}