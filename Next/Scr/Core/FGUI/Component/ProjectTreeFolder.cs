using System.Collections.Generic;

namespace SkySwordKill.Next.FGUI.Component
{
    public class ProjectTreeFolder : ProjectTreeNodeBase
    {
        public override bool IsLeaf => false;
        public bool IsExpended { get; set; }
    }
}