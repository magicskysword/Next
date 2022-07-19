/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore
{
    public partial class UI_ComMainModList : GComponent
    {
        public GGraph m_bg;
        public GList m_list;
        public const string URL = "ui://028qk31hq0gg3e";

        public static UI_ComMainModList CreateInstance()
        {
            return (UI_ComMainModList)UIPackage.CreateObject("NextCore", "ComMainModList");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_bg = (GGraph)GetChild("bg");
            m_list = (GList)GetChild("list");
        }
    }
}