/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace SkySwordKill.NextFGUI.NextCore;

public partial class UI_WinSeidEditorDialog : GComponent
{
    public UI_WindowFrameDialog m_frame;
    public GGraph m_bgSeid;
    public GTree m_list;
    public UI_ComMainInspector m_inspector;
    public GButton m_btnAdd;
    public GButton m_btnRemove;
    public GButton m_btnEnable;
    public GButton m_btnDisable;
    public GButton m_btnMoveUp;
    public GButton m_btnMoveDown;
    public const string URL = "ui://028qk31h7exm3n";

    public static UI_WinSeidEditorDialog CreateInstance()
    {
        return (UI_WinSeidEditorDialog)UIPackage.CreateObject("NextCore", "WinSeidEditorDialog");
    }

    public override void ConstructFromXML(XML xml)
    {
        base.ConstructFromXML(xml);

        m_frame = (UI_WindowFrameDialog)GetChild("frame");
        m_bgSeid = (GGraph)GetChild("bgSeid");
        m_list = (GTree)GetChild("list");
        m_inspector = (UI_ComMainInspector)GetChild("inspector");
        m_btnAdd = (GButton)GetChild("btnAdd");
        m_btnRemove = (GButton)GetChild("btnRemove");
        m_btnEnable = (GButton)GetChild("btnEnable");
        m_btnDisable = (GButton)GetChild("btnDisable");
        m_btnMoveUp = (GButton)GetChild("btnMoveUp");
        m_btnMoveDown = (GButton)GetChild("btnMoveDown");
    }
}