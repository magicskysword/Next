using FairyGUI;
using SkySwordKill.Next.XiaoYeGUI;
using UnityEngine;

namespace SkySwordKill.Next.FGUI;

public class FGUIWindowBase : Window
{
    public string comName;
    public string pkgName;
    public RayBlocker RayBlocker;

    public FGUIWindowBase(string pkgName,string comName)
    {
        this.pkgName = pkgName;
        this.comName = comName;
        contentPane = Main.FGUI.CreateUIObject(pkgName, comName).asCom;
        contentPane.onKeyDown.Add(OnKeyDown);
        
        Main.FGUI.RegisterWindow(this);
    }

    protected override void OnInit()
    {
        RayBlocker = RayBlocker.CreateRayBlock(comName);
        onSizeChanged.Add(ResetRayBlocker);
        onPositionChanged.Add(ResetRayBlocker);
    }

    public virtual void ResetRayBlocker()
    {
        if (modal)
        {
            RayBlocker.SetSize(new Rect(0, 0, Screen.width, Screen.height));
        }
        else
        {
            RayBlocker.SetSize(new Rect(x, y, width, height));
        }
    }

    protected override void OnShown()
    {
        base.OnShown();
        RayBlocker.OpenBlocker();
        contentPane.RequestFocus();
    }

    protected override void OnHide()
    {
        RayBlocker.CloseBlocker();
        RayBlocker.DestroySelf();
        OnDispose();
        Dispose();
    }
    
    public virtual void HideForce()
    {
        RayBlocker.CloseBlocker();
        RayBlocker.DestroySelf();
        OnDispose();
        Dispose();
    }

    protected virtual void OnDispose()
    {
        Main.FGUI.UnRegisterWindow(this);
    }

    protected virtual void OnKeyDown(EventContext context)
    {
        
    }

    protected override void closeEventHandler(EventContext context)
    {
            
    }

    /// <summary>
    /// 按缩放因子设置缩放大小并居中
    /// </summary>
    public void MakeFullScreenAndCenter(float factor = 1f)
    {
        this.SetSize(GRoot.inst.width * factor, GRoot.inst.height * factor);
        Center();
        ResetRayBlocker();
    }
}