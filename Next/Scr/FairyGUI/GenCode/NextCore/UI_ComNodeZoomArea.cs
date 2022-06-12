/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore
{
    public partial class UI_ComNodeZoomArea : GComponent
    {
        public GComponent m_container;
        public const string URL = "ui://028qk31hasvv3c";

        public static UI_ComNodeZoomArea CreateInstance()
        {
            return (UI_ComNodeZoomArea)UIPackage.CreateObject("NextCore", "ComNodeZoomArea");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_container = (GComponent)GetChild("container");
        }
    }
}