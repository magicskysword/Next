using System;
using FairyGUI;
using SkySwordKill.NextFGUI.NextCore;
using UnityEngine;

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
    
    public static void CreateDialog(string title, bool canCancel, int defaultInt, Action<int> onConfirm, Action onCancel = null)
    {
        var window = new WindowIntInputDialog();
        window.Title = title;
        window.CanCancel = canCancel;
        window.OnConfirm = onConfirm;
        window.OnCancel = onCancel;
        
        window.InputDialog.m_inContent.text = defaultInt.ToString();

        window.modal = true;
        window.Show();
    }
        
    private string Title { get; set; }
    private bool CanCancel { get; set; }
    private Action<int> OnConfirm { get; set; }
    private Action OnCancel { get; set; }
    private bool _result;
    private int _resultValue;

    public UI_WinInputDialog InputDialog => (UI_WinInputDialog)contentPane;

    protected override void OnInit()
    {
        base.OnInit();
            
        InputDialog.m_frame.title = Title;
        InputDialog.m_inContent.restrict = "[0-9-]";
        InputDialog.m_inContent.cursor = FGUIManager.MOUSE_TEXT;
        InputDialog.m_btnOk.onClick.Add(OnClickConfirm);
        InputDialog.m_frame.m_closeButton.onClick.Add(OnClickCancel);
        InputDialog.m_closeButton.onClick.Add(OnClickCancel);

        InputDialog.m_type.selectedIndex = CanCancel ? 1 : 0;
    }
    
    protected override void OnKeyDown(EventContext context)
    {
        base.OnKeyDown(context);
        
        if (context.inputEvent.keyCode == KeyCode.Escape && CanCancel)
        {
            Cancel();
        }
    }

    private void OnClickConfirm(EventContext context)
    {
        if (int.TryParse(InputDialog.m_inContent.text, out var value))
        {
            _result = true;
            _resultValue = value;
            Hide();
        }
    }
        
    private void OnClickCancel(EventContext context)
    {
        Cancel();
    }
    
    private void Cancel()
    {
        _result = false;
        Hide();
    }
    
    protected override void OnHide()
    {
        base.OnHide();
        if (_result)
        {
            OnConfirm?.Invoke(_resultValue);
        }
        else
        {
            OnCancel?.Invoke();
        }
    }
}