using System;
using System.Collections.Generic;
using System.Linq;
using FairyGUI;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.Next.FGUI.Component
{
    public class CtlComboSearchBox
    {
        public CtlComboSearchBox(UI_ComboSearchBox searchBox)
        {
            MainView = searchBox;
            MainView.onChanged.Add(OnMainViewChanged);
            Dropdown = (UI_ComboSearchBox_popup)MainView.dropdown;
            SearchBox = new CtlToolsSearchBox(Dropdown.m_searchBox);
            
            SearchBox.OnSearch = OnSearch;
        }

        public UI_ComboSearchBox MainView { get; set; }
        public UI_ComboSearchBox_popup Dropdown { get; set; }
        public CtlToolsSearchBox SearchBox { get; set; }
        
        public int SelectedIndex
        {
            get => GetSelectedIndex();
            set => SetSelectedIndex(value);
        }

        public Action OnChanged { get; set; } = () => { };
        private List<string> _items = new List<string>();
        private List<string> _searchItems = new List<string>();

        public void SetItems(IEnumerable<string> items)
        {
            _items.Clear();
            _items.AddRange(items);
            SearchBox.SearchContent = string.Empty;
            ClearSearch();
        }
        
        private void ClearSearch()
        {
            SearchBox.SearchContent = string.Empty;
            MainView.items = _items.ToArray();
            _searchItems = _items;
        }
        
        private void OnMainViewChanged(EventContext context)
        {
            var curItem = _searchItems[MainView.selectedIndex];
            var trueIndex = _items.IndexOf(curItem);
            ClearSearch();
            SetSelectedIndex(trueIndex);
            OnChanged?.Invoke();
        }
        
        private void OnSearch(string searchText)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                _searchItems = _items;
            }
            else
            {
                _searchItems = _items.FindAll(item => item.Contains(searchText)).ToList();
            }
            ResetDropdownItems();
        }

        private void ResetDropdownItems()
        {
            var list = Dropdown.m_list;
            list.RemoveChildrenToPool();
            Main.LogInfo($"搜索到{_searchItems.Count}个结果");
            int cnt = _searchItems.Count;
            for (int i = 0; i < cnt; i++)
            {
                GObject item = list.AddItemFromPool();
                item.text = _searchItems[i];
                item.icon = string.Empty;
                item.name = i.ToString();
            }
            list.ResizeToFit(UIConfig.defaultComboBoxVisibleItemCount);
        }

        public void SetEditable(bool value)
        {
            MainView.enabled = value;
        }
        
        private void SetSelectedIndex(int value)
        {
            MainView.selectedIndex = value;
        }

        private int GetSelectedIndex()
        {
            return MainView.selectedIndex;
        }
    }
}