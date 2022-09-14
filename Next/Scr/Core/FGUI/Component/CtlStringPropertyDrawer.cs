using System;
using FairyGUI;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.Next.FGUI.Component
{
    public class StringPropertyDrawerUndoCommand : IUndoCommand
    {
        private string _oldValue;
        private string _newValue;
        private Action<string> _onValueChanged;

        public StringPropertyDrawerUndoCommand(string oldValue, string newValue, Action<string> onValueChanged)
        {
            _oldValue = oldValue;
            _newValue = newValue;
            _onValueChanged = onValueChanged;
        }

        public void Execute()
        {
            _onValueChanged?.Invoke(_newValue);
        }

        public void Undo()
        {
            _onValueChanged?.Invoke(_oldValue);
        }
    }

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

        protected override void SetDrawerEditable(bool value)
        {
            Drawer.SetEditable(value);
        }

        private void OnSetProperty(string text)
        {
            this.Record(new ValueChangedCommand<string>(OnGetProperty(),text, _setter));
            OnChanged?.Invoke();
        }

        private string OnGetProperty()
        {
            return _getter.Invoke();
        }
    }
}