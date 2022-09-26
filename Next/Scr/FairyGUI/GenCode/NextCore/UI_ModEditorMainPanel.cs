/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore;

public partial class UI_ModEditorMainPanel : GComponent
{
    public UI_ComMainHeader m_comHeader;
    public UI_ComTreeProject m_comProject;
    public UI_ComMainDocumentView m_comDocument;
    public GComponent m_comFooter;
    public GGraph m_seg;
    public const string URL = "ui://028qk31hnkvz30";

    public static UI_ModEditorMainPanel CreateInstance()
    {
        return (UI_ModEditorMainPanel)UIPackage.CreateObject("NextCore", "ModEditorMainPanel");
    }

    public override void ConstructFromXML(XML xml)
    {
        base.ConstructFromXML(xml);

        m_comHeader = (UI_ComMainHeader)GetChild("comHeader");
        m_comProject = (UI_ComTreeProject)GetChild("comProject");
        m_comDocument = (UI_ComMainDocumentView)GetChild("comDocument");
        m_comFooter = (GComponent)GetChild("comFooter");
        m_seg = (GGraph)GetChild("seg");
    }
}