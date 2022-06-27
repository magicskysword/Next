namespace SkySwordKill.NextEditor.PanelProject
{
    public abstract class ProjectTreeBase
    {
        public string Name { get; set; }
        public int Layer { get; set; }
        public abstract bool IsLeaf { get; }
    }
}