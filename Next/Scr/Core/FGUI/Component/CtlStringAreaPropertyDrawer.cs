using System;
using FairyGUI;
using SkySwordKill.Next.FGUI.Dialog;
using SkySwordKill.Next.Extension;
using SkySwordKill.Next.FGUI;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.Next.FGUI.Component
{
    public class CtlStringAreaPropertyDrawer : CtlPropertyDrawerBase
    {
        private string _drawerName;
        private UI_ComStringAreaDrawer Drawer => (UI_ComStringAreaDrawer)Component;
        private Action<string> _setter;
        private Func<string> _getter;
    
        public CtlStringAreaPropertyDrawer(string drawerName,Action<string> setter,Func<string> getter)
        {
            _drawerName = drawerName;
            _setter = setter;
            _getter = getter;
        }
        
        protected override GComponent OnCreateCom()
        {
            var drawer = UI_ComStringAreaDrawer.CreateInstance();
            drawer.m_inContent.onFocusOut.Add(() => OnSetProperty(drawer.m_inContent.text));
            drawer.title = _drawerName;
            drawer.m_btnEdit.onClick.Set(()=>
            {
                WindowStringAreaInputDialog.CreateDialog(
                    "ModEditor.Main.dialog.textEdit".I18N(),
                    OnGetProperty(), 
                    OnConfirmEdit);
            });
            return drawer;
        }
    
        private void OnConfirmEdit(string str)
        {
            OnSetProperty(str);
            Refresh();
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