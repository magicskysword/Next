/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore;

public partial class UI_WinTableDialog : GComponent
{
    public UI_WindowFrameDialog m_frame;
    public GButton m_btnOk;
    public GButton m_closeButton;
    public GGraph m_bg;
    public GGraph m_bgToolsBar;
    public GTextField m_txtTips;
    public UI_ComTableList m_table;
    public UI_ComToolsBar m_toolsBar;
    public const string URL = "ui://028qk31hnkvz33";

    public static UI_WinTableDialog CreateInstance()
    {
        return (UI_WinTableDialog)UIPackage.CreateObject("NextCore", "WinTableDialog");
    }

    public override void ConstructFromXML(XML xml)
    {
        base.ConstructFromXML(xml);

        m_frame = (UI_WindowFrameDialog)GetChild("frame");
        m_btnOk = (GButton)GetChild("btnOk");
        m_closeButton = (GButton)GetChild("closeButton");
        m_bg = (GGraph)GetChild("bg");
        m_bgToolsBar = (GGraph)GetChild("bgToolsBar");
        m_txtTips = (GTextField)GetChild("txtTips");
        m_table = (UI_ComTableList)GetChild("table");
        m_toolsBar = (UI_ComToolsBar)GetChild("toolsBar");
    }
}