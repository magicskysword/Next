using System.Collections.Generic;

namespace SkySwordKill.Next.FGUI.Component;

public abstract class ProjectTreeFolder : ProjectTreeNodeBase
{
    public override bool IsLeaf => false;
    protected override List<ProjectTreeNodeBase> Children { get; } = new List<ProjectTreeNodeBase>();
    public IReadOnlyList<ProjectTreeNodeBase> AllChildren => Children;
    public virtual void AddChild(ProjectTreeNodeBase child)
    {
        child.Parent = this;
        Children.Add(child);
        BindNode.AddChild(child.BindNode);
    }
    
    public virtual void RemoveChild(ProjectTreeNodeBase child)
    {
        child.Parent = null;
        Children.Remove(child);
        BindNode.RemoveChild(child.BindNode);
    }
    
    public virtual void InsertChild(int index, ProjectTreeNodeBase child)
    {
        child.Parent = this;
        Children.Insert(index, child);
        BindNode.AddChildAt(child.BindNode, index);
    }
    
    public virtual void RemoveChildAt(int index)
    {
        var child = Children[index];
        child.Parent = null;
        Children.RemoveAt(index);
        BindNode.RemoveChild(child.BindNode);
    }

    public virtual void RemoveAllChildren()
    {
        foreach (var child in Children)
        {
            child.Parent = null;
        }
        Children.Clear();
        BindNode.RemoveChildren();
    }
}