/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore
{
    public partial class UI_WinStringInputDialog : GComponent
    {
        public UI_WindowFrame2 m_frame;
        public GButton m_btnOk;
        public GButton m_closeButton;
        public GTextInput m_inContent;
        public const string URL = "ui://028qk31hnkvz31";

        public static UI_WinStringInputDialog CreateInstance()
        {
            return (UI_WinStringInputDialog)UIPackage.CreateObject("NextCore", "WinStringInputDialog");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_frame = (UI_WindowFrame2)GetChild("frame");
            m_btnOk = (GButton)GetChild("btnOk");
            m_closeButton = (GButton)GetChild("closeButton");
            m_inContent = (GTextInput)GetChild("inContent");
        }
    }
}