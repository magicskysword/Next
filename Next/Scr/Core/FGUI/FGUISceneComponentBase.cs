using SkySwordKill.Next;
using SkySwordKill.Next.FGUI;

namespace Next.Scr.Core.FGUI;

public abstract class FGUIScenePanelBase : FGUIPanelBase
{
    public bool DontDestroyOnLoad { get; set; }

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