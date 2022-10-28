/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore
{
    public partial class UI_ComSliderDrawer : GComponent
    {
        public GTextField m_txt_title;
        public GGraph m_bg_input;
        public GTextInput m_txt_input;
        public UI_ComSliderDrawer_slider m_slider;
        public const string URL = "ui://028qk31htp225a";

        public static UI_ComSliderDrawer CreateInstance()
        {
            return (UI_ComSliderDrawer)UIPackage.CreateObject("NextCore", "ComSliderDrawer");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_txt_title = (GTextField)GetChild("txt_title");
            m_bg_input = (GGraph)GetChild("bg_input");
            m_txt_input = (GTextInput)GetChild("txt_input");
            m_slider = (UI_ComSliderDrawer_slider)GetChild("slider");
        }
    }
}