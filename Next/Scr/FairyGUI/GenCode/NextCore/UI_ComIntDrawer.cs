/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore
{
    public partial class UI_ComIntDrawer : GLabel
    {
        public Controller m_warning;
        public GTextInput m_inContent;
        public const string URL = "ui://028qk31hnkvz1y";

        public static UI_ComIntDrawer CreateInstance()
        {
            return (UI_ComIntDrawer)UIPackage.CreateObject("NextCore", "ComIntDrawer");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_warning = GetController("warning");
            m_inContent = (GTextInput)GetChild("inContent");
        }
    }
}