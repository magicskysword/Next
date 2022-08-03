using FairyGUI;
using SkySwordKill.Next.FGUI;
using UnityEngine;

namespace SkySwordKill.Next.FGUI
{
    public abstract class WindowDialogBase : FGUIWindowBase
    {
        protected WindowDialogBase(string pkgName, string comName) : base(pkgName, comName)
        {
            modal = true;
        }
    
        protected override void DoShowAnimation()
        {
            base.DoShowAnimation();
            pivot = new Vector2(0.5f, 0.5f);
            alpha = 0f;
            scale = Vector2.zero;
            Center();
            TweenScale(Vector2.one, 0.3f).SetEase(EaseType.CubicOut);
            TweenFade(1f, 0.3f).OnComplete(OnShown);
        }
    }
}