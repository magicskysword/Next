/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore
{
    public partial class UI_ComMainInspector : GComponent
    {
        public GList m_list;
        public const string URL = "ui://028qk31hnkvz2f";

        public static UI_ComMainInspector CreateInstance()
        {
            return (UI_ComMainInspector)UIPackage.CreateObject("NextCore", "ComMainInspector");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_list = (GList)GetChild("list");
        }
    }
}