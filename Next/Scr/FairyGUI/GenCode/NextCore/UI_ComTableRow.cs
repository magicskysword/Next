/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore;

public partial class UI_ComTableRow : GButton
{
    public GGraph m_selected;
    public GGraph m_hover;
    public GList m_list;
    public const string URL = "ui://028qk31hnkvz2d";

    public static UI_ComTableRow CreateInstance()
    {
        return (UI_ComTableRow)UIPackage.CreateObject("NextCore", "ComTableRow");
    }

    public override void ConstructFromXML(XML xml)
    {
        base.ConstructFromXML(xml);

        m_selected = (GGraph)GetChild("selected");
        m_hover = (GGraph)GetChild("hover");
        m_list = (GList)GetChild("list");
    }
}