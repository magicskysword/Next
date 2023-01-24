/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore
{
    public partial class UI_AnimWait : GComponent
    {
        public Transition m_t1;
        public const string URL = "ui://028qk31hf830j5f";

        public static UI_AnimWait CreateInstance()
        {
            return (UI_AnimWait)UIPackage.CreateObject("NextCore", "AnimWait");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_t1 = GetTransition("t1");
        }
    }
}