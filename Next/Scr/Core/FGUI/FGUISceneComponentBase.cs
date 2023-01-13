using SkySwordKill.Next;
using SkySwordKill.Next.FGUI;
using SkySwordKill.Next.XiaoYeGUI;

namespace Next.Scr.Core.FGUI;

public abstract class FGUIScenePanelBase : FGUIPanelBase
{
    public FGUIScenePanelBase(string pkgName, string comName) : base(pkgName, comName)
    {
        Main.FGUI.RegisterScenePanel(this);
    }

    protected override void OnHide()
    {
        base.OnHide();
        Main.FGUI.RemoveScenePanel(this);
    }
}