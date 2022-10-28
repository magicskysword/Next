/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore
{
    public partial class UI_ComSliderDrawer_slider : GSlider
    {
        public GGraph m_bg;
        public const string URL = "ui://028qk31htxbi5b";

        public static UI_ComSliderDrawer_slider CreateInstance()
        {
            return (UI_ComSliderDrawer_slider)UIPackage.CreateObject("NextCore", "ComSliderDrawer_slider");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_bg = (GGraph)GetChild("bg");
        }
    }
}