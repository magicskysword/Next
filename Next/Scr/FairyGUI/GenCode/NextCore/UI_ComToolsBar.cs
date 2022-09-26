/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore;

public partial class UI_ComToolsBar : GComponent
{
    public GList m_tools;
    public const string URL = "ui://028qk31hd4rw3w";

    public static UI_ComToolsBar CreateInstance()
    {
        return (UI_ComToolsBar)UIPackage.CreateObject("NextCore", "ComToolsBar");
    }

    public override void ConstructFromXML(XML xml)
    {
        base.ConstructFromXML(xml);

        m_tools = (GList)GetChild("tools");
    }
}