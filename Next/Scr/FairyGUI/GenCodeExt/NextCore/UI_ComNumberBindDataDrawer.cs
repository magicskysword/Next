using System;
using System.Collections.Generic;
using SkySwordKill.NextModEditor.Mod;

namespace SkySwordKill.NextFGUI.NextCore
{
    public partial class UI_ComNumberBindDataDrawer
    {
        public void BindEndEdit(Action<int> endEdit)
        {
            m_inContent.restrict = @"[0-9-]";
            var ctrl = GetController("warning");
            
            m_inContent.onFocusOut.Set(() =>
            {
                var str = m_inContent.text;
                if(int.TryParse(str,out var num))
                {
                    endEdit?.Invoke(num);
                    ctrl.selectedIndex = 0;
                }
                else
                {
                    ctrl.selectedIndex = 1;
                }
            });
        }

        public delegate void OnEndMultiDataEdit(bool isParseSuccess,List<int> ids);
        
        public void BindEndMultiEdit(OnEndMultiDataEdit onEndEdit)
        {
            m_inContent.restrict = @"[0-9-,，]";
            var ctrl = GetController("warning");
            
            m_inContent.onFocusOut.Set(() =>
            {
                var str = m_inContent.text.Replace("，",",");
                if(str.TryFormatToListInt(out var list))
                {
                    ctrl.selectedIndex = 0;
                    onEndEdit?.Invoke(true,list);
                }
                else
                {
                    ctrl.selectedIndex = 1;
                    onEndEdit?.Invoke(false,list);
                }
            });
        }

        public void SetEditable(bool value)
        {
            grayed = !value;
            m_inContent.editable = value;
            m_inContent.cursor = value ? "text" : string.Empty;
        }
    }
}