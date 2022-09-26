/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore;

public partial class UI_ComboBox_item : GButton
{
    public GGraph m_selected;
    public GGraph m_hover;
    public const string URL = "ui://028qk31hnkvz1t";

    public static UI_ComboBox_item CreateInstance()
    {
        return (UI_ComboBox_item)UIPackage.CreateObject("NextCore", "ComboBox_item");
    }

    public override void ConstructFromXML(XML xml)
    {
        base.ConstructFromXML(xml);

        m_selected = (GGraph)GetChild("selected");
        m_hover = (GGraph)GetChild("hover");
    }
}