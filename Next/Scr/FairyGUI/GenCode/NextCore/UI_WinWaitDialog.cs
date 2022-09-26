/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore;

public partial class UI_WinWaitDialog : GComponent
{
    public UI_WindowFrameDialog m_frame;
    public GTextField m_text;
    public const string URL = "ui://028qk31hhc7r4f";

    public static UI_WinWaitDialog CreateInstance()
    {
        return (UI_WinWaitDialog)UIPackage.CreateObject("NextCore", "WinWaitDialog");
    }

    public override void ConstructFromXML(XML xml)
    {
        base.ConstructFromXML(xml);

        m_frame = (UI_WindowFrameDialog)GetChild("frame");
        m_text = (GTextField)GetChild("text");
    }
}