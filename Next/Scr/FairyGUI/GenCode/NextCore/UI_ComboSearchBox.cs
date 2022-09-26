/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore;

public partial class UI_ComboSearchBox : GComboBox
{
    public Controller m_grayed;
    public const string URL = "ui://028qk31hmdyt4j";

    public static UI_ComboSearchBox CreateInstance()
    {
        return (UI_ComboSearchBox)UIPackage.CreateObject("NextCore", "ComboSearchBox");
    }

    public override void ConstructFromXML(XML xml)
    {
        base.ConstructFromXML(xml);

        m_grayed = GetController("grayed");
    }
}