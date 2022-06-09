/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore
{
    public partial class UI_ComStringAreaDrawer : GLabel
    {
        public GTextInput m_inContent;
        public GButton m_btnEdit;
        public const string URL = "ui://028qk31hnkvz21";

        public static UI_ComStringAreaDrawer CreateInstance()
        {
            return (UI_ComStringAreaDrawer)UIPackage.CreateObject("NextCore", "ComStringAreaDrawer");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_inContent = (GTextInput)GetChild("inContent");
            m_btnEdit = (GButton)GetChild("btnEdit");
        }
    }
}