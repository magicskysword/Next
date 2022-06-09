using System;
using FairyGUI;
using UnityEngine;

namespace SkySwordKill.NextFGUI.NextCore
{
    public partial class UI_LabelTableGridHeader
    {
        private TableInfo _info;
        private Action _onChangeWidth;

        public void BindTableInfo(TableInfo info,Action onChangeWidth)
        {
            _info = info;
            _onChangeWidth = onChangeWidth;

            m_dragable.draggable = true;
            m_dragable.cursor = "resizeH";
            m_dragable.onDragStart.Set(OnDragSegStart);
        }

        private void OnDragSegStart(EventContext context)
        {
            context.PreventDefault();
            DragDropManager.inst.StartDrag(null, null,null, (int)context.data);
            DragDropManager.inst.dragAgent.onDragMove.Set(OnDragSegMove);
        }

        private void OnDragSegMove(EventContext context)
        {
            var posX = context.inputEvent.x;
            var dragWidth = posX - LocalToGlobal(Vector2.zero).x;
            dragWidth = Mathf.Clamp(dragWidth, 
                TableInfo.DEFAULT_GRID_WIDTH / 4, 
                TableInfo.DEFAULT_GRID_WIDTH * 10);

            width = dragWidth;
            _info.Width = dragWidth;
            _onChangeWidth.Invoke();
        }
    }
}