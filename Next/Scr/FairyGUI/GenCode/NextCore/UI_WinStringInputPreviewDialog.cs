/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore;

public partial class UI_WinStringInputPreviewDialog : GComponent
{
    public UI_WindowFrameDialog m_frame;
    public UI_ComInputArea m_inContent;
    public UI_ComTextPreviewArea m_comTextPreview;
    public GGraph m_seg;
    public GButton m_btnOk;
    public GButton m_closeButton;
    public const string URL = "ui://028qk31hqiuf4p";

    public static UI_WinStringInputPreviewDialog CreateInstance()
    {
        return (UI_WinStringInputPreviewDialog)UIPackage.CreateObject("NextCore", "WinStringInputPreviewDialog");
    }

    public override void ConstructFromXML(XML xml)
    {
        base.ConstructFromXML(xml);

        m_frame = (UI_WindowFrameDialog)GetChild("frame");
        m_inContent = (UI_ComInputArea)GetChild("inContent");
        m_comTextPreview = (UI_ComTextPreviewArea)GetChild("comTextPreview");
        m_seg = (GGraph)GetChild("seg");
        m_btnOk = (GButton)GetChild("btnOk");
        m_closeButton = (GButton)GetChild("closeButton");
    }
}