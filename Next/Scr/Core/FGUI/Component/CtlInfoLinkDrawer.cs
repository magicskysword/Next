using FairyGUI;
using SkySwordKill.NextFGUI.NextCore;
using UnityEngine;

namespace SkySwordKill.Next.FGUI.Component;

public class CtlInfoLinkDrawer : CtlInfoDrawer
{
    public CtlInfoLinkDrawer(string title, string content, string link) : base(title, content)
    {
        _link = link;
    }

    public CtlInfoLinkDrawer(string title, string content, int fontSize, string link) : base(title, content, fontSize)
    {
        _link = link;
    }

    protected string _link;

    protected override GComponent OnCreateCom()
    {
        var com = base.OnCreateCom().As<UI_ComInfoDrawer>();
        com.m_mask.cursor = FGUIManager.MOUSE_HAND;
        com.m_mask.onClick.Add(() => { Application.OpenURL(_link); });
        com.m_content.textFormat.underline = true;
        return com;
    }

    public virtual void SetLink(string link)
    {
        _link = link;
    }
}