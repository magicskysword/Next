using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FairyGUI;
using SkySwordKill.Next.FGUI.Dialog;
using SkySwordKill.Next.FGUI;
using SkySwordKill.NextFGUI.NextCore;
using SkySwordKill.NextModEditor.Mod.Data;

namespace SkySwordKill.Next.FGUI.Component
{
    public class CtlIntBindTablePropertyDrawer : CtlPropertyDrawerBase
    {
        private string _drawerName;
        private UI_ComNumberBindDataDrawer Drawer => (UI_ComNumberBindDataDrawer)Component;
        private Action<int> _setter;
        private Func<int> _getter;
        private Func<int, string> _descGetter;
        private List<TableInfo> _tableInfos;
        private Func<List<IModData>> _dataListGetter;

        public CtlIntBindTablePropertyDrawer(string drawerName, Action<int> setter, Func<int> getter,
            Func<int, string> descGetter, List<TableInfo> tableInfos, Func<List<IModData>> dataListGetter)
        {
            _drawerName = drawerName;
            _setter = setter;
            _getter = getter;
            _descGetter = descGetter;
            _tableInfos = tableInfos;
            _dataListGetter = dataListGetter;
        }

        protected override GComponent OnCreateCom()
        {
            var drawer = UI_ComNumberBindDataDrawer.CreateInstance();
            drawer.m_btnEdit.onClick.Set(OnClickEdit);
            drawer.BindEndEdit(OnSetProperty);
            drawer.title = _drawerName;
            return drawer;
        }

        protected override void OnRefresh()
        {
            Drawer.m_inContent.text = OnGetProperty().ToString();
            Drawer.m_txtDesc.text = OnGetDesc() ?? "";
        }

        protected override void SetDrawerEditable(bool value)
        {
            Drawer.SetEditable(value);
        }

        private void OnSetProperty(int value)
        {
            _setter.Invoke(value);
            OnChanged?.Invoke();
        }

        private int OnGetProperty()
        {
            return _getter.Invoke();
        }

        private string OnGetDesc()
        {
            var id = _getter.Invoke();

            return _descGetter.Invoke(id);
        }

        private void OnClickEdit()
        {
            WindowTableSelectorDialog.CreateDialog(_drawerName, _tableInfos,
                new[] { OnGetProperty() }, false, _dataListGetter.Invoke(), false,
                list =>
                {
                    OnSetProperty(list.FirstOrDefault());
                    Refresh();
                });
        }
    }
}