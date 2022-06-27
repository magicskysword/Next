using System;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.Next.FGUI
{
    public class WindowStringInputDialog : WindowDialogBase
    {
        private string _title;
        private string _defaultText;
        private Action<string> _onConfirm;
        private Action<string> _onCancel;

        public static void CreateDialog(string title,string text, Action<string> onConfirm = null,Action<string> onCancel = null)
        {
            var window = new WindowStringInputDialog();
            window._defaultText = text;
            window._onConfirm = onConfirm;
            window._onCancel = onCancel;
            window._title = title;
            window.modal = true;
        
            window.Show();
        }
    
        public WindowStringInputDialog() : base("NextCore", "WinStringInputDialog")
        {
        }

        public UI_WinStringInputDialog MainView => contentPane as UI_WinStringInputDialog;
    
        protected override void OnInit()
        {
            base.OnInit();
            MainView.m_frame.title = _title;
            MainView.m_inContent.text = _defaultText;
            closeButton.onClick.Set(() => Cancel(MainView.m_inContent.text));
            MainView.m_closeButton.onClick.Set(() => Cancel(MainView.m_inContent.text));
            MainView.m_btnOk.onClick.Set(() => Confirm(MainView.m_inContent.text));
        }

        private void Confirm(string getStr)
        {
            _onConfirm?.Invoke(getStr);
            Hide();
        }

        private void Cancel(string getStr)
        {
            _onCancel?.Invoke(getStr);
            Hide();
        }
    }
}