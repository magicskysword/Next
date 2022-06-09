/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore
{
    public partial class UI_PopupMenu : GComponent
    {
        public GList m_list;
        public const string URL = "ui://028qk31hnkvz2j";

        public static UI_PopupMenu CreateInstance()
        {
            return (UI_PopupMenu)UIPackage.CreateObject("NextCore", "PopupMenu");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_list = (GList)GetChild("list");
        }
    }
}