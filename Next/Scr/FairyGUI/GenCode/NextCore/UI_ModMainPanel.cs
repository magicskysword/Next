/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore
{
    public partial class UI_ModMainPanel : GComponent
    {
        public UI_WindowFrameDialogStyle2 m_frame;
        public GGraph m_bgTable;
        public GGraph m_bgTools;
        public GGraph m_bg_seg;
        public UI_ComTreeProject m_mods;
        public UI_ComMainInspector m_inspector;
        public GGraph m_seg;
        public GButton m_btnApply;
        public GList m_listLink;
        public const string URL = "ui://028qk31hn15k4w";

        public static UI_ModMainPanel CreateInstance()
        {
            return (UI_ModMainPanel)UIPackage.CreateObject("NextCore", "ModMainPanel");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_frame = (UI_WindowFrameDialogStyle2)GetChild("frame");
            m_bgTable = (GGraph)GetChild("bgTable");
            m_bgTools = (GGraph)GetChild("bgTools");
            m_bg_seg = (GGraph)GetChild("bg_seg");
            m_mods = (UI_ComTreeProject)GetChild("mods");
            m_inspector = (UI_ComMainInspector)GetChild("inspector");
            m_seg = (GGraph)GetChild("seg");
            m_btnApply = (GButton)GetChild("btnApply");
            m_listLink = (GList)GetChild("listLink");
        }
    }
}