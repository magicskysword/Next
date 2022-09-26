/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore;

public partial class UI_ComTextPreviewArea : GComponent
{
    public GRichTextField m_txtPreview;
    public const string URL = "ui://028qk31hqiuf4q";

    public static UI_ComTextPreviewArea CreateInstance()
    {
        return (UI_ComTextPreviewArea)UIPackage.CreateObject("NextCore", "ComTextPreviewArea");
    }

    public override void ConstructFromXML(XML xml)
    {
        base.ConstructFromXML(xml);

        m_txtPreview = (GRichTextField)GetChild("txtPreview");
    }
}