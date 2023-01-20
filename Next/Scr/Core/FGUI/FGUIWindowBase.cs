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
    }

    protected override void OnHide()
    {
        RayBlocker.CloseBlocker();
        RayBlocker.DestroySelf();
        Dispose();
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