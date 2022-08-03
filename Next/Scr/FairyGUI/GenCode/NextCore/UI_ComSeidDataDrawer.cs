/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore
{
    public partial class UI_ComSeidDataDrawer : GLabel
    {
        public GList m_lstSeid;
        public GButton m_btnEdit;
        public const string URL = "ui://028qk31h7exm3k";

        public static UI_ComSeidDataDrawer CreateInstance()
        {
            return (UI_ComSeidDataDrawer)UIPackage.CreateObject("NextCore", "ComSeidDataDrawer");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_lstSeid = (GList)GetChild("lstSeid");
            m_btnEdit = (GButton)GetChild("btnEdit");
        }
    }
}