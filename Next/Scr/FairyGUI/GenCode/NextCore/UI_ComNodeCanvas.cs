/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore;

public partial class UI_ComNodeCanvas : GComponent
{
    public UI_ComNodeZoomArea m_zoom;
    public const string URL = "ui://028qk31hf7x834";

    public static UI_ComNodeCanvas CreateInstance()
    {
        return (UI_ComNodeCanvas)UIPackage.CreateObject("NextCore", "ComNodeCanvas");
    }

    public override void ConstructFromXML(XML xml)
    {
        base.ConstructFromXML(xml);

        m_zoom = (UI_ComNodeZoomArea)GetChild("zoom");
    }
}