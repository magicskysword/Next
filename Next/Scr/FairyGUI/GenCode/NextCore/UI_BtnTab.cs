/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore
{
    public partial class UI_BtnTab : GButton
    {
        public GGraph m_dragArea;
        public GButton m_closeButton;
        public const string URL = "ui://028qk31hnkvz1o";

        public static UI_BtnTab CreateInstance()
        {
            return (UI_BtnTab)UIPackage.CreateObject("NextCore", "BtnTab");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_dragArea = (GGraph)GetChild("dragArea");
            m_closeButton = (GButton)GetChild("closeButton");
        }
    }
}