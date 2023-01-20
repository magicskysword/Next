using System.Collections.Generic;

namespace SkySwordKill.Next.FGUI.Component;

public abstract class ProjectTreeItem : ProjectTreeNodeBase
{
    public override bool IsLeaf => true;
    protected override List<ProjectTreeNodeBase> Children { get; } = null;
}