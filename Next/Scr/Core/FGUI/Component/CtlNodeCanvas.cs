using System;
using System.Collections.Generic;
using FairyGUI;
using SkySwordKill.Next.FCanvas;
using SkySwordKill.Next.Utils;
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
    
    public Vector2 PosScale { get; set; } = new Vector2(1, 1);
    
    private Action<FCommand> OnClickCommandCallback { get; set; }

    public CtlNodeCanvas(UI_ComNodeCanvas com)
    {
        MainView = com;

        //MainView.m_comInner.on
        MainView.draggable = true;
        MainView.onDragStart.Add(context =>
        {
            context.PreventDefault();
            if(context.inputEvent.button == 0)
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
            
        var mouseRoll = -context.inputEvent.mouseWheelDelta * 0.2f;
        var scale = Zoom.scaleX;
        if (scale <= 1f)
            mouseRoll /= 3;
        //Main.LogDebug(mouseRoll);
        scale = Mathf.Clamp(scale + mouseRoll, 0.2f, 4f);
        GTween.Kill(Zoom);
        Zoom.TweenScale(new Vector2(scale, scale), 0.3f);
    }


    public void Resize()
    {
        int minX = 0;
        int minY = 0;
        int maxX = 0;
        int maxY = 0;

        if (Container.numChildren > 0)
        {
            var defaultChild = Container.GetChildAt(0);
            minX = (int)defaultChild.x;
            maxX = (int)(defaultChild.x + defaultChild.width);
            minY = (int)defaultChild.y;
            maxY = (int)(defaultChild.y + defaultChild.height);
        }
        
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
        block.ResetPosition(notifyResize, PosScale);
        block.RefreshBlock();
        block.SetOnClickCommand(OnClickCommand);
        Main.LogDebug($"添加Block：{block.FBlock.Name}，位置：{block.MainView.x},{block.MainView.y}");
    }
    
    public void SetOnClickCommand(Action<FCommand> callback)
    {
        OnClickCommandCallback = callback;
    }

    private void OnClickCommand(FCommand obj)
    {
        OnClickCommandCallback?.Invoke(obj);
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
    
    public void FocusToFirstBlock()
    {
        if (Blocks.Count > 0)
        {
            var block = Blocks.MinBy(b => b.MainView.x);
            SetPosition(block, new Vector2(200, 200));
        }
    }
    
    public void SetPosition(float x, float y)
    {
        Container.SetXY(x, y);
    }
    
    public void SetPosition(CtlNodeBlock block, Vector2 offset = default)
    {
        if(block?.MainView == null)
            return;
        
        // block的坐标和画布坐标是相反的
        var pos = new Vector2(-block.MainView.x, -block.MainView.y);
        pos += offset;
        
        Container.SetXY(pos.x, pos.y);
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