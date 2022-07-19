using System;
using System.Collections.Generic;

namespace SkySwordKill.Next.FGUI.Component
{
    public abstract class ProjectTreeItem : ProjectTreeNodeBase
    {
        public virtual string ID => this.GetType().FullName;
        public abstract PanelPageBase CreatePage();
        public override bool IsLeaf => true;
        public virtual string Icon { get; } = String.Empty;
    }
}