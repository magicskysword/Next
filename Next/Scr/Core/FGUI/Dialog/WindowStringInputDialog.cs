using System;
using FairyGUI;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.Next.FGUI.Dialog
{
    public class WindowStringInputDialog : WindowDialogBase
    {
        private WindowStringInputDialog() : base("NextCore", "WinInputDialog")
        {
            
        }

        public static void CreateDialog(string title,string defaultText, bool canCancel,Action<string> onConfirm, Action onCancel = null)
        {
            var window = new WindowStringInputDialog();
            window.DefaultText = defaultText;
            window.Title = title;
            window.CanCancel = canCancel;
            window.OnConfirm = onConfirm;
            window.OnCancel = onCancel;

            window.modal = true;
            window.Show();
        }

        

        private string Title { get; set; }
        private string DefaultText { get; set; }
        private bool CanCancel { get; set; }
        private Action<string> OnConfirm { get; set; }
        private Action OnCancel { get; set; }

        public UI_WinInputDialog InputDialog => (UI_WinInputDialog)contentPane;

        protected override void OnInit()
        {
            base.OnInit();

            InputDialog.GetController("type").selectedIndex = CanCancel ? 1 : 0;
            InputDialog.m_frame.title = Title;
            InputDialog.m_inContent.text = DefaultText;
            InputDialog.m_inContent.cursor = "text";
            InputDialog.m_btnOk.onClick.Add(OnClickConfirm);
            InputDialog.m_frame.m_closeButton.onClick.Add(OnClickCancel);
            InputDialog.m_closeButton.onClick.Add(OnClickCancel);
        }

        private void OnClickConfirm(EventContext context)
        {
            Hide();
            OnConfirm?.Invoke(InputDialog.m_inContent.text);
        }
        
        private void OnClickCancel(EventContext context)
        {
            Hide();
            OnCancel?.Invoke();
        }
    }
}