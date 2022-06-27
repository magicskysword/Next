/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore
{
    public partial class UI_ComMainDocumentView : GComponent
    {
        public GList m_lstTab;
        public GList m_content;
        public const string URL = "ui://028qk31hnkvz29";

        public static UI_ComMainDocumentView CreateInstance()
        {
            return (UI_ComMainDocumentView)UIPackage.CreateObject("NextCore", "ComMainDocumentView");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_lstTab = (GList)GetChild("lstTab");
            m_content = (GList)GetChild("content");
        }
    }
}