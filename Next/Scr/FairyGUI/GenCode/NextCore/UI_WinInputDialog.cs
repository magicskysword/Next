/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore;

public partial class UI_WinInputDialog : GComponent
{
    public Controller m_type;
    public UI_WindowFrameDialog m_frame;
    public GTextInput m_inContent;
    public GButton m_btnOk;
    public GButton m_closeButton;
    public const string URL = "ui://028qk31hd4rw3y";

    public static UI_WinInputDialog CreateInstance()
    {
        return (UI_WinInputDialog)UIPackage.CreateObject("NextCore", "WinInputDialog");
    }

    public override void ConstructFromXML(XML xml)
    {
        base.ConstructFromXML(xml);

        m_type = GetController("type");
        m_frame = (UI_WindowFrameDialog)GetChild("frame");
        m_inContent = (GTextInput)GetChild("inContent");
        m_btnOk = (GButton)GetChild("btnOk");
        m_closeButton = (GButton)GetChild("closeButton");
    }
}