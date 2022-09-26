using System.Collections.Generic;
using FairyGUI;
using SkySwordKill.Next.FCanvas;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.Next.FGUI.Component;

public class CtlDocumentNodeView
{
    public CtlDocumentNodeView(UI_ComMainDocumentNodeView com)
    {
        MainView = com;
        TableList = new CtlTableList(MainView.m_list);
        NodeCanvas = new CtlNodeCanvas(MainView.m_nodeCanvas);
        NodeCanvas.OnContentPosChange = RefreshNodeCanvasPos;
        RefreshNodeCanvasPos();
    }

    private void RefreshNodeCanvasPos()
    {
        var position = NodeCanvas.MainView.m_zoom.m_container.position;
        MainView.m_txtState.text =
            $"X : {position.x}  Y : {position.y}";
    }

    public UI_ComMainDocumentNodeView MainView { get; set; }
    public CtlTableList TableList { get; set; }
    public CtlNodeCanvas NodeCanvas { get; set; }
    public List<TableInfo> TableInfos { get; set; }
    public List<FFlowchart> FFlowcharts { get; set; }

    public void Init(List<TableInfo> tableInfos, List<FFlowchart> fFlowcharts)
    {
        FFlowcharts = fFlowcharts;
        TableInfos = tableInfos;
        //TableList.BindTable(tableInfos, OnGetFlowchart, OnGetFlowchartCount, OnItemRenderer, OnClickItem);
    }

    private int OnGetFlowchartCount()
    {
        return FFlowcharts.Count;
    }

    private object OnGetFlowchart(int index)
    {
        return FFlowcharts[index];
    }

    private void OnClickItem(EventContext context)
    {
        if(!context.inputEvent.isDoubleClick)
            return;
            
        NodeCanvas.Bind((FFlowchart)OnGetFlowchart(TableList.SelectedIndex));
    }

    private void OnItemRenderer(int index, UI_ComTableRow row)
    {
            
    }
}