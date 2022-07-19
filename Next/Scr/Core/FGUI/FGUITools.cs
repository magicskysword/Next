using System;
using FairyGUI;
using UnityEngine;

namespace SkySwordKill.Next.FGUI
{
    public static class FGUITools
    {
        public static T As<T>(this GObject obj) where T : GObject
        {
            //Main.LogDebug($"Type : {component.GetType()} to Type : {typeof(T)}");
            return obj as T;
        }

        public static void BindHSeg(GObject seg, Func<float> xMinGetter, Func<float> xMaxGetter)
        {
            EventCallback1 onDragMove = context =>
            {
                var posX = context.inputEvent.x;
                posX = Mathf.Clamp(posX, xMinGetter.Invoke(), xMaxGetter.Invoke());
                seg.x = posX;
            };
            
            EventCallback1 onDragStart = context =>
            {
                context.PreventDefault();
                DragDropManager.inst.StartDrag(null, null,null, (int)context.data);
                
                DragDropManager.inst.dragAgent.onDragMove.Set(onDragMove);
            };
            
            seg.draggable = true;
            seg.onDragStart.Set(onDragStart);
            seg.cursor = "resizeH";
        }
    }
}