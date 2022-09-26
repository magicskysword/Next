/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore;

public partial class UI_ComboSearchBox_popup : GComponent
{
    public GList m_list;
    public GGraph m_bg_searchBox;
    public UI_ComToolsSearchBox m_searchBox;
    public const string URL = "ui://028qk31hmdyt4k";

    public static UI_ComboSearchBox_popup CreateInstance()
    {
        return (UI_ComboSearchBox_popup)UIPackage.CreateObject("NextCore", "ComboSearchBox_popup");
    }

    public override void ConstructFromXML(XML xml)
    {
        base.ConstructFromXML(xml);

        m_list = (GList)GetChild("list");
        m_bg_searchBox = (GGraph)GetChild("bg_searchBox");
        m_searchBox = (UI_ComToolsSearchBox)GetChild("searchBox");
    }
}