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
        }
        
        public UI_ComTableEditor MainView { get; set; }
        public CtlTableList TableList { get; set; }
        public CtlPropertyInspector Inspector { get; set; }
    }
}