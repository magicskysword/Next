/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore
{
    public partial class UI_ComDropdownDrawer : GLabel
    {
        public GComboBox m_dropdown;
        public const string URL = "ui://028qk31hnkvz23";

        public static UI_ComDropdownDrawer CreateInstance()
        {
            return (UI_ComDropdownDrawer)UIPackage.CreateObject("NextCore", "ComDropdownDrawer");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_dropdown = (GComboBox)GetChild("dropdown");
        }
    }
}