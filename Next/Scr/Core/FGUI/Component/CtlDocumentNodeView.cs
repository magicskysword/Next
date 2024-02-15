using System.Collections.Generic;
using FairyGUI;
using SkySwordKill.Next.Extension;
using SkySwordKill.Next.FCanvas;
using SkySwordKill.Next.FGUI.ComponentBuilder;
using SkySwordKill.NextFGUI.NextCore;
using SkySwordKill.NextModEditor.PanelPage;
using UnityEngine;

namespace SkySwordKill.Next.FGUI.Component;

public class CtlDocumentNodeView
{
    public CtlDocumentNodeView(UI_ComMainDocumentNodeView com)
    {
        MainView = com;
        TableList = new CtlTableList(MainView.m_list);
        NodeCanvas = new CtlNodeCanvas(MainView.m_nodeCanvas);
        Inspector = new CtlPropertyInspector(MainView.m_inspector);
        
        NodeCanvas.OnContentPosChange = RefreshNodeCanvasPos;
        NodeCanvas.PosScale = new Vector2(2, 4);
        NodeCanvas.SetOnClickCommand(OnClickCommand);
    }

    public UI_ComMainDocumentNodeView MainView { get; set; }
    public CtlTableList TableList { get; set; }
    public CtlNodeCanvas NodeCanvas { get; set; }
    public CtlPropertyInspector Inspector { get; set; }
    public List<TableInfo> TableInfos { get; set; }
    public List<FFlowchart> FFlowcharts { get; set; }
    
    public FFlowchart CurFlowchart { get; set; }

    protected virtual void RefreshNodeCanvasPos()
    {
        var position = NodeCanvas.MainView.m_zoom.m_container.position;
        MainView.m_txtState.text =
            $"X : {position.x}  Y : {position.y}";
    }
    
    public void Init(List<TableInfo> tableInfos, List<FFlowchart> fFlowcharts)
    {
        FFlowcharts = fFlowcharts;
        TableInfos = tableInfos;
        TableList.BindTable(tableInfos, new ModDataTableDataList<FFlowchart>(fFlowcharts));
        TableList.SetClickItem(OnClickLeftListItem);
        
        Inspector.Clear();
        
        if (FFlowcharts.Count > 0)
        {
            SetFlowchart(FFlowcharts[0]);
        }
    }

    private void OnClickCommand(FCommand command)
    {
        Inspector.Clear();
        
        Inspector.AddDrawer(new CtlButtonDrawerBuilder()
            .SetTitle("流程图")
            .SetButtonText("返回")
            .SetOnClick(RefreshFlowchartInspector)
            .Build());
        
        if (command == null)
            return;

        Inspector.AddDrawer(new CtlGroupDrawerBuilder()
            .SetTitle("Command信息".I18NTodo())
            .SetExpand(true)
            .AddDrawer(new CtlInfoDrawerBuilder()
                .SetTitle("ID".I18NTodo())
                .SetContent(command.ItemID.ToString())
                .Build())
            .AddDrawer(new CtlInfoDrawerBuilder()
                .SetTitle("类型".I18NTodo())
                .SetContent(command.CmdType)
                .Build())
            .AddDrawer(new CtlInfoDrawerBuilder()
                .SetTitle("描述".I18NTodo())
                .SetContent(command.GetSummary())
                .Build())
            .Build());
    }
    
    private void OnClickLeftListItem(EventContext context)
    {
        if(TableList.SelectedIndex < 0 || TableList.SelectedIndex >= FFlowcharts.Count)
            return;
        
        SetFlowchart(FFlowcharts[TableList.SelectedIndex]);
    }
    
    private void SetFlowchart(FFlowchart fFlowchart)
    {
        CurFlowchart = fFlowchart;
        
        NodeCanvas.Bind(fFlowchart);
        NodeCanvas.FocusToFirstBlock();
        
        RefreshFlowchartInspector();
    }

    private void RefreshFlowchartInspector()
    {
        var fFlowchart = CurFlowchart;
        
        Inspector.Clear();
        
        // 添加FFlowchart的信息
        Inspector.AddDrawer(new CtlGroupDrawerBuilder()
            .SetTitle("流程图信息".I18NTodo())
            .AddDrawer(new CtlInfoDrawerBuilder()
                .SetTitle("ID".I18NTodo())
                .SetContent(fFlowchart.Name)
                .Build())
            .AddDrawer(new CtlInfoDrawerBuilder()
                .SetTitle("Block数量".I18NTodo())
                .SetContent(fFlowchart.Blocks.Count.ToString())
                .Build())
            .Build());
        
        // 添加FFlowchart的Variables
        var groupDrawerBuilder = new CtlGroupDrawerBuilder()
            .SetTitle("Variables".I18NTodo());

        foreach (var fVariable in fFlowchart.Variables)
        {
            groupDrawerBuilder.AddDrawer(new CtlInfoDrawerBuilder()
                .SetTitle(fVariable.Key)
                .SetContent(fVariable.Value)
                .SetTooltips($"变量名：\t{fVariable.Key}\n" +
                             $"变量类型：\\t{fVariable.Type}\n" +
                             $"变量值：\\t{fVariable.Value}")
                .Build());
        }
        
        Inspector.AddDrawer(groupDrawerBuilder.Build());
    }
}