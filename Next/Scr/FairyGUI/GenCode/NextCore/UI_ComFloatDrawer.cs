/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore
{
    public partial class UI_ComFloatDrawer : GLabel
    {
        public GTextInput m_inContent;
        public const string URL = "ui://028qk31hnkvz20";

        public static UI_ComFloatDrawer CreateInstance()
        {
            return (UI_ComFloatDrawer)UIPackage.CreateObject("NextCore", "ComFloatDrawer");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_inContent = (GTextInput)GetChild("inContent");
        }
    }
}