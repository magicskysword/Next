using System;
using FairyGUI;
using SkySwordKill.NextFGUI.NextCore;
using UnityEngine;

namespace SkySwordKill.Next.FGUI.Dialog;

public class WindowConfirmDialog : WindowDialogBase
{
    public string _title;
    public string _showText;
    public bool _canCancel;
    public Action _onConfirm;
    public Action _onCancel;
    private bool _result;

    public WindowConfirmDialog() : base("NextCore", "WinConfirmDialog")
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
        window.modal = true;

        window.Show();
    }

    protected override void OnKeyDown(EventContext context)
    {
        base.OnKeyDown(context);
        
        if (context.inputEvent.keyCode == KeyCode.Escape && _canCancel)
        {
            Cancel();
        }
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
        _result = true;
        Hide();
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
            _onConfirm?.Invoke();
        else
            _onCancel?.Invoke();
    }
}