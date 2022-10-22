using System;
using System.Collections.Generic;
using FairyGUI;

namespace SkySwordKill.Next.FGUI.Component;

public abstract class ProjectTreeNodeBase : ProjectBase
{
    protected GTreeNode _bindNode;
    protected string _icon;

    public abstract bool IsLeaf { get; }
    protected abstract List<ProjectTreeNodeBase> Children { get; }
    public virtual ProjectTreeNodeBase Parent { get;set; }
    public virtual string ResURL { get; }
    public override string Icon => _icon;

    public virtual GTreeNode BindNode
    {
        get
        {
            if(_bindNode == null)
            {
                var node = new GTreeNode(!IsLeaf);
                node.data = this;
                node.expanded = true;
                node._resURL = ResURL;
                _bindNode = node;
            }
            
            return _bindNode;
        }
        set
        {
            _bindNode = value;
            _bindNode.data = this;
        }
    }
    
    public virtual void SetIcon(string icon)
    {
        _icon = icon;
        _bindNode.icon = icon;
    }
}