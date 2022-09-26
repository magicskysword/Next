/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore;

public partial class UI_ComIconPreviewDrawer : GLabel
{
    public GGraph m_bg;
    public const string URL = "ui://028qk31hr3ga4n";

    public static UI_ComIconPreviewDrawer CreateInstance()
    {
        return (UI_ComIconPreviewDrawer)UIPackage.CreateObject("NextCore", "ComIconPreviewDrawer");
    }

    public override void ConstructFromXML(XML xml)
    {
        base.ConstructFromXML(xml);

        m_bg = (GGraph)GetChild("bg");
    }
}