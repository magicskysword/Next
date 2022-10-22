/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore
{
    public partial class UI_WindowFrameDialog : GLabel
    {
        public Controller m_hasCloseButton;
        public Controller m_canResize;
        public GGraph m_bgContent;
        public GGraph m_bgTitle;
        public GGraph m_line;
        public GGraph m_dragArea;
        public GGraph m_contentArea;
        public GButton m_closeButton;
        public GLoader m_resizeHandle;
        public const string URL = "ui://028qk31hnkvz2m";

        public static UI_WindowFrameDialog CreateInstance()
        {
            return (UI_WindowFrameDialog)UIPackage.CreateObject("NextCore", "WindowFrameDialog");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_hasCloseButton = GetController("hasCloseButton");
            m_canResize = GetController("canResize");
            m_bgContent = (GGraph)GetChild("bgContent");
            m_bgTitle = (GGraph)GetChild("bgTitle");
            m_line = (GGraph)GetChild("line");
            m_dragArea = (GGraph)GetChild("dragArea");
            m_contentArea = (GGraph)GetChild("contentArea");
            m_closeButton = (GButton)GetChild("closeButton");
            m_resizeHandle = (GLoader)GetChild("resizeHandle");
        }
    }
}