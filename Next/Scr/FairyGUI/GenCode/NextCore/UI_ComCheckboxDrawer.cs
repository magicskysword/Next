/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore;

public partial class UI_ComCheckboxDrawer : GLabel
{
    public GButton m_checkbox;
    public const string URL = "ui://028qk31hrabj4a";

    public static UI_ComCheckboxDrawer CreateInstance()
    {
        return (UI_ComCheckboxDrawer)UIPackage.CreateObject("NextCore", "ComCheckboxDrawer");
    }

    public override void ConstructFromXML(XML xml)
    {
        base.ConstructFromXML(xml);

        m_checkbox = (GButton)GetChild("checkbox");
    }
}