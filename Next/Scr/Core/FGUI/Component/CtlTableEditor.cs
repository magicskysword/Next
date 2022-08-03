using FairyGUI;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.Next.FGUI.Component
{
    public class CtlTableEditor
    {
        public CtlTableEditor(UI_ComTableEditor tableEditor)
        {
            MainView = tableEditor;
            TableList = new CtlTableList(tableEditor.m_table);
            Inspector = new CtlPropertyInspector(tableEditor.m_inspector);
            ToolsBar = new CtlToolsBar(tableEditor.m_toolsBar);
            
            FGUITools.BindHSeg(tableEditor.m_seg, 
                () => MainView.width * 0.1f, 
                () => MainView.width * 0.9f);
        }
        
        public UI_ComTableEditor MainView { get; }
        public CtlTableList TableList { get; }
        public CtlPropertyInspector Inspector { get; }
        public CtlToolsBar ToolsBar { get; }
    }
}