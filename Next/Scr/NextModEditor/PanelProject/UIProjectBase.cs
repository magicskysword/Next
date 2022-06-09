namespace SkySwordKill.NextEditor.PanelProject
{
    public abstract class UIProjectBase
    {
        public string Name { get; set; }
        public int Layer { get; set; }
        public abstract bool IsLeaf { get; }
    }
}