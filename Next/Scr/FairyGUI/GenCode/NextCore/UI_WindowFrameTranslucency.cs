/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore
{
    public partial class UI_WindowFrameTranslucency : GLabel
    {
        public GGraph m_bgContent;
        public GGraph m_bgBar;
        public GGraph m_dragArea;
        public GGraph m_contentArea;
        public GButton m_closeButton;
        public const string URL = "ui://028qk31hcnag3";

        public static UI_WindowFrameTranslucency CreateInstance()
        {
            return (UI_WindowFrameTranslucency)UIPackage.CreateObject("NextCore", "WindowFrameTranslucency");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_bgContent = (GGraph)GetChild("bgContent");
            m_bgBar = (GGraph)GetChild("bgBar");
            m_dragArea = (GGraph)GetChild("dragArea");
            m_contentArea = (GGraph)GetChild("contentArea");
            m_closeButton = (GButton)GetChild("closeButton");
        }
    }
}