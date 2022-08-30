/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore
{
    public partial class UI_ComStringDrawer : GLabel
    {
        public Controller m_grayed;
        public GTextInput m_inContent;
        public const string URL = "ui://028qk31hnkvz1x";

        public static UI_ComStringDrawer CreateInstance()
        {
            return (UI_ComStringDrawer)UIPackage.CreateObject("NextCore", "ComStringDrawer");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_grayed = GetController("grayed");
            m_inContent = (GTextInput)GetChild("inContent");
        }
    }
}