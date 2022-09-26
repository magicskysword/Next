/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore;

public partial class UI_ComNodeBlock : GLabel
{
    public GGraph m_bgbHeader;
    public GGraph m_bgBody;
    public GGraph m_dragArea;
    public GButton m_btnEdit;
    public GList m_content;
    public const string URL = "ui://028qk31hf7x836";

    public static UI_ComNodeBlock CreateInstance()
    {
        return (UI_ComNodeBlock)UIPackage.CreateObject("NextCore", "ComNodeBlock");
    }

    public override void ConstructFromXML(XML xml)
    {
        base.ConstructFromXML(xml);

        m_bgbHeader = (GGraph)GetChild("bgbHeader");
        m_bgBody = (GGraph)GetChild("bgBody");
        m_dragArea = (GGraph)GetChild("dragArea");
        m_btnEdit = (GButton)GetChild("btnEdit");
        m_content = (GList)GetChild("content");
    }
}