using System.Collections.Generic;
using UnityEngine;

namespace SkySwordKill.NextFGUI.NextCore
{
    public partial class UI_ComTableRow
    {
        public void RefreshItem(IEnumerable<TableInfo> infos,object getData,float frameWidth)
        {
            m_list.RemoveChildrenToPool();
            foreach (var tableItemInfo in infos)
            {
                var item = m_list.AddItemFromPool().asLabel;
                item.title = tableItemInfo.Getter.Invoke(getData);
                item.width = tableItemInfo.Width;
            }
            m_list.ResizeToFit();
            width = Mathf.Max(m_list.width, frameWidth);
        }
    }
}