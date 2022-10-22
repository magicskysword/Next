/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore
{
    public partial class UI_ComListProject : GComponent
    {
        public Controller m_showToolbar;
        public UI_ComToolsBar m_toolsBar;
        public GList m_list;
        public const string URL = "ui://028qk31hro2x4e";

        public static UI_ComListProject CreateInstance()
        {
            return (UI_ComListProject)UIPackage.CreateObject("NextCore", "ComListProject");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_showToolbar = GetController("showToolbar");
            m_toolsBar = (UI_ComToolsBar)GetChild("toolsBar");
            m_list = (GList)GetChild("list");
        }
    }
}