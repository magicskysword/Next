/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore
{
    public partial class UI_ComTableList : GComponent
    {
        public GList m_listHeader;
        public GGraph m_bgList;
        public GList m_list;
        public const string URL = "ui://028qk31hnkvz2b";

        public static UI_ComTableList CreateInstance()
        {
            return (UI_ComTableList)UIPackage.CreateObject("NextCore", "ComTableList");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_listHeader = (GList)GetChild("listHeader");
            m_bgList = (GGraph)GetChild("bgList");
            m_list = (GList)GetChild("list");
        }
    }
}