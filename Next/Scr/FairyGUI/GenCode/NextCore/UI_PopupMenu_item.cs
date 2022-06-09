/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore
{
    public partial class UI_PopupMenu_item : GButton
    {
        public Controller m_checked;
        public GImage m_select;
        public GImage m_arrow;
        public const string URL = "ui://028qk31hnkvz2h";

        public static UI_PopupMenu_item CreateInstance()
        {
            return (UI_PopupMenu_item)UIPackage.CreateObject("NextCore", "PopupMenu_item");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_checked = GetController("checked");
            m_select = (GImage)GetChild("select");
            m_arrow = (GImage)GetChild("arrow");
        }
    }
}