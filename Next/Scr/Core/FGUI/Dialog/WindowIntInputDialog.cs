using System;
using FairyGUI;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.Next.FGUI.Dialog;

public class WindowIntInputDialog : WindowDialogBase
{
    private WindowIntInputDialog() : base("NextCore", "WinInputDialog")
    {
            
    }

    public static void CreateDialog(string title, bool canCancel,Action<int> onConfirm, Action onCancel = null)
    {
        var window = new WindowIntInputDialog();
        window.Title = title;
        window.CanCancel = canCancel;
        window.OnConfirm = onConfirm;
        window.OnCancel = onCancel;

        window.modal = true;
        window.Show();
    }
        
    private string Title { get; set; }
    private bool CanCancel { get; set; }
    private Action<int> OnConfirm { get; set; }
    private Action OnCancel { get; set; }

    public UI_WinInputDialog InputDialog => (UI_WinInputDialog)contentPane;

    protected override void OnInit()
    {
        base.OnInit();
            
        InputDialog.m_frame.title = Title;
        InputDialog.m_inContent.restrict = "[0-9-]";
        InputDialog.m_inContent.cursor = "text";
        InputDialog.m_btnOk.onClick.Add(OnClickConfirm);
        InputDialog.m_frame.m_closeButton.onClick.Add(OnClickCancel);
        InputDialog.m_closeButton.onClick.Add(OnClickCancel);

        InputDialog.m_type.selectedIndex = CanCancel ? 1 : 0;
    }

    private void OnClickConfirm(EventContext context)
    {
        if (int.TryParse(InputDialog.m_inContent.text, out var value))
        {
            Hide();
            OnConfirm?.Invoke(value);
        }
    }
        
    private void OnClickCancel(EventContext context)
    {
        Hide();
        OnCancel?.Invoke();
    }
}