using System;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.Next.FGUI.Dialog
{
    public class WindowConfirmDialog : WindowDialogBase
    {
        private string _title;
        private string _showText;
        private bool _canCancel;
        private Action _onConfirm;
        private Action _onCancel;

        private WindowConfirmDialog() : base("NextCore", "WinConfirmDialog")
        {
        }

        public UI_WinConfirmDialog MainView => contentPane as UI_WinConfirmDialog;

        public static void CreateDialog(string title,string text,bool canCancel,Action onConfirm = null,Action onCancel = null)
        {
            var window = new WindowConfirmDialog();
            window._showText = text;
            window._canCancel = canCancel;
            window._onConfirm = onConfirm;
            window._onCancel = onCancel;
            window._title = title;

            window.Show();
        }
    
        protected override void OnInit()
        {
            base.OnInit();
            MainView.m_frame.title = _title;
            MainView.m_text.text = _showText;
            MainView.m_closeButton.onClick.Set(Cancel);
            MainView.m_btnOk.onClick.Set(Confirm);
            var typeCtl = MainView.GetController("type");
            typeCtl.selectedIndex = _canCancel ? 1 : 0;
            MainView.Center();
        }

        private void Confirm()
        {
            _onConfirm?.Invoke();
            Hide();
        }

        private void Cancel()
        {
            _onCancel?.Invoke();
            Hide();
        }
    }
}