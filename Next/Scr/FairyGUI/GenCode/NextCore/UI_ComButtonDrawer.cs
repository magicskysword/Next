/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore
{
    public partial class UI_ComButtonDrawer : GLabel
    {
        public GButton m_button;
        public const string URL = "ui://028qk31hs4q2j5i";

        public static UI_ComButtonDrawer CreateInstance()
        {
            return (UI_ComButtonDrawer)UIPackage.CreateObject("NextCore", "ComButtonDrawer");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_button = (GButton)GetChild("button");
        }
    }
}