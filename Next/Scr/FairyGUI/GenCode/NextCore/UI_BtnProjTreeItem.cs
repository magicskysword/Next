/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore;

public partial class UI_BtnProjTreeItem : GButton
{
    public Controller m_expanded;
    public Controller m_leaf;
    public GGraph m_selected;
    public GGraph m_hover;
    public GGraph m_indent;
    public GButton m_expandButton;
    public const string URL = "ui://028qk31hnkvz1j";

    public static UI_BtnProjTreeItem CreateInstance()
    {
        return (UI_BtnProjTreeItem)UIPackage.CreateObject("NextCore", "BtnProjTreeItem");
    }

    public override void ConstructFromXML(XML xml)
    {
        base.ConstructFromXML(xml);

        m_expanded = GetController("expanded");
        m_leaf = GetController("leaf");
        m_selected = (GGraph)GetChild("selected");
        m_hover = (GGraph)GetChild("hover");
        m_indent = (GGraph)GetChild("indent");
        m_expandButton = (GButton)GetChild("expandButton");
    }
}