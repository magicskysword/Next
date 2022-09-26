using System;
using System.Collections.Generic;
using FairyGUI;
using SkySwordKill.Next;
using SkySwordKill.Next.FCanvas;
using SkySwordKill.NextFGUI.NextCore;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SkySwordKill.Next.FGUI.Component;

public class CtlNodeCanvas
{
    /// <summary>
    /// 画布边缘宽度
    /// </summary>
    private int CANVAS_EDGE = 400;

    public UI_ComNodeCanvas MainView { get; set; }
    public List<CtlNodeBlock> Blocks { get; set; } = new List<CtlNodeBlock>();
    public Action OnContentPosChange { get; set; }
    public Margin Margin { get; set; }

    public GComponent Container => MainView.m_zoom.m_container;
    public GComponent Zoom => MainView.m_zoom;

    public CtlNodeCanvas(UI_ComNodeCanvas com)
    {
        MainView = com;

        //MainView.m_comInner.on
        MainView.draggable = true;
        MainView.onDragStart.Add(context =>
        {
            context.PreventDefault();
            if(context.inputEvent.button != 2)
                return;
            Container.StartDrag((int)context.data);
        });
            
        Container.onPositionChanged.Add(context =>
        {
            var posX = Mathf.Clamp(Container.x, Margin.left, Margin.right);
            var posY = Mathf.Clamp(Container.y, Margin.top, Margin.bottom);
            Container.SetXY(posX,posY);
            OnContentPosChange?.Invoke();
        });

        MainView.onKeyDown.Add(context =>
        {
            if (context.inputEvent.keyCode == KeyCode.Space)
            {
                var block = Blocks[Random.Range(0, Blocks.Count)];
                // Container里坐标是负的，进行一次转换
                Container.SetXY(-block.MainView.x, -block.MainView.y);
            }
        });
            
        MainView.onRollOver.Add(context =>
        {
            Stage.inst.onMouseWheel.Add(OnScrollChanged);
        });
            
        MainView.onRollOut.Add(context =>
        {
            Stage.inst.onMouseWheel.Remove(OnScrollChanged);
        });
    }

    private void OnScrollChanged(EventContext context)
    {
        if(!context.inputEvent.ctrlOrCmd)
            return;
            
        var mouseRoll = -context.inputEvent.mouseWheelDelta * 0.1f;
        var scale = Zoom.scaleX;
        if (scale <= 1f)
            mouseRoll /= 3;
        Main.LogDebug(mouseRoll);
        scale = Mathf.Clamp(scale + mouseRoll, 0.1f, 4f);
        GTween.Kill(Zoom);
        Zoom.TweenScale(new Vector2(scale, scale), 0.3f);
    }


    public void Resize()
    {
        int minX = 0;
        int minY = 0;
        int maxX = 0;
        int maxY = 0;
        // 计算新边缘大小
        foreach (var child in Container.GetChildren())
        {
            if (child.x < minX)
            {
                minX = (int)child.x;
            }
            if (child.x + child.width > maxX)
            {
                maxX = (int)(child.x + child.width);
            }
            if (child.y < minY)
            {
                minY = (int)child.y;
            }
            if (child.y + child.height > maxY)
            {
                maxY = (int)(child.y + child.height);
            }
        }
            
        minX -= CANVAS_EDGE;
        maxX += CANVAS_EDGE;
        minY -= CANVAS_EDGE;
        maxY += CANVAS_EDGE;
            
        Margin = new Margin()
        {
            left = -maxX,
            right = -minX,
            top = -maxY,
            bottom = -minY
        };
    }

    public void AddBlock(CtlNodeBlock block,bool notifyResize = true)
    {
        block.OnPosChanged = Resize;
        Blocks.Add(block);
        Container.AddChild(block.MainView);
        block.ResetPosition(notifyResize);
        block.RefreshBlock();
        Main.LogDebug($"添加Block：{block.FBlock.Name}，位置：{block.MainView.x},{block.MainView.y}");
            
    }
        
    public void RemoveBlock(CtlNodeBlock block, bool notifyResize = true)
    {
        block.OnPosChanged = null;
        Blocks.Remove(block);
        Container.RemoveChild(block.MainView);
        if(notifyResize)
            Resize();
    }

    public void Bind(FFlowchart fFlowchart)
    {
        Clear();
            
        foreach (var fBlock in fFlowchart.Blocks)
        {
            var block = new CtlNodeBlock(UI_ComNodeBlock.CreateInstance(), fBlock);
            AddBlock(block, false);
        }

        Container.SetXY(0, 0);
            
        Resize();
        Main.LogDebug($"当前Block数量：{Blocks.Count}");
        Main.LogDebug($"当前margin：l:{Margin.left} r:{Margin.right} t:{Margin.top} b:{Margin.bottom}");
    }

    public void Clear()
    {
        foreach (var block in Blocks)
        {
            block.OnPosChanged = null;
            block.MainView.Dispose();
        }
        Blocks.Clear();
        Container.RemoveChildren();
    }
}