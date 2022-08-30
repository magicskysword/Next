using System;
using FairyGUI;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.Next.FGUI.Component
{
    public class CtlStringBindDataPropertyDrawer: CtlPropertyDrawerBase
    {
        public CtlStringBindDataPropertyDrawer(string title, Action<string> setter, Func<string> getter,
            Func<string, string> descGetter,Action<string> onClickEdit)
        {
            _drawerName = title;
            _setter = setter;
            _getter = getter;
            _onClickEdit = onClickEdit;
            _descGetter = descGetter;
        }

        private UI_ComStringBindDataDrawer Drawer => (UI_ComStringBindDataDrawer)Component;
        private string _drawerName;
        private readonly Action<string> _setter;
        private readonly Func<string> _getter;
        private readonly Action<string> _onClickEdit;
        private readonly Func<string,string> _descGetter;

        protected override GComponent OnCreateCom()
        {
            var drawer = UI_ComStringBindDataDrawer.CreateInstance();
            drawer.title = _drawerName;
            drawer.m_inContent.onFocusOut.Add(() => OnSetProperty(drawer.m_inContent.text));
            return drawer;
        }

        protected override void OnRefresh()
        {
            Drawer.m_inContent.text = OnGetProperty();
            Drawer.m_txtDesc.text = OnGetDesc();
        }
        
        private void OnSetProperty(string text)
        {
            _setter.Invoke(text);
            Refresh();
            OnChanged?.Invoke();
        }

        private string OnGetProperty()
        {
            return _getter.Invoke();
        }
        
        private string OnGetDesc()
        {
            return _descGetter.Invoke(OnGetProperty());
        }
        
        protected override void SetDrawerEditable(bool value)
        {
            Drawer.SetEditable(value);
        }
    }
}