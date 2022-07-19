using System;
using FairyGUI;
using Fungus;
using SkySwordKill.Next.FCanvas;
using SkySwordKill.NextFGUI.NextCore;
using Say = SkySwordKill.Next.FCanvas.FakerCommand.Say;

namespace SkySwordKill.Next.FGUI.Component
{
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
        }
        
        public UI_ComNodeBlock MainView { get; set; }
        public FBlock FBlock { get; set; }
        public Action OnPosChanged { get; set; }
        public GList Content { get; set; }

        public void RefreshBlock()
        {
            Content.numItems = 0;
            for (var index = 0; index < FBlock.Commands.Count; index++)
            {
                var command = FBlock.Commands[index];
                GLabel item;
                if (command is Say cmdSay)
                {
                    item = Content.AddItemFromPool("ui://NextCore/ComCmdText").asLabel;
                    var id = cmdSay.AvatarIDSetType == StartFight.MonstarType.Normal
                        ? cmdSay.AvatarIDInt.ToString()
                        : cmdSay.AvatarBindKey;
                    item.title = $"{id} : {cmdSay.StoryText}";
                }
                else if (command is SkySwordKill.Next.FCanvas.FakerCommand.Menu cmdMenu)
                {
                    item = Content.AddItemFromPool("ui://NextCore/ComCmdText").asLabel;
                    item.title = $"【选项】{cmdMenu.Text} => {cmdMenu.TargetBlockID}";
                }
                else if (command is SkySwordKill.Next.FCanvas.FakerCommand.If cmdIf)
                {
                    item = Content.AddItemFromPool("ui://NextCore/ComCmdText").asLabel;
                    item.title = $"if ({cmdIf.Condition})";
                }
                else if (command is SkySwordKill.Next.FCanvas.FakerCommand.ElseIf cmdElseIf)
                {
                    item = Content.AddItemFromPool("ui://NextCore/ComCmdText").asLabel;
                    item.title = $"else if ({cmdElseIf.Condition})";
                }
                else if (command is SkySwordKill.Next.FCanvas.FakerCommand.Else cmdElse)
                {
                    item = Content.AddItemFromPool("ui://NextCore/ComCmdText").asLabel;
                    item.title = "else";
                }
                else if (command is SkySwordKill.Next.FCanvas.FakerCommand.Call cmdCall)
                {
                    item = Content.AddItemFromPool("ui://NextCore/ComCmdText").asLabel;
                    item.title = $"=> {cmdCall.targetFlowchartName ?? "this"}:{cmdCall.targetBlockID}";
                }
                else
                {
                    item = Content.AddItemFromPool("ui://NextCore/ComCmdText").asLabel;
                    item.title = $"[{command.CmdType}]";
                }

                var controller = item.GetController("bg");
                if(controller != null)
                    controller.selectedIndex = index % 2;
            }
            Content.ResizeToFit();
        }

        public void ResetPosition(bool notifyResize = true)
        {
            SetPosition(FBlock.Position.x, FBlock.Position.y, notifyResize);
        }

        public void SetPosition(float x, float y,bool notifyResize = true)
        {
            MainView.x = x * 2;
            MainView.y = y * 3;
            if (notifyResize)
                OnPosChanged?.Invoke();
        }
    }
}