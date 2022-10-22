/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore
{
    public partial class UI_BtnListItemProject : GButton
    {
        public GGraph m_selected;
        public GGraph m_hover;
        public const string URL = "ui://028qk31hro2x4c";

        public static UI_BtnListItemProject CreateInstance()
        {
            return (UI_BtnListItemProject)UIPackage.CreateObject("NextCore", "BtnListItemProject");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_selected = (GGraph)GetChild("selected");
            m_hover = (GGraph)GetChild("hover");
        }
    }
}