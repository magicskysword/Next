using System;
using System.Collections.Generic;
using System.Linq;
using FairyGUI;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.Next.FGUI.Component
{
    public class CtlDropdownPropertyDrawer : CtlPropertyDrawerBase
    {
        private string _drawerName;
        private Func<IEnumerable<string>> _optionsGetter;
        private readonly Action<int> _indexSetter;
        private readonly Func<int> _indexGetter;

        private UI_ComDropdownDrawer Drawer => (UI_ComDropdownDrawer)Component;
        private CtlComboSearchBox SearchBox { get; set; }

        public CtlDropdownPropertyDrawer(string drawerName, Func<IEnumerable<string>> optionsGetter,
            Action<int> indexSetter, Func<int> indexGetter)
        {
            _drawerName = drawerName;
            _optionsGetter = optionsGetter;
            _indexSetter = indexSetter;
            _indexGetter = indexGetter;
        }

        protected override GComponent OnCreateCom()
        {
            var drawer = UI_ComDropdownDrawer.CreateInstance();
            drawer.title = _drawerName;
            SearchBox = new CtlComboSearchBox(drawer.m_searchDropdown);
            SearchBox.SetItems(_optionsGetter.Invoke().ToArray()); 
            SearchBox.SelectedIndex = OnGetPropertyIndex();
            SearchBox.OnChanged += OnDropdownChange;
            return drawer;
        }

        private void OnSetPropertyIndex(int value)
        {
            this.Record(new ValueChangedCommand<int>(OnGetPropertyIndex(), value, _indexSetter));
            OnChanged?.Invoke();
        }

        private int OnGetPropertyIndex()
        {
            return _indexGetter.Invoke();
        }

        private void OnDropdownChange()
        {
            OnSetPropertyIndex(SearchBox.SelectedIndex);
        }

        protected override void OnRefresh()
        {
            SearchBox.SelectedIndex = OnGetPropertyIndex();
        }

        protected override void SetDrawerEditable(bool value)
        {
            SearchBox.SetEditable(value);
        }
    }
}