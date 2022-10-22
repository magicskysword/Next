/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore
{
    public partial class UI_ComInputArea : GComponent
    {
        public GTextInput m_input;
        public const string URL = "ui://028qk31h7exm3i";

        public static UI_ComInputArea CreateInstance()
        {
            return (UI_ComInputArea)UIPackage.CreateObject("NextCore", "ComInputArea");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_input = (GTextInput)GetChild("input");
        }
    }
}