using System;
using FairyGUI;
using Fungus;
using SkySwordKill.Next.FCanvas;
using SkySwordKill.NextFGUI.NextCore;
using UnityEngine;

namespace SkySwordKill.Next.FGUI.Component;

public class CtlNodeBlock
{
    public CtlNodeBlock(UI_ComNodeBlock com,FBlock fBlock)
    {
        MainView = com;
        FBlock = fBlock;

        MainView.title = $"{fBlock.ItemID}[{fBlock.Name}]";
        Content = MainView.m_content;

        MainView.m_dragArea.draggable = true;
        MainView.m_dragArea.onDragStart.Add(context =>
        {
            context.PreventDefault();
            MainView.StartDrag((int)context.data);
        });
        MainView.onDragEnd.Add(() =>
        {
            OnPosChanged ?.Invoke();
        });
        
        Content.onClickItem.Add((context) =>
        {
            var item = context.data as GObject;
            if (item == null)
                return;
            
            var index = Content.GetChildIndex(item);
            if (index < 0 || index >= FBlock.Commands.Count)
                return;
            
            var command = FBlock.Commands[index];
            OnCommandClick?.Invoke(command);
        });
    }
        
    public UI_ComNodeBlock MainView { get; set; }
    public FBlock FBlock { get; set; }
    public Action OnPosChanged { get; set; }
    public GList Content { get; set; }
    
    protected Action<FCommand> OnCommandClick { get; set; }
    
    public void SetOnClickCommand(Action<FCommand> action)
    {
        OnCommandClick = action;
    }

    public void RefreshBlock()
    {
        int indent = 0;
        int addIndent = 0;
        int maxIndent = 0;
        
        Content.numItems = 0;
        for (var index = 0; index < FBlock.Commands.Count; index++)
        {
            var command = FBlock.Commands[index];
            GComponent item;
            switch (command)
            {
                default:
                {
                    item = Content.AddItemFromPool("ui://NextCore/ComCmdText").asCom;
                    item.text = command.GetSummary();
                    break;
                }
            }

            switch (command)
            {
                case FCanvas.FakerCommand.While:
                case FCanvas.FakerCommand.If:
                {
                    addIndent++;
                    break;
                }
                case FCanvas.FakerCommand.ElseIf:
                {
                    indent--;
                    addIndent++;
                    break;
                }
                case FCanvas.FakerCommand.Else:
                {
                    indent--;
                    addIndent++;
                    break;
                }
                case FCanvas.FakerCommand.End:
                {
                    indent--;
                    break;
                }
            }

            // 间隔背景颜色
            var controller = item.GetController("bg");
            if(controller != null)
                controller.selectedIndex = index % 2;
            
            // 间距排版
            var indentObj = item.GetChild("indent");
            if (indentObj != null)
            {
                indentObj.width = indent * 20;
            }
            maxIndent = Math.Max(maxIndent, indent);
            indent += addIndent;
            addIndent = 0;
        }
        Content.width += maxIndent * 20;
        Content.ResizeToFit();
    }

    public void ResetPosition(bool notifyResize, Vector2 posScale)
    {
        SetPosition(FBlock.Position.x * posScale.x, FBlock.Position.y * posScale.y, notifyResize);
    }

    public void SetPosition(float x, float y,bool notifyResize = true)
    {
        MainView.x = x;
        MainView.y = y;
        if (notifyResize)
            OnPosChanged?.Invoke();
    }
}