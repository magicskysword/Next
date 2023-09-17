using FairyGUI;
using SkySwordKill.Next.XiaoYeGUI;
using UnityEngine;

namespace SkySwordKill.Next.FGUI;

public class FGUIPanelBase
{
    public string comName;
    public string pkgName;
    public RayBlocker RayBlocker;
        
    public GComponent contentPane { get; set; }
    public bool IsShowing { get; set; }
    private bool IsInit { get; set; } = false;
    public bool IsModal { get; set; }

    public FGUIPanelBase(string pkgName,string comName)
    {
        this.pkgName = pkgName;
        this.comName = comName;
        contentPane = Main.FGUI.CreateUIObject(pkgName, comName).asCom;
    }
        
    protected virtual void OnInit()
    {
        RayBlocker = RayBlocker.CreateRayBlock(comName);
        if (!IsModal)
        {
            contentPane.onSizeChanged.Add(ResetRayBlocker);
            contentPane.onPositionChanged.Add(ResetRayBlocker);
        }
        else
        {
            RayBlocker.SetSize(new Rect(0, 0, GRoot.inst.width, GRoot.inst.height));
        }
    }

    public virtual void ResetRayBlocker()
    {
        RayBlocker.SetSize(new Rect(contentPane.x, contentPane.y, contentPane.width, contentPane.height));
    }
        
    public void Show()
    {
        if(!IsInit)
        {
            OnInit();
            IsInit = true;
        }
        GRoot.inst.AddChild(contentPane);
        DoShowAnimation();
    }
        
    public void Hide()
    {
        if (IsShowing)
            DoHideAnimation();
    }

    protected virtual void DoShowAnimation()
    {
        OnShown();
    }
        
    protected virtual void DoHideAnimation()
    {
        HideImmediately();
    }

    protected void HideImmediately()
    {
        GRoot.inst.RemoveChild(contentPane);
        IsShowing = false;
        OnHide();
    }

    protected virtual void OnShown()
    {
        IsShowing = true;
        RayBlocker.OpenBlocker();
        contentPane.RequestFocus();
    }

    protected virtual void OnHide()
    {
        RayBlocker.CloseBlocker();
        RayBlocker.DestroySelf();
        OnDispose();
        contentPane.Dispose();
    }
    
    protected virtual void OnDispose()
    {
        
    }
        
    /// <summary>
    /// 按缩放因子设置缩放大小并居中
    /// </summary>
    public void MakeFullScreenAndCenter()
    {
        contentPane.SetSize(GRoot.inst.width, GRoot.inst.height);
        contentPane.Center();
        ResetRayBlocker();
    }
}