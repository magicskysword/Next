using System.Collections.Generic;

namespace SkySwordKill.Next.FGUI.Component
{
    public abstract class ProjectTreeFolder : ProjectTreeNodeBase
    {
        public override bool IsLeaf => false;
        public override List<ProjectTreeNodeBase> Children { get; } = new List<ProjectTreeNodeBase>();
    }
}