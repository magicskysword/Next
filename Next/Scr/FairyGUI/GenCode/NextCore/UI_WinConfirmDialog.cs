/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore;

public partial class UI_WinConfirmDialog : GComponent
{
    public Controller m_type;
    public UI_WindowFrameDialog m_frame;
    public GTextField m_text;
    public GButton m_btnOk;
    public GButton m_closeButton;
    public const string URL = "ui://028qk31hnkvz32";

    public static UI_WinConfirmDialog CreateInstance()
    {
        return (UI_WinConfirmDialog)UIPackage.CreateObject("NextCore", "WinConfirmDialog");
    }

    public override void ConstructFromXML(XML xml)
    {
        base.ConstructFromXML(xml);

        m_type = GetController("type");
        m_frame = (UI_WindowFrameDialog)GetChild("frame");
        m_text = (GTextField)GetChild("text");
        m_btnOk = (GButton)GetChild("btnOk");
        m_closeButton = (GButton)GetChild("closeButton");
    }
}