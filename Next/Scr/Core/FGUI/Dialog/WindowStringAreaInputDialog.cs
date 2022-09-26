using System;
using SkySwordKill.Next.FGUI.Component;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.Next.FGUI.Dialog;

public class WindowStringAreaInputDialog : WindowDialogBase
{
    private string _title;
    private string _defaultText;
    private Action<string> _onConfirm;
    private Action<string> _onCancel;

    public static void CreateDialog(string title,string text, Action<string> onConfirm = null,Action<string> onCancel = null)
    {
        var window = new WindowStringAreaInputDialog();
        window._defaultText = text;
        window._onConfirm = onConfirm;
        window._onCancel = onCancel;
        window._title = title;
        window.modal = true;
        
        window.Show();
    }
    
    public WindowStringAreaInputDialog() : base("NextCore", "WinStringInputDialog")
    {
    }

    public UI_WinStringInputDialog MainView => contentPane as UI_WinStringInputDialog;
    public CtlTextInputArea InputArea { get; set; }
    
    protected override void OnInit()
    {
        base.OnInit();
        InputArea = new CtlTextInputArea(MainView.m_inContent);
            
        MainView.m_frame.title = _title;
        InputArea.Text = _defaultText;
        closeButton.onClick.Set(() => Cancel(InputArea.Text));
        MainView.m_closeButton.onClick.Set(() => Cancel(InputArea.Text));
        MainView.m_btnOk.onClick.Set(() => Confirm(InputArea.Text));
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