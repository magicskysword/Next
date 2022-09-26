using System;
using System.Collections.Generic;
using FairyGUI;

namespace SkySwordKill.Next.FGUI.Component;

public abstract class ProjectTreeNodeBase : ProjectBase
{
    public abstract bool IsLeaf { get; }
    public abstract List<ProjectTreeNodeBase> Children { get; }
}