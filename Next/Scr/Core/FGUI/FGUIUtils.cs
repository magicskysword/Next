using System;
using FairyGUI;
using UnityEngine;

namespace SkySwordKill.Next.FGUI
{
    public static class FGUIUtils
    {
        public static T As<T>(this GObject obj) where T : GObject
        {
            //Main.LogDebug($"Type : {component.GetType()} to Type : {typeof(T)}");
            return obj as T;
        }

        public static void BindHSeg(GObject seg, Func<float> xMinGetter, Func<float> xMaxGetter)
        {
            void OnDragMove(EventContext context)
            {
                var pos = new Vector2(context.inputEvent.x, 0);
                seg.x = Mathf.Clamp(seg.parent.RootToLocal(pos, null).x, xMinGetter.Invoke(), xMaxGetter.Invoke());
            }

            void OnDragStart(EventContext context)
            {
                context.PreventDefault();
                DragDropManager.inst.StartDrag(null, null, null, (int)context.data);

                DragDropManager.inst.dragAgent.onDragMove.Set((EventCallback1)OnDragMove);
            }

            seg.draggable = true;
            seg.onDragStart.Set((EventCallback1)OnDragStart);
            seg.cursor = "resizeH";
        }
    }
}