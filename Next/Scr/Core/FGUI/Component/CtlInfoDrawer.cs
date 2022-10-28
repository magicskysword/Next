using System;
using FairyGUI;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.Next.FGUI.Component;

public class CtlInfoDrawer : CtlPropertyDrawerBase
{
    public CtlInfoDrawer(string title, string content)
    {
        _title = title;
        _content = content;
    }
    
    public CtlInfoDrawer(string title, string content, int fontSize) : this(title, content)
    {
        _fontSize = fontSize;
    }

    protected UI_ComInfoDrawer Drawer { get; set; }
    protected string _title;
    protected string _content;
    protected int _fontSize = Int32.MinValue;

    protected override GComponent OnCreateCom()
    {
        Drawer = UIPackage.CreateObject(FGUIManager.PKG_NEXT_CORE, "ComInfoDrawer").As<UI_ComInfoDrawer>();
        Drawer.title = _title;
        Drawer.m_content.text = _content;
        if(_fontSize > 0)
        {
            Drawer.titleFontSize = _fontSize;
            Drawer.m_content.textFormat.size = _fontSize;
        }
        return Drawer;
    }

    protected override void SetDrawerEditable(bool value)
    {
        
    }

    public void SetTitle(string title)
    {
        Drawer.title = title;
    }

    public void SetContent(string content)
    {
        Drawer.m_content.text = content;
    }
}