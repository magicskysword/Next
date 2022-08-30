/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore
{
    public partial class UI_ComboSearchBox_item : GButton
    {
        public GGraph m_selected;
        public GGraph m_hover;
        public const string URL = "ui://028qk31hmdyt4m";

        public static UI_ComboSearchBox_item CreateInstance()
        {
            return (UI_ComboSearchBox_item)UIPackage.CreateObject("NextCore", "ComboSearchBox_item");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_selected = (GGraph)GetChild("selected");
            m_hover = (GGraph)GetChild("hover");
        }
    }
}