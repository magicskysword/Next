/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore
{
    public partial class UI_ComMainHeader : GComponent
    {
        public GList m_lstHeader;
        public const string URL = "ui://028qk31hnkvz24";

        public static UI_ComMainHeader CreateInstance()
        {
            return (UI_ComMainHeader)UIPackage.CreateObject("NextCore", "ComMainHeader");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_lstHeader = (GList)GetChild("lstHeader");
        }
    }
}