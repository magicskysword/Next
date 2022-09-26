using System;
using SkySwordKill.Next.FGUI.Component;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.Next.FGUI.Dialog;

public class WindowStringAreaInputPreviewDialog : WindowDialogBase
{
    private string _title;
    private string _defaultText;
    private Action<string> _onConfirm;
    private Action<string> _onCancel;
    private Func<string, string> _onAnalysisRef;

    public static void CreateDialog(string title,string text, 
        Action<string> onConfirm = null,
        Action<string> onCancel = null,
        Func<string, string> onAnalysisRef = null)
    {
        var window = new WindowStringAreaInputPreviewDialog();
        window._defaultText = text;
        window._onConfirm = onConfirm;
        window._onCancel = onCancel;
        window._title = title;
        window.modal = true;
        window._onAnalysisRef = onAnalysisRef;
        
        window.Show();
    }
    
    public WindowStringAreaInputPreviewDialog() : base("NextCore", "WinStringInputPreviewDialog")
    {
    }

    public UI_WinStringInputPreviewDialog MainView => contentPane as UI_WinStringInputPreviewDialog;
    public CtlTextInputArea InputArea { get; set; }
    public CtlTextPreviewArea PreviewArea { get; set; }
    
    protected override void OnInit()
    {
        base.OnInit();
        InputArea = new CtlTextInputArea(MainView.m_inContent);
        PreviewArea = new CtlTextPreviewArea(MainView.m_comTextPreview);
            
        MainView.m_frame.title = _title;
        InputArea.Text = _defaultText;
        InputArea.OnChanged += OnInputChanged;
        PreviewArea.OnAnalysisRef += _onAnalysisRef;
        PreviewArea.SetPreviewText(_defaultText);
        closeButton.onClick.Set(() => Cancel(InputArea.Text));
        MainView.m_closeButton.onClick.Set(() => Cancel(InputArea.Text));
        MainView.m_btnOk.onClick.Set(() => Confirm(InputArea.Text));
    }

    public void OnInputChanged(string txt)
    {
        PreviewArea.SetPreviewText(txt);
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