/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore
{
    public partial class UI_ModMainPanel : GComponent
    {
        public UI_WindowFrame_style2 m_frame;
        public const string URL = "ui://028qk31hq7os0";

        public static UI_ModMainPanel CreateInstance()
        {
            return (UI_ModMainPanel)UIPackage.CreateObject("NextCore", "ModMainPanel");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_frame = (UI_WindowFrame_style2)GetChild("frame");
        }
    }
}