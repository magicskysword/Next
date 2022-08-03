using System;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.Next.FGUI.Component
{
    public class CtlToolsSearchBox
    {
        public CtlToolsSearchBox(UI_ComToolsSearchBox searchBox)
        {
            SearchBox = searchBox;
            SearchBox.m_btnSearch.onClick.Add(OnClickSearch);
            SearchBox.m_btnReset.onClick.Add(OnClickReset);
            
            SearchBox.m_inContent.onSubmit.Add(OnClickSearch);
        }
        
        public UI_ComToolsSearchBox SearchBox { get; }
        public Action<string> OnSearch { get; set; }

        public string SearchContent
        {
            get => SearchBox.m_inContent.text;
            set => SearchBox.m_inContent.text = value;
        }

        private void OnClickSearch()
        {
            OnSearch?.Invoke(SearchContent);
        }
        
        private void OnClickReset()
        {
            SearchContent = string.Empty;
            OnSearch?.Invoke(string.Empty);
        }
    }
}