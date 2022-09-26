/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore;

public partial class UI_ComCmdTextWithPort : GComponent
{
    public GGraph m_bg;
    public GTextField m_title;
    public const string URL = "ui://028qk31hasvv3a";

    public static UI_ComCmdTextWithPort CreateInstance()
    {
        return (UI_ComCmdTextWithPort)UIPackage.CreateObject("NextCore", "ComCmdTextWithPort");
    }

    public override void ConstructFromXML(XML xml)
    {
        base.ConstructFromXML(xml);

        m_bg = (GGraph)GetChild("bg");
        m_title = (GTextField)GetChild("title");
    }
}