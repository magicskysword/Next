/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore;

public partial class UI_ComboBox : GComboBox
{
    public Controller m_grayed;
    public const string URL = "ui://028qk31hnkvz1w";

    public static UI_ComboBox CreateInstance()
    {
        return (UI_ComboBox)UIPackage.CreateObject("NextCore", "ComboBox");
    }

    public override void ConstructFromXML(XML xml)
    {
        base.ConstructFromXML(xml);

        m_grayed = GetController("grayed");
    }
}