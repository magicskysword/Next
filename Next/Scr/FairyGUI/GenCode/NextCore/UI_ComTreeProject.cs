/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore
{
    public partial class UI_ComTreeProject : GComponent
    {
        public Controller m_showToolbar;
        public GTree m_treeView;
        public UI_ComToolsBar m_toolsBar;
        public const string URL = "ui://028qk31hnkvz25";

        public static UI_ComTreeProject CreateInstance()
        {
            return (UI_ComTreeProject)UIPackage.CreateObject("NextCore", "ComTreeProject");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_showToolbar = GetController("showToolbar");
            m_treeView = (GTree)GetChild("treeView");
            m_toolsBar = (UI_ComToolsBar)GetChild("toolsBar");
        }
    }
}