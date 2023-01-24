using System;
using FairyGUI;
using SkySwordKill.NextFGUI.NextCore;
using UnityEngine;

namespace SkySwordKill.Next.FGUI.Dialog;

public class WindowConfirmDialogExtra : WindowDialogBase
{
    private string _title;
    private string _showText;
    private string _extraText;
    private bool _canCancel;
    private bool _defaultExtra;
    private Action<bool> _onConfirm;
    private Action _onCancel;
    private bool _result;

    private WindowConfirmDialogExtra() : base("NextCore", "WinConfirmDialogExtra")
    {
    }

    public UI_WinConfirmDialogExtra MainView => contentPane as UI_WinConfirmDialogExtra;

    public static void CreateDialog(string title,string text,string extraText,bool canCancel,bool defaultExtra = false,
        Action<bool> onConfirm = null, Action onCancel = null)
    {
        var window = new WindowConfirmDialogExtra();
        window._showText = text;
        window._extraText = extraText;
        window._defaultExtra = defaultExtra;
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
        MainView.m_txtExtra.text = _extraText;
        MainView.m_tglExtra.selected = _defaultExtra;
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
        if (_result)
            _onConfirm?.Invoke(MainView.m_tglExtra.selected);
        else
            _onCancel?.Invoke();
    }
}