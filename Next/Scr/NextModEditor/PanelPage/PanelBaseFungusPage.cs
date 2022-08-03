using System.Collections.Generic;
using System.Linq;
using FairyGUI;
using SkySwordKill.Next.FGUI.Component;
using SkySwordKill.Next.FCanvas;
using SkySwordKill.NextEditor.Mod;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.NextEditor.PanelPage
{
    public class PanelBaseFungusPage : PanelPageBase
    {
        public PanelBaseFungusPage(string name) : base(name)
        {
            
        }
        
        public CtlDocumentNodeView NodeView;

        protected override GObject OnAdd()
        {
            NodeView = new CtlDocumentNodeView(UI_ComMainDocumentNodeView.CreateInstance());
            NodeView.Init(new List<TableInfo>()
            {
                new TableInfo(
                    "ID",
                    TableInfo.DEFAULT_GRID_WIDTH * 1.5f,
                    data =>
                    {
                        var flowchart = (FFlowchart)data;
                        return flowchart.Name;
                    })
            }, ModEditorManager.I.DefaultFFlowchart.Values.ToList());
            
            return NodeView.MainView;
        }

        protected override void OnOpen()
        {
            
        }

        protected override void OnRemove()
        {
            
        }
    }
}