/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore
{
    public partial class UI_WindowFrameDialogStyle2 : GLabel
    {
        public GGraph m_bgContent;
        public GGraph m_bgBar;
        public GGraph m_dragArea;
        public GGraph m_contentArea;
        public GButton m_closeButton;
        public GGraph m_line;
        public const string URL = "ui://028qk31hq0gg3d";

        public static UI_WindowFrameDialogStyle2 CreateInstance()
        {
            return (UI_WindowFrameDialogStyle2)UIPackage.CreateObject("NextCore", "WindowFrameDialogStyle2");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_bgContent = (GGraph)GetChild("bgContent");
            m_bgBar = (GGraph)GetChild("bgBar");
            m_dragArea = (GGraph)GetChild("dragArea");
            m_contentArea = (GGraph)GetChild("contentArea");
            m_closeButton = (GButton)GetChild("closeButton");
            m_line = (GGraph)GetChild("line");
        }
    }
}