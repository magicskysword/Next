/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore;

public partial class UI_ComNumberDrawer : GLabel
{
    public Controller m_warning;
    public Controller m_grayed;
    public GTextInput m_inContent;
    public const string URL = "ui://028qk31hnkvz1y";

    public static UI_ComNumberDrawer CreateInstance()
    {
        return (UI_ComNumberDrawer)UIPackage.CreateObject("NextCore", "ComNumberDrawer");
    }

    public override void ConstructFromXML(XML xml)
    {
        base.ConstructFromXML(xml);

        m_warning = GetController("warning");
        m_grayed = GetController("grayed");
        m_inContent = (GTextInput)GetChild("inContent");
    }
}