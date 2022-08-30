/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore
{
    public partial class UI_WinCreateWorkshopModDialog : GComponent
    {
        public UI_WindowFrameDialog m_frame;
        public UI_ComMainInspector m_inspector;
        public GButton m_btnCancel;
        public GButton m_btnConfirm;
        public UI_ComListProject m_projectList;
        public GGraph m_sep;
        public const string URL = "ui://028qk31hdtat4b";

        public static UI_WinCreateWorkshopModDialog CreateInstance()
        {
            return (UI_WinCreateWorkshopModDialog)UIPackage.CreateObject("NextCore", "WinCreateWorkshopModDialog");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_frame = (UI_WindowFrameDialog)GetChild("frame");
            m_inspector = (UI_ComMainInspector)GetChild("inspector");
            m_btnCancel = (GButton)GetChild("btnCancel");
            m_btnConfirm = (GButton)GetChild("btnConfirm");
            m_projectList = (UI_ComListProject)GetChild("projectList");
            m_sep = (GGraph)GetChild("sep");
        }
    }
}