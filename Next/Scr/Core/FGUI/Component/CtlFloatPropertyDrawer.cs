using System;
using System.Globalization;
using FairyGUI;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.Next.FGUI.Component
{
    public class CtlFloatPropertyDrawer : CtlPropertyDrawerBase
    {
        private string _drawerName;
        private UI_ComNumberDrawer Drawer => (UI_ComNumberDrawer)Component;
        private Action<float> _setter;
        private Func<float> _getter;

        public CtlFloatPropertyDrawer(string drawerName,Action<float> setter,Func<float> getter)
        {
            _drawerName = drawerName;
            _setter = setter;
            _getter = getter;
        }
    
        protected override GComponent OnCreateCom()
        {
            var drawer = UI_ComNumberDrawer.CreateInstance();
            drawer.BindFloatEndEdit(OnSetProperty);
            drawer.title = _drawerName;
            return drawer;
        }

        protected override void OnRefresh()
        {
            Drawer.m_inContent.text = OnGetProperty().ToString(CultureInfo.InvariantCulture);
        }

        protected override void SetDrawerEditable(bool value)
        {
            Drawer.SetEditable(value);
        }

        private void OnSetProperty(float value)
        {
            this.Record(new ValueChangedCommand<float>(OnGetProperty(),value, _setter));
            OnChanged?.Invoke();
        }

        private float OnGetProperty()
        {
            return _getter.Invoke();
        }
    }
}