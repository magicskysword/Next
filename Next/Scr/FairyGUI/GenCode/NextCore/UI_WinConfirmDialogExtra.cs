/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore
{
    public partial class UI_WinConfirmDialogExtra : GComponent
    {
        public Controller m_type;
        public UI_WindowFrameDialog m_frame;
        public GLabel m_text;
        public GButton m_btnOk;
        public GButton m_closeButton;
        public GButton m_tglExtra;
        public GTextField m_txtExtra;
        public const string URL = "ui://028qk31hpk2ij5d";

        public static UI_WinConfirmDialogExtra CreateInstance()
        {
            return (UI_WinConfirmDialogExtra)UIPackage.CreateObject("NextCore", "WinConfirmDialogExtra");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_type = GetController("type");
            m_frame = (UI_WindowFrameDialog)GetChild("frame");
            m_text = (GLabel)GetChild("text");
            m_btnOk = (GButton)GetChild("btnOk");
            m_closeButton = (GButton)GetChild("closeButton");
            m_tglExtra = (GButton)GetChild("tglExtra");
            m_txtExtra = (GTextField)GetChild("txtExtra");
        }
    }
}