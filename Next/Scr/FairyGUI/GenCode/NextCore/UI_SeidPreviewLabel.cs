/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore;

public partial class UI_SeidPreviewLabel : GLabel
{
    public Controller m_button;
    public const string URL = "ui://028qk31hspup3r";

    public static UI_SeidPreviewLabel CreateInstance()
    {
        return (UI_SeidPreviewLabel)UIPackage.CreateObject("NextCore", "SeidPreviewLabel");
    }

    public override void ConstructFromXML(XML xml)
    {
        base.ConstructFromXML(xml);

        m_button = GetController("button");
    }
}