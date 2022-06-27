/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore
{
    public partial class UI_WindowFrame2 : GLabel
    {
        public Controller m_hasCloseButton;
        public GGraph m_dragArea;
        public GGraph m_contentArea;
        public GButton m_closeButton;
        public const string URL = "ui://028qk31hnkvz2m";

        public static UI_WindowFrame2 CreateInstance()
        {
            return (UI_WindowFrame2)UIPackage.CreateObject("NextCore", "WindowFrame2");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_hasCloseButton = GetController("hasCloseButton");
            m_dragArea = (GGraph)GetChild("dragArea");
            m_contentArea = (GGraph)GetChild("contentArea");
            m_closeButton = (GButton)GetChild("closeButton");
        }
    }
}