using FairyGUI;
using UnityEngine;

namespace SkySwordKill.Next.FGUI.Component;

public class CtlTextLinkDrawer : CtlTextDrawer
{
    public CtlTextLinkDrawer(string text, string link) : base(text)
    {
        _link = link;
    }

    public CtlTextLinkDrawer(string text ,int fontSize, string link) : base(text, fontSize)
    {
        _link = link;
    }

    protected string _link;

    protected override GComponent OnCreateCom()
    {
        var com = base.OnCreateCom();
        com.cursor = FGUIManager.MOUSE_HAND;
        com.onClick.Add(() =>
        {
            Application.OpenURL(_link);
        });
        return com;
    }
    
    public virtual void SetLink(string link)
    {
        _link = link;
    }
}