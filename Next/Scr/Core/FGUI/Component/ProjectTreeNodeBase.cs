using System.Collections.Generic;
using FairyGUI;

namespace SkySwordKill.Next.FGUI.Component
{
    public abstract class ProjectTreeNodeBase
    {
        public string Name { get; set; }
        public int Layer { get; set; }
        public abstract bool IsLeaf { get; }
    }
}