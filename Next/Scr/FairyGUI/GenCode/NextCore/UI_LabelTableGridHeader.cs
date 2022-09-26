/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore;

public partial class UI_LabelTableGridHeader : GLabel
{
    public GGraph m_dragable;
    public const string URL = "ui://028qk31hnkvz2c";

    public static UI_LabelTableGridHeader CreateInstance()
    {
        return (UI_LabelTableGridHeader)UIPackage.CreateObject("NextCore", "LabelTableGridHeader");
    }

    public override void ConstructFromXML(XML xml)
    {
        base.ConstructFromXML(xml);

        m_dragable = (GGraph)GetChild("dragable");
    }
}