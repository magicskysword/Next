/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore
{
    public partial class UI_WinCreateABMod : GComponent
    {
        public UI_WindowFrameDialogStyle2 m_frame;
        public GButton m_btnOk;
        public GButton m_btnCancle;
        public UI_ComTitleDrawer m_title;
        public UI_ComStringDrawer m_inputABName;
        public UI_ComStringDrawer m_inputProjectPath;
        public UI_ComStringDrawer m_inputExportPath;
        public GGroup m_groupInfo;
        public GButton m_btnEditProjectPath;
        public GButton m_btnEditOutputABPath;
        public const string URL = "ui://028qk31hc5lpj5h";

        public static UI_WinCreateABMod CreateInstance()
        {
            return (UI_WinCreateABMod)UIPackage.CreateObject("NextCore", "WinCreateABMod");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_frame = (UI_WindowFrameDialogStyle2)GetChild("frame");
            m_btnOk = (GButton)GetChild("btnOk");
            m_btnCancle = (GButton)GetChild("btnCancle");
            m_title = (UI_ComTitleDrawer)GetChild("title");
            m_inputABName = (UI_ComStringDrawer)GetChild("inputABName");
            m_inputProjectPath = (UI_ComStringDrawer)GetChild("inputProjectPath");
            m_inputExportPath = (UI_ComStringDrawer)GetChild("inputExportPath");
            m_groupInfo = (GGroup)GetChild("groupInfo");
            m_btnEditProjectPath = (GButton)GetChild("btnEditProjectPath");
            m_btnEditOutputABPath = (GButton)GetChild("btnEditOutputABPath");
        }
    }
}