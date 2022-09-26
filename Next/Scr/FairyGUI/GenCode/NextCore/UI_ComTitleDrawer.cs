/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore;

public partial class UI_ComTitleDrawer : GLabel
{
    public GGraph m_underline;
    public const string URL = "ui://028qk31hfqcb3s";

    public static UI_ComTitleDrawer CreateInstance()
    {
        return (UI_ComTitleDrawer)UIPackage.CreateObject("NextCore", "ComTitleDrawer");
    }

    public override void ConstructFromXML(XML xml)
    {
        base.ConstructFromXML(xml);

        m_underline = (GGraph)GetChild("underline");
    }
}