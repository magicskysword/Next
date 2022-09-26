/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore;

public partial class UI_LabGroupTitle : GLabel
{
    public GGraph m_left;
    public GGraph m_right;
    public const string URL = "ui://028qk31hro2x4d";

    public static UI_LabGroupTitle CreateInstance()
    {
        return (UI_LabGroupTitle)UIPackage.CreateObject("NextCore", "LabGroupTitle");
    }

    public override void ConstructFromXML(XML xml)
    {
        base.ConstructFromXML(xml);

        m_left = (GGraph)GetChild("left");
        m_right = (GGraph)GetChild("right");
    }
}