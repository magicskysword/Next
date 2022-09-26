/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore;

public partial class UI_ComStringBindDataDrawer : GLabel
{
    public Controller m_warning;
    public Controller m_grayed;
    public GTextInput m_inContent;
    public GTextField m_txtDesc;
    public GButton m_btnEdit;
    public const string URL = "ui://028qk31hmdyt4i";

    public static UI_ComStringBindDataDrawer CreateInstance()
    {
        return (UI_ComStringBindDataDrawer)UIPackage.CreateObject("NextCore", "ComStringBindDataDrawer");
    }

    public override void ConstructFromXML(XML xml)
    {
        base.ConstructFromXML(xml);

        m_warning = GetController("warning");
        m_grayed = GetController("grayed");
        m_inContent = (GTextInput)GetChild("inContent");
        m_txtDesc = (GTextField)GetChild("txtDesc");
        m_btnEdit = (GButton)GetChild("btnEdit");
    }
}