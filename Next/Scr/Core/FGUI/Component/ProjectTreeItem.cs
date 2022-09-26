using System;
using System.Collections.Generic;

namespace SkySwordKill.Next.FGUI.Component;

public abstract class ProjectTreeItem : ProjectTreeNodeBase, IProjectItem
{
    public abstract string ID { get; }
    public abstract PanelPageBase CreatePage();
    public override bool IsLeaf => true;
    public override List<ProjectTreeNodeBase> Children { get; } = null;
}