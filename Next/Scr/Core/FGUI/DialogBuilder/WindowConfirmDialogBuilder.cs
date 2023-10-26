using System;
using SkySwordKill.Next.FGUI.Dialog;

namespace SkySwordKill.Next.FGUI.DialogBuilder;

public class WindowConfirmDialogBuilder
{
    public string Content { get; set; }
    public bool CanCancel { get; set; }
    public Action OnConfirm { get; set; }
    public Action OnCancel { get; set; }
    public string Title { get; set; }
    
    
    public WindowConfirmDialog Build()
    {
        var window = new WindowConfirmDialog();
        window._showText = Content;
        window._canCancel = CanCancel;
        window._onConfirm = OnConfirm;
        window._onCancel = OnCancel;
        window._title = Title;
        window.modal = true;
        
        return window;
    }
    
    public WindowConfirmDialogBuilder SetContent(string text)
    {
        Content = text;
        return this;
    }
    
    public WindowConfirmDialogBuilder SetCanCancel(bool canCancel)
    {
        CanCancel = canCancel;
        return this;
    }
    
    public WindowConfirmDialogBuilder SetOnConfirm(Action onConfirm)
    {
        OnConfirm = onConfirm;
        return this;
    }
    
    public WindowConfirmDialogBuilder SetOnCancel(Action onCancel)
    {
        OnCancel = onCancel;
        return this;
    }
    
    public WindowConfirmDialogBuilder SetTitle(string title)
    {
        Title = title;
        return this;
    }
}