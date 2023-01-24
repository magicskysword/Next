using System;
using FairyGUI;
using SkySwordKill.NextFGUI.NextCore;
using UnityEngine;

namespace SkySwordKill.Next.FGUI.Dialog;

public class WindowStringInputDialog : WindowDialogBase
{
    private bool _result;

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
        InputDialog.m_inContent.cursor = FGUIManager.MOUSE_TEXT;
        InputDialog.m_btnOk.onClick.Add(OnClickConfirm);
        InputDialog.m_frame.m_closeButton.onClick.Add(OnClickCancel);
        InputDialog.m_closeButton.onClick.Add(OnClickCancel);
    }

    private void OnClickConfirm(EventContext context)
    {
        _result = true;
        Hide();
    }
        
    private void OnClickCancel(EventContext context)
    {
       Cancel();
    }
    
    protected override void OnKeyDown(EventContext context)
    {
        base.OnKeyDown(context);
        
        if (context.inputEvent.keyCode == KeyCode.Escape)
        {
            Cancel();
        }
    }

    private void Cancel()
    {
        _result = false;
        Hide();
    }

    protected override void OnHide()
    {
        base.OnHide();
        if(_result)
            OnConfirm?.Invoke(InputDialog.m_inContent.text);
        else
            OnCancel?.Invoke();
    }
}