/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore
{
    public partial class UI_WinModSelectorDialog : GComponent
    {
        public UI_WindowFrameDialog m_frame;
        public GGraph m_bg;
        public UI_ComTableList m_table;
        public UI_ComMainInspector m_inspector;
        public GButton m_btnConfirm;
        public GGraph m_sep;
        public const string URL = "ui://028qk31hnqml3g";

        public static UI_WinModSelectorDialog CreateInstance()
        {
            return (UI_WinModSelectorDialog)UIPackage.CreateObject("NextCore", "WinModSelectorDialog");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_frame = (UI_WindowFrameDialog)GetChild("frame");
            m_bg = (GGraph)GetChild("bg");
            m_table = (UI_ComTableList)GetChild("table");
            m_inspector = (UI_ComMainInspector)GetChild("inspector");
            m_btnConfirm = (GButton)GetChild("btnConfirm");
            m_sep = (GGraph)GetChild("sep");
        }
    }
}