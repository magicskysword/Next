using FairyGUI;
using SkySwordKill.Next.FGUI;
using UnityEngine;

namespace SkySwordKill.Next.FGUI;

public abstract class WindowDialogBase : FGUIWindowBase
{
    protected WindowDialogBase(string pkgName, string comName) : base(pkgName, comName)
    {
        modal = true;
    }
        
    public Vector2 MinSize { get; set; } = new Vector2(20, 20);
    
    protected override void DoShowAnimation()
    {
        base.DoShowAnimation();
        pivot = new Vector2(0.5f, 0.5f);
        alpha = 0f;
        scale = Vector2.zero;
        Center();
        TweenScale(Vector2.one, 0.3f).SetEase(EaseType.CubicOut);
        TweenFade(1f, 0.3f).OnComplete(OnShown);
    }

    protected override void OnInit()
    {
        base.OnInit();
        BindResizeHandle();
    }

    private Vector2 _startPos = Vector2.zero;
    private Vector2 _originSize = Vector2.zero;

    private void BindResizeHandle()
    {
        MinSize = contentPane.size;

        void OnDragStartEvent(EventContext context)
        {
            context.PreventDefault();
            DragDropManager.inst.StartDrag(null, null, null, (int)context.data);
            DragDropManager.inst.dragAgent.onDragMove.Set(OnDragMoveEvent);
            _startPos = DragDropManager.inst.dragAgent.position;
            _originSize = contentPane.size;
        }
            
        void OnDragMoveEvent(EventContext context)
        {
            var dragObj = (GObject)context.sender;
            var curSize = _originSize + ((Vector2)dragObj.position - _startPos);

            if (curSize.x < MinSize.x) curSize.x = MinSize.x;
            if (curSize.y < MinSize.y) curSize.y = MinSize.y;

            contentPane.size = curSize;
        }

        var ctlResize = frame.GetController("canResize");
        if (ctlResize != null)
        {
            var resizeHandle = frame.GetChild("resizeHandle");
            if (resizeHandle != null)
            {
                resizeHandle.cursor = FGUIManager.MOUSE_RESIZE_BR;
                resizeHandle.draggable = true;
                resizeHandle.onDragStart.Add(OnDragStartEvent);
            }
        }
    }
}