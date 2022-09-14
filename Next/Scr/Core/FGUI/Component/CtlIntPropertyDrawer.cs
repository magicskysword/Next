using System;
using FairyGUI;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.Next.FGUI.Component
{
    public class CtlIntPropertyDrawer : CtlPropertyDrawerBase
    {
        private string _drawerName;
        private UI_ComNumberDrawer Drawer => (UI_ComNumberDrawer)Component;
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
            var drawer = UI_ComNumberDrawer.CreateInstance();
            drawer.BindIntEndEdit(OnSetProperty);
            drawer.title = _drawerName;
            return drawer;
        }

        protected override void OnRefresh()
        {
            Drawer.m_inContent.text = OnGetProperty().ToString();
        }
    
        private void OnSetProperty(int value)
        {
            this.Record(new ValueChangedCommand<int>(OnGetProperty(), value, _setter));
            OnChanged?.Invoke();
        }

        private int OnGetProperty()
        {
            return _getter.Invoke();
        }
        protected override void SetDrawerEditable(bool value)
        {
            Drawer.SetEditable(value);
        }
        
    }
}