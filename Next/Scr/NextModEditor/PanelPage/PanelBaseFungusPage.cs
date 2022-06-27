using System.Collections.Generic;
using System.Linq;
using SkySwordKill.Next.FCanvas;
using SkySwordKill.Next.FGUI.ComponentCtl;
using SkySwordKill.NextEditor.Mod;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.NextEditor.PanelPage
{
    public class PanelBaseFungusPage : PanelPageBase
    {
        public CtlDocumentNodeView NodeView;

        public override void OnAdd()
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
            }, ModMgr.I.DefaultFFlowchart.Values.ToList());
            Content = NodeView.MainView;
        }

        public override void OnOpen()
        {
            
        }

        public override void OnRemove()
        {
            Content.Dispose();
        }
    }
}