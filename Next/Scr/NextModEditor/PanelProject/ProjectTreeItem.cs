using System;
using SkySwordKill.NextEditor.PanelPage;

namespace SkySwordKill.NextEditor.PanelProject
{
    public abstract class ProjectTreeItem : ProjectTreeBase
    {
        public virtual string ID => this.GetType().FullName;
        public abstract PanelPageBase CreatePage();
        public override bool IsLeaf => true;
        public virtual string Icon { get; } = String.Empty;
    }
}