/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore
{
    public partial class UI_WinSelectorDialog : GComponent
    {
        public UI_WindowFrame2 m_frame;
        public GButton m_btnOk;
        public GButton m_closeButton;
        public GImage m_bg;
        public GTextField m_txtTips;
        public UI_ComTableList m_table;
        public const string URL = "ui://028qk31hnkvz33";

        public static UI_WinSelectorDialog CreateInstance()
        {
            return (UI_WinSelectorDialog)UIPackage.CreateObject("NextCore", "WinSelectorDialog");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_frame = (UI_WindowFrame2)GetChild("frame");
            m_btnOk = (GButton)GetChild("btnOk");
            m_closeButton = (GButton)GetChild("closeButton");
            m_bg = (GImage)GetChild("bg");
            m_txtTips = (GTextField)GetChild("txtTips");
            m_table = (UI_ComTableList)GetChild("table");
        }
    }
}