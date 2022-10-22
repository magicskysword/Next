/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore
{
    public partial class UI_ComToolsSearchBox : GComponent
    {
        public GTextInput m_inContent;
        public GButton m_btnSearch;
        public GButton m_btnReset;
        public const string URL = "ui://028qk31hd4rw3z";

        public static UI_ComToolsSearchBox CreateInstance()
        {
            return (UI_ComToolsSearchBox)UIPackage.CreateObject("NextCore", "ComToolsSearchBox");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_inContent = (GTextInput)GetChild("inContent");
            m_btnSearch = (GButton)GetChild("btnSearch");
            m_btnReset = (GButton)GetChild("btnReset");
        }
    }
}