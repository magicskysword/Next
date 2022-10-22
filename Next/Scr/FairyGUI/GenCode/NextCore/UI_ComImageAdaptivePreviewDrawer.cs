/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore
{
    public partial class UI_ComImageAdaptivePreviewDrawer : GLabel
    {
        public GGraph m_bg;
        public const string URL = "ui://028qk31hol2b52";

        public static UI_ComImageAdaptivePreviewDrawer CreateInstance()
        {
            return (UI_ComImageAdaptivePreviewDrawer)UIPackage.CreateObject("NextCore", "ComImageAdaptivePreviewDrawer");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_bg = (GGraph)GetChild("bg");
        }
    }
}