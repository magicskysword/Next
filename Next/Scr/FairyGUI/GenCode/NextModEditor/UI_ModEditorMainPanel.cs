/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextModEditor
{
    public partial class UI_ModEditorMainPanel : GComponent
    {
        public GComponent m_comHeader;
        public GComponent m_comProject;
        public GComponent m_comDocument;
        public GComponent m_comInspector;
        public GComponent m_comFooter;
        public GGraph m_leftSeg;
        public GGraph m_rightSeg;
        public const string URL = "ui://kvh7d0b3nkvz30";

        public static UI_ModEditorMainPanel CreateInstance()
        {
            return (UI_ModEditorMainPanel)UIPackage.CreateObject("NextModEditor", "ModEditorMainPanel");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_comHeader = (GComponent)GetChild("comHeader");
            m_comProject = (GComponent)GetChild("comProject");
            m_comDocument = (GComponent)GetChild("comDocument");
            m_comInspector = (GComponent)GetChild("comInspector");
            m_comFooter = (GComponent)GetChild("comFooter");
            m_leftSeg = (GGraph)GetChild("leftSeg");
            m_rightSeg = (GGraph)GetChild("rightSeg");
        }
    }
}