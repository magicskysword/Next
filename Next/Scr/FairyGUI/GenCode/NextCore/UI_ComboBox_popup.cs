/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore
{
    public partial class UI_ComboBox_popup : GComponent
    {
        public GList m_list;
        public const string URL = "ui://028qk31hnkvz1u";

        public static UI_ComboBox_popup CreateInstance()
        {
            return (UI_ComboBox_popup)UIPackage.CreateObject("NextCore", "ComboBox_popup");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_list = (GList)GetChild("list");
        }
    }
}