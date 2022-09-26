/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore;

public partial class UI_ComInfoDrawer : GLabel
{
    public GTextField m_content;
    public const string URL = "ui://028qk31hspup3q";

    public static UI_ComInfoDrawer CreateInstance()
    {
        return (UI_ComInfoDrawer)UIPackage.CreateObject("NextCore", "ComInfoDrawer");
    }

    public override void ConstructFromXML(XML xml)
    {
        base.ConstructFromXML(xml);

        m_content = (GTextField)GetChild("content");
    }
}