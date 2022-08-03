using System;
using FairyGUI;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.Next.FGUI.Component
{
    public class CtlHeaderBar
    {
        public CtlHeaderBar(UI_ComMainHeader header)
        {
            Header = header;
        }
        
        public UI_ComMainHeader Header { get; set; }

        public void AddMenu(string name, Func<PopupMenu> popupBuilder)
        {
            var btnFile = Header.m_lstHeader.AddItemFromPool().asButton;
            btnFile.title = name;
            btnFile.onClick.Add(() => OnClickMenu(btnFile, popupBuilder.Invoke()));
        }

        private void OnClickMenu(GObject obj,PopupMenu popup)
        {
            popup.Show(obj, PopupDirection.Down);
        }
    }
}