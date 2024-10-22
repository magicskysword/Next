/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore
{
    public partial class UI_ComMainDocumentNodeView : GComponent
    {
        public GGraph m_bgTable;
        public UI_ComTableList m_list;
        public UI_ComNodeCanvas m_nodeCanvas;
        public UI_ComMainInspector m_inspector;
        public GGraph m_frameLine;
        public GGraph m_frameLineRight;
        public GTextField m_txtState;
        public GGraph m_segLeft;
        public GGraph m_segRight;
        public const string URL = "ui://028qk31hasvv3b";

        public static UI_ComMainDocumentNodeView CreateInstance()
        {
            return (UI_ComMainDocumentNodeView)UIPackage.CreateObject("NextCore", "ComMainDocumentNodeView");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_bgTable = (GGraph)GetChild("bgTable");
            m_list = (UI_ComTableList)GetChild("list");
            m_nodeCanvas = (UI_ComNodeCanvas)GetChild("nodeCanvas");
            m_inspector = (UI_ComMainInspector)GetChild("inspector");
            m_frameLine = (GGraph)GetChild("frameLine");
            m_frameLineRight = (GGraph)GetChild("frameLineRight");
            m_txtState = (GTextField)GetChild("txtState");
            m_segLeft = (GGraph)GetChild("segLeft");
            m_segRight = (GGraph)GetChild("segRight");
        }
    }
}