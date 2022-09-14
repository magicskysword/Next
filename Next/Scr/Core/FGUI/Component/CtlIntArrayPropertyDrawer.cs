using System;
using System.Collections.Generic;
using FairyGUI;
using SkySwordKill.NextEditor.Mod;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.Next.FGUI.Component
{
    public class CtlIntArrayPropertyDrawer : CtlPropertyDrawerBase
    {
        private string _drawerName;
        private UI_ComNumberDrawer Drawer => (UI_ComNumberDrawer)Component;
        private Action<List<int>> _setter;
        private Func<List<int>> _getter;

        public CtlIntArrayPropertyDrawer(string drawerName,Action<List<int>> setter,Func<List<int>> getter)
        {
            _drawerName = drawerName;
            _setter = setter;
            _getter = getter;
        }
    
        protected override GComponent OnCreateCom()
        {
            var drawer = UI_ComNumberDrawer.CreateInstance();
            drawer.BindArrayEndEdit(OnEndEdit);
            drawer.title = _drawerName;
            return drawer;
        }
        
        private void OnEndEdit(List<int> values)
        {
            OnSetProperty(values);
        }

        protected override void OnRefresh()
        {
            Drawer.m_inContent.text = OnGetProperty().ToFormatString();
        }

        protected override void SetDrawerEditable(bool value)
        {
            Drawer.SetEditable(value);
        }

        private void OnSetProperty(List<int> value)
        {
            this.Record(new ValueChangedCommand<List<int>>(OnGetProperty(), value, _setter));
            OnChanged?.Invoke();
        }

        private List<int> OnGetProperty()
        {
            return _getter.Invoke();
        }
    }
}