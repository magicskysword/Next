/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore
{
    public partial class UI_ComCmdText : GButton
    {
        public Controller m_bg;
        public GGraph m_indent;
        public GGraph m_bg_2;
        public const string URL = "ui://028qk31hasvv39";

        public static UI_ComCmdText CreateInstance()
        {
            return (UI_ComCmdText)UIPackage.CreateObject("NextCore", "ComCmdText");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_bg = GetController("bg");
            m_indent = (GGraph)GetChild("indent");
            m_bg_2 = (GGraph)GetChild("bg");
        }
    }
}