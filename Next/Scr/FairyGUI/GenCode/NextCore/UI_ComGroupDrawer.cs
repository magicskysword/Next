/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore;

public partial class UI_ComGroupDrawer : GLabel
{
    public GButton m_btnExpand;
    public GGraph m_line;
    public GList m_list;
    public const string URL = "ui://028qk31hv5ah4h";

    public static UI_ComGroupDrawer CreateInstance()
    {
        return (UI_ComGroupDrawer)UIPackage.CreateObject("NextCore", "ComGroupDrawer");
    }

    public override void ConstructFromXML(XML xml)
    {
        base.ConstructFromXML(xml);

        m_btnExpand = (GButton)GetChild("btnExpand");
        m_line = (GGraph)GetChild("line");
        m_list = (GList)GetChild("list");
    }
}