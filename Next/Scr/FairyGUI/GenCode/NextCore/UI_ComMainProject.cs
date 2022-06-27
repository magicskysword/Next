/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore
{
    public partial class UI_ComMainProject : GComponent
    {
        public GTextInput m_inSearch;
        public GTree m_treeView;
        public const string URL = "ui://028qk31hnkvz25";

        public static UI_ComMainProject CreateInstance()
        {
            return (UI_ComMainProject)UIPackage.CreateObject("NextCore", "ComMainProject");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_inSearch = (GTextInput)GetChild("inSearch");
            m_treeView = (GTree)GetChild("treeView");
        }
    }
}