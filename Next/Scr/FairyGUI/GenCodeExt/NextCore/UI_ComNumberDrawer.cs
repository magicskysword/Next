using System;
using System.Collections.Generic;
using SkySwordKill.NextModEditor.Mod;

namespace SkySwordKill.NextFGUI.NextCore
{
    public partial class UI_ComNumberDrawer
    {
        public void SetInputRestrict(string regex)
        {
            m_inContent.restrict = regex;
        }
        
        public void BindIntEndEdit(Action<int> endEdit)
        {
            SetInputRestrict("[0-9-]");
            m_inContent.onFocusOut.Set(() =>
            {
                var str = m_inContent.text;
                if(int.TryParse(str,out var num))
                {
                    endEdit?.Invoke(num);
                    Warning = false;
                }
                else
                {
                    Warning = true;
                }
            });
        }
        
        public void BindFloatEndEdit(Action<float> endEdit)
        {
            SetInputRestrict("[0-9-.eE]");
            m_inContent.onFocusOut.Set(() =>
            {
                var str = m_inContent.text;
                if(float.TryParse(str,out var num))
                {
                    endEdit?.Invoke(num);
                    Warning = false;
                }
                else
                {
                    Warning = true;
                }
            });
        }
        
        public void BindArrayEndEdit(Action<List<int>> endEdit)
        {
            SetInputRestrict("[0-9-,，]");
            m_inContent.onFocusOut.Set(() =>
            {
                var strArr = m_inContent.text;
                if(strArr.TryFormatToListInt(out var list))
                {
                    endEdit?.Invoke(list);
                    Warning = false;
                }
                else
                {
                    Warning = true;
                }
            });
        }

        public bool Warning
        {
            get => GetController("warning").selectedIndex == 1;
            set => GetController("warning").selectedIndex = value ? 1 : 0;
        }

        public void SetEditable(bool value)
        {
            grayed = !value;
            m_inContent.editable = value;
            m_inContent.cursor = value ? "text" : string.Empty;
        }
    }
}