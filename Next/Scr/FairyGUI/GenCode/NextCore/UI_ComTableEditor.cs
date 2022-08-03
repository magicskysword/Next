/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore
{
    public partial class UI_ComTableEditor : GComponent
    {
        public GGraph m_bg;
        public GGraph m_bgTable;
        public UI_ComTableList m_table;
        public UI_ComMainInspector m_inspector;
        public GGraph m_seg;
        public UI_ComToolsBar m_toolsBar;
        public const string URL = "ui://028qk31heg8y3f";

        public static UI_ComTableEditor CreateInstance()
        {
            return (UI_ComTableEditor)UIPackage.CreateObject("NextCore", "ComTableEditor");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_bg = (GGraph)GetChild("bg");
            m_bgTable = (GGraph)GetChild("bgTable");
            m_table = (UI_ComTableList)GetChild("table");
            m_inspector = (UI_ComMainInspector)GetChild("inspector");
            m_seg = (GGraph)GetChild("seg");
            m_toolsBar = (UI_ComToolsBar)GetChild("toolsBar");
        }
    }
}